using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace frmaviraunin
{
    public partial class Form1 : Form
    {
        private string aviraInstallPath = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DetectOperatingSystem();
            CheckAviraStatus();
        }

        // ===== OPERATING SYSTEM =====
        // Source - https://stackoverflow.com/a/50224392
        // Posted by Rovann Linhalis
        // Retrieved 2026-04-02, License - CC BY-SA 4.0

        private void DetectOperatingSystem()
        {
            try
            {
                string osInfo = "";
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption, OSArchitecture FROM Win32_OperatingSystem"))
                {
                    ManagementObjectCollection information = searcher.Get();
                    if (information != null)
                    {
                        foreach (ManagementObject obj in information)
                        {
                            osInfo = obj["Caption"].ToString().Trim() + " - " + obj["OSArchitecture"].ToString();
                        }
                    }
                }
                labelOS.Text = $"🖥️ {osInfo}";
            }
            catch
            {
                labelOS.Text = "🖥️ Unable to detect OS";
            }
        }

        // ===== AVIRA DETECTION =====

        private void CheckAviraStatus()
        {
            bool isSafeMode = IsSafeMode();

            if (isSafeMode)
            {
                // Safe Mode - run unins000.exe directly
                HandleSafeMode();
            }
            else
            {
                // Normal Mode - check Avira folder
                HandleNormalMode();
            }
        }

        // Check if running in Safe Mode
        private bool IsSafeMode()
        {
            return System.Windows.Forms.SystemInformation.BootMode != BootMode.Normal;
        }

        // Handle Safe Mode scenario
        private void HandleSafeMode()
        {
            aviraInstallPath = GetAviraPath();

            if (!string.IsNullOrEmpty(aviraInstallPath))
            {
                string uninstallerPath = Path.Combine(aviraInstallPath, "unins000.exe");

                if (File.Exists(uninstallerPath))
                {
                    AppendLog("✓ Safe Mode detected");
                    AppendLog("✓ Avira found: " + aviraInstallPath);
                    AppendLog("✓ unins000.exe found");
                    AppendLog("");
                    AppendLog("→ Ready to uninstall. Press Uninstall button.");
                    buttonUninstall.Enabled = true;
                }
                else
                {
                    AppendLog("✓ Safe Mode detected");
                    AppendLog("✓ Avira folder found: " + aviraInstallPath);
                    AppendLog("✗ unins000.exe NOT found");
                    AppendLog("");
                    AppendLog("→ Avira is installed but damaged.");
                    AppendLog("→ Opening residual folder scanner...");
                    buttonUninstall.Enabled = false;
                    OpenForm3();
                }
            }
            else
            {
                AppendLog("✓ Safe Mode detected");
                AppendLog("✗ Avira folder NOT found");
                AppendLog("");
                AppendLog("→ Avira is not installed.");
                AppendLog("→ Opening residual folder scanner...");
                buttonUninstall.Enabled = false;
                OpenForm3();
            }
        }

        // Handle Normal Mode scenario
        private void HandleNormalMode()
        {
            AppendLog("Checking system...");
            AppendLog("");

            aviraInstallPath = GetAviraPath();

            if (!string.IsNullOrEmpty(aviraInstallPath))
            {
                // Avira folder found
                AppendLog("✓ Avira is installed");
                AppendLog("  Path: " + aviraInstallPath);
                AppendLog("");

                // Check for unins000.exe
                string uninstallerPath = Path.Combine(aviraInstallPath, "unins000.exe");

                if (File.Exists(uninstallerPath))
                {
                    AppendLog("✓ unins000.exe found");
                    AppendLog("");
                    AppendLog("→ System needs to reboot into Safe Mode");
                    AppendLog("  to run the uninstaller.");
                    AppendLog("");
                    AppendLog("→ Press Uninstall to reboot into Safe Mode.");
                    buttonUninstall.Enabled = true;
                    buttonUninstall.Text = "🔄\r\nSafe Mode";
                }
                else
                {
                    AppendLog("✗ unins000.exe NOT found");
                    AppendLog("");
                    AppendLog("→ Avira is installed but the uninstaller");
                    AppendLog("  is missing or damaged.");
                    AppendLog("→ Please repair Avira first, then uninstall.");
                    buttonUninstall.Enabled = false;
                }
            }
            else
            {
                // Avira folder not found
                AppendLog("✗ Avira is NOT installed on this system");
                AppendLog("");
                buttonUninstall.Enabled = false;

                DialogResult result = MessageBox.Show(
                    "Avira is not installed on this system.\r\n\r\nDo you want to scan for residual files and registry entries?",
                    "Avira Not Found",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {
                    OpenForm3();
                }
            }
        }

        // Get Avira installation path
        private string GetAviraPath()
        {
            // x64: Avira is located in Program Files (x86)
            string path64 = @"C:\Program Files (x86)\Avira\Security";
            if (Directory.Exists(path64))
                return path64;

            // x86: Avira is located in Program Files
            string path32 = @"C:\Program Files\Avira\Security";
            if (Directory.Exists(path32))
                return path32;

            return null;
        }

        // ===== UNINSTALL BUTTON =====

        private void buttonUninstall_Click(object sender, EventArgs e)
        {
            if (IsSafeMode())
            {
                // Safe Mode - run unins000.exe
                RunUninstallerInSafeMode();
            }
            else
            {
                // Normal Mode - reboot into Safe Mode
                RebootToSafeMode();
            }
        }

        // Run uninstaller in Safe Mode
        private void RunUninstallerInSafeMode()
        {
            if (string.IsNullOrEmpty(aviraInstallPath))
                return;

            string uninstallerPath = Path.Combine(aviraInstallPath, "unins000.exe");

            if (!File.Exists(uninstallerPath))
            {
                MessageBox.Show(
                    "unins000.exe not found.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Prepare files before running
                PrepareUninstallFiles(uninstallerPath);

                AppendLog("");
                AppendLog("→ Launching uninstaller...");
                buttonUninstall.Enabled = false;

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = uninstallerPath;
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                psi.WorkingDirectory = Path.GetDirectoryName(uninstallerPath);

                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);

                AppendLog("→ Uninstaller is running. Waiting for it to finish...");
                textBoxInfo.Refresh();

                // Wait for uninstaller to finish
                proc.WaitForExit();

                int exitCode = proc.ExitCode;
                AppendLog("");
                AppendLog($"→ Uninstaller finished (exit code: {exitCode})");
                AppendLog("→ Opening residual folder scanner...");
                textBoxInfo.Refresh();

                // Remove Safe Mode config so next reboot returns to Normal Mode
                RemoveSafeBootConfig();

                // Open Form3 to scan for residual folders
                OpenForm3();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error running uninstaller:\r\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Remove safeboot config so next restart returns to Normal Mode
        private void RemoveSafeBootConfig()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = "bcdedit.exe";
                psi.Arguments = "/deletevalue {current} safeboot";
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
                proc.WaitForExit(5000);
            }
            catch
            {
                // Not critical
            }
        }

        // Prepare files before running uninstaller
        private void PrepareUninstallFiles(string uninstallerPath)
        {
            try
            {
                string dir = Path.GetDirectoryName(uninstallerPath);
                string datFile = Path.Combine(dir, "unins000.dat");

                if (File.Exists(datFile))
                {
                    FileInfo fi = new FileInfo(datFile);
                    // Remove Hidden attribute
                    fi.Attributes &= ~FileAttributes.Hidden;
                    // Remove ReadOnly attribute
                    fi.Attributes &= ~FileAttributes.ReadOnly;
                }

                if (File.Exists(uninstallerPath))
                {
                    FileInfo fi = new FileInfo(uninstallerPath);
                    fi.Attributes &= ~FileAttributes.Hidden;
                    fi.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
            catch
            {
                // Not critical
            }
        }

        // Reboot into Safe Mode
        private void RebootToSafeMode()
        {
            DialogResult result = MessageBox.Show(
                "The system will reboot into Safe Mode to uninstall Avira.\r\n\r\n" +
                "Please save all your work before continuing.\r\n\r\n" +
                "After rebooting, run this tool again to complete the uninstallation.\r\n\r\n" +
                "Reboot now?",
                "Reboot to Safe Mode",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // bcdedit /set {current} safeboot minimal
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                    psi.FileName = "bcdedit.exe";
                    psi.Arguments = "/set {current} safeboot minimal";
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                    System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
                    proc.WaitForExit(5000);

                    if (proc.ExitCode == 0)
                    {
                        // Reboot immediately
                        System.Diagnostics.Process.Start("shutdown.exe", "/r /t 5 /f");
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Failed to configure Safe Mode boot.\r\nPlease reboot to Safe Mode manually.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error: " + ex.Message + "\r\n\r\nPlease reboot to Safe Mode manually.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // ===== UTILITIES =====

        // Write log to textbox
        private void AppendLog(string message)
        {
            if (string.IsNullOrEmpty(textBoxInfo.Text))
                textBoxInfo.Text = message;
            else
                textBoxInfo.Text += "\r\n" + message;
        }

        // Open Form3 - Residual Folder Scanner
        private void OpenForm3()
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ko-fi.com/thanhnguyen150993");
        }
    }
}
