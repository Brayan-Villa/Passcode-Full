using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Renci.SshNet;
using System.Windows.Forms;

public class Passcode
{
  public SshClient Ssh = new SshClient("127.0.0.1", "root", "alpine");
  public ScpClient Scp = new ScpClient("127.0.0.1", "root", "alpine");
  public Process proceso = new Process();

  public string UDID()
  {
    try
    {
      string CMD = "Libimobiledevice\\idevice_id -l | Libimobiledevice\\awk.exe '{printf $NS}'";
      File.WriteAllText("Udid.cmd", CMD);
      proceso = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "Udid.cmd",
          UseShellExecute = false,
          RedirectStandardOutput = true,
          CreateNoWindow = true
        },
      };
      proceso.Start();
      StreamReader UniqueDeviceID = proceso.StandardOutput;
      string UDIDReturn = UniqueDeviceID.ReadToEnd();
      proceso.WaitForExit();
      return UDIDReturn;
    }
    catch(Exception Err)
    {
      MessageBox.Show(Err.Message, "ERROR");
    }
  }
  
  public void ClientSSH(string Comando)
  {
    if(Ssh.IsConnected)
    {
      Ssh.Disconnect();
      Sleep(1);
      Ssh.Connect();
    }
    else
    {
      Ssh.Connect();
    }
    Ssh.CreateCommand(Comando).Execute();
  }
  
  public void Extraction()
  {
    try
    {
      proceso = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "Libimobiledevice\\iproxy.exe",
          Arguments = "22 44",
          UseShellExecute = false,
          RedirectStandardOutput = true,
          CreateNoWindow = true
        },
      };
      proceso.Start();
      string Known = "%USERPROFILE%\\.ssh\\known_hosts";
      if(File.Exists(Known))
      {
        File.Delete(Known);
      }
      string FOLDERBackup = "Backups\\" + UDID() + "";
      string FILEBackup = "" + FOLDERBackup + "\\" + UDID() + ".tar";
      if(!Directory.Exists(FOLDERBackup))
      {
        Directory.CreateDirectory(FOLDERBackup);
      }
      else
      {
        if(File.Exists(FILEBackup))
        {
          File.Delete(FILEBackup);
        }
      }
    }
    catch(Exception Err)
    {
      MessageBox.Show(Err.Message, "ERROR");
    }
  }
  
}
