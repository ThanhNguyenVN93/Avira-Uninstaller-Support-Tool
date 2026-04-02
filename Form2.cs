using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace frmaviraunin
{
    public partial class Form2 : Form
    {
        // Known Avira registry key paths to scan (hive, subkey)
        private static readonly string[][] RegistryKeyScans = new string[][]
        {
            new[] { "HKLM", @"SOFTWARE\Avira" },
            new[] { "HKLM", @"SOFTWARE\WOW6432Node\Avira" },
            new[] { "HKCU", @"SOFTWARE\Avira" },
            new[] { "HKLM", @"SOFTWARE\Microsoft\AMSI\Providers\{2781761E-28E0-4109-99FE-B9D127C57AFE}" },
        };

        // Registry paths to search for Avira uninstall entries
        private static readonly string[] UninstallSearchPaths = new string[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };

        // Registry paths to search for Avira services
        private static readonly string ServiceBasePath = @"SYSTEM\CurrentControlSet\Services";

        // Registry paths to search for Avira startup entries
        private static readonly string[][] RunKeyPaths = new string[][]
        {
            new[] { "HKLM", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" },
            new[] { "HKCU", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run" },
            new[] { "HKLM", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" },
        };

        // Residual folder locations
        private static readonly string[] ResidualFolders = new string[]
        {
            @"C:\Program Files\Avira",
            @"C:\Program Files (x86)\Avira",
            @"C:\ProgramData\Avira",
        };

        // Store found items for cleanup
        private List<string> _foundRegistryKeys = new List<string>();
        private List<string> _foundRegistryValues = new List<string>();
        private List<string> _foundFolders = new List<string>();

        public Form2()
        {
            InitializeComponent();
            buttonCleanRegistry.Click += buttonCleanRegistry_Click;
            buttonCleanFolders.Click += buttonCleanFolders_Click;
            buttonCleanAll.Click += buttonCleanAll_Click;
            buttonClose.Click += buttonClose_Click;
            buttonDonate.Click += buttonDonate_Click;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ScanAll();
        }

        // ===== SCAN =====

        private void ScanAll()
        {
            ScanRegistry();
            ScanFolders();
            UpdateButtons();
        }

        private void ScanRegistry()
        {
            listBoxRegistry.Items.Clear();
            _foundRegistryKeys.Clear();
            _foundRegistryValues.Clear();

            // Scan known Avira registry keys
            foreach (string[] entry in RegistryKeyScans)
            {
                string hive = entry[0];
                string subKey = entry[1];
                RegistryKey root = GetHive(hive);

                if (root != null)
                {
                    try
                    {
                        using (RegistryKey key = root.OpenSubKey(subKey))
                        {
                            if (key != null)
                            {
                                string fullPath = hive + @"\" + subKey;
                                _foundRegistryKeys.Add(fullPath);
                                listBoxRegistry.Items.Add("[KEY] " + fullPath);
                            }
                        }
                    }
                    catch { }
                }
            }

            // Scan uninstall entries for Avira
            foreach (string uninstallPath in UninstallSearchPaths)
            {
                try
                {
                    using (RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey(uninstallPath))
                    {
                        if (uninstallKey == null) continue;

                        foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                        {
                            try
                            {
                                using (RegistryKey appKey = uninstallKey.OpenSubKey(subKeyName))
                                {
                                    if (appKey == null) continue;

                                    string displayName = (appKey.GetValue("DisplayName") ?? "").ToString();
                                    string publisher = (appKey.GetValue("Publisher") ?? "").ToString();

                                    if (displayName.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        publisher.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        string fullPath = @"HKLM\" + uninstallPath + @"\" + subKeyName;
                                        _foundRegistryKeys.Add(fullPath);
                                        listBoxRegistry.Items.Add("[UNINSTALL] " + displayName + " → " + fullPath);
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }

            // Scan services for Avira
            try
            {
                using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(ServiceBasePath))
                {
                    if (servicesKey != null)
                    {
                        foreach (string svcName in servicesKey.GetSubKeyNames())
                        {
                            if (svcName.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                string fullPath = @"HKLM\" + ServiceBasePath + @"\" + svcName;
                                _foundRegistryKeys.Add(fullPath);
                                listBoxRegistry.Items.Add("[SERVICE] " + svcName + " → " + fullPath);
                            }
                        }
                    }
                }
            }
            catch { }

            // Scan startup Run keys for Avira values
            foreach (string[] entry in RunKeyPaths)
            {
                string hive = entry[0];
                string subKey = entry[1];
                RegistryKey root = GetHive(hive);

                if (root == null) continue;

                try
                {
                    using (RegistryKey runKey = root.OpenSubKey(subKey))
                    {
                        if (runKey == null) continue;

                        foreach (string valueName in runKey.GetValueNames())
                        {
                            string valueData = (runKey.GetValue(valueName) ?? "").ToString();

                            if (valueName.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                valueData.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                string fullPath = hive + @"\" + subKey + " → " + valueName;
                                _foundRegistryValues.Add(hive + @"\" + subKey + "|" + valueName);
                                listBoxRegistry.Items.Add("[STARTUP] " + valueName + " → " + fullPath);
                            }
                        }
                    }
                }
                catch { }
            }

            if (listBoxRegistry.Items.Count == 0)
            {
                listBoxRegistry.Items.Add("✓ No Avira registry entries found.");
            }
        }

        private void ScanFolders()
        {
            listBoxFolders.Items.Clear();
            _foundFolders.Clear();

            // Check fixed paths
            foreach (string folder in ResidualFolders)
            {
                ScanOneFolder(folder);
            }

            // Check user-specific paths
            string appDataRoaming = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Avira");
            ScanOneFolder(appDataRoaming);

            string appDataLocal = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Avira");
            ScanOneFolder(appDataLocal);

            // Check Windows Temp for Avira files
            string winTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
            ScanTempFolder(winTemp);

            string userTemp = Path.GetTempPath();
            if (!userTemp.Equals(winTemp, StringComparison.OrdinalIgnoreCase))
            {
                ScanTempFolder(userTemp);
            }

            if (listBoxFolders.Items.Count == 0)
            {
                listBoxFolders.Items.Add("✓ No residual Avira folders found.");
            }
        }

        // Scan a single Avira folder and list all files inside (including hidden)
        private void ScanOneFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;

            _foundFolders.Add(folderPath);

            // Count files and size
            long totalSize = 0;
            int fileCount = 0;
            List<string> fileEntries = new List<string>();

            try
            {
                foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        fileCount++;
                        totalSize += fi.Length;

                        string relativePath = file.Substring(folderPath.Length + 1);
                        string attrs = "";
                        if ((fi.Attributes & FileAttributes.Hidden) != 0) attrs += " [Hidden]";
                        if ((fi.Attributes & FileAttributes.System) != 0) attrs += " [System]";
                        if ((fi.Attributes & FileAttributes.ReadOnly) != 0) attrs += " [ReadOnly]";

                        fileEntries.Add("   ├ " + relativePath + " (" + FormatSize(fi.Length) + ")" + attrs);
                    }
                    catch { }
                }
            }
            catch { }

            // Add folder header
            listBoxFolders.Items.Add("📁 " + folderPath + " (" + fileCount + " files, " + FormatSize(totalSize) + ")");

            // Add file list (limit to 50 to avoid flooding)
            int shown = 0;
            foreach (string entry in fileEntries)
            {
                if (shown >= 50)
                {
                    listBoxFolders.Items.Add("   └ ... and " + (fileEntries.Count - 50) + " more file(s)");
                    break;
                }
                listBoxFolders.Items.Add(entry);
                shown++;
            }
        }

        // Scan temp folder for individual Avira files
        private void ScanTempFolder(string tempPath)
        {
            if (!Directory.Exists(tempPath))
                return;

            try
            {
                // Look for Avira-named subfolders in temp
                foreach (string dir in Directory.GetDirectories(tempPath, "*Avira*", SearchOption.TopDirectoryOnly))
                {
                    if (!_foundFolders.Contains(dir))
                    {
                        _foundFolders.Add(dir);
                        try
                        {
                            int fileCount = Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length;
                            listBoxFolders.Items.Add("📁 " + dir + " (" + fileCount + " files) [TEMP]");
                        }
                        catch
                        {
                            listBoxFolders.Items.Add("📁 " + dir + " [TEMP]");
                        }
                    }
                }
            }
            catch { }
        }

        private void UpdateButtons()
        {
            bool hasRegistry = _foundRegistryKeys.Count > 0 || _foundRegistryValues.Count > 0;
            bool hasFolders = _foundFolders.Count > 0;
            buttonCleanRegistry.Enabled = hasRegistry || hasFolders;
            buttonCleanFolders.Enabled = hasFolders;
            buttonCleanAll.Enabled = hasRegistry || hasFolders;
        }

        // ===== CLEAN ACTIONS =====

        // Yellow button: Create restore point → Backup registry → Clean registry → Auto clean folders
        private void buttonCleanRegistry_Click(object sender, EventArgs e)
        {
            int totalReg = _foundRegistryKeys.Count + _foundRegistryValues.Count;
            int totalFolders = _foundFolders.Count;

            if (totalReg == 0 && totalFolders == 0)
            {
                MessageBox.Show("Nothing to clean.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "This will clean all Avira residuals:\r\n\r\n" +
                $"• {_foundRegistryKeys.Count} registry key(s)\r\n" +
                $"• {_foundRegistryValues.Count} registry value(s)\r\n" +
                $"• {_foundFolders.Count} residual folder(s)\r\n\r\n" +
                "A System Restore point and registry backup\r\n" +
                "will be created first.\r\n\r\n" +
                "Continue?",
                "Confirm Full Cleanup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            buttonCleanRegistry.Enabled = false;
            buttonCleanFolders.Enabled = false;
            buttonCleanAll.Enabled = false;

            // Step 1: Create System Restore point
            groupBoxRegistry.Text = "📋 Registry Entries — Creating restore point...";
            groupBoxRegistry.Refresh();

            bool restoreCreated = CreateSystemRestorePoint();

            // Step 2: Backup registry keys to .reg files
            groupBoxRegistry.Text = "📋 Registry Entries — Backing up registry...";
            groupBoxRegistry.Refresh();

            string backupPath = BackupRegistryKeys();

            if (restoreCreated || !string.IsNullOrEmpty(backupPath))
            {
                string msg = "";
                if (restoreCreated)
                    msg += "✓ System Restore point created\r\n";
                else
                    msg += "✗ Could not create Restore point (not critical)\r\n";

                if (!string.IsNullOrEmpty(backupPath))
                    msg += "✓ Registry backup saved to:\r\n   " + backupPath;

                MessageBox.Show(msg, "Backup Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Step 3: Clean registry
            groupBoxRegistry.Text = "📋 Registry Entries — Cleaning registry...";
            groupBoxRegistry.Refresh();

            int regCleaned = 0;
            if (totalReg > 0)
            {
                regCleaned = CleanRegistryEntries();
            }

            // Step 4: Auto clean folders
            groupBoxFolders.Text = "📁 Residual Folders — Cleaning folders...";
            groupBoxFolders.Refresh();

            int folderCleaned = 0;
            if (totalFolders > 0)
            {
                folderCleaned = CleanFolderEntries();
            }

            // Done
            MessageBox.Show(
                "Cleanup complete!\r\n\r\n" +
                $"• Registry: {regCleaned} item(s) cleaned\r\n" +
                $"• Folders: {folderCleaned} of {totalFolders} folder(s) cleaned\r\n\r\n" +
                "Avira has been completely removed from this system.",
                "Full Cleanup Done",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Application.Exit();
        }

        // Green button: Clean folders only
        private void buttonCleanFolders_Click(object sender, EventArgs e)
        {
            if (_foundFolders.Count == 0)
            {
                MessageBox.Show("No residual Avira folders to clean.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Delete {_foundFolders.Count} residual folder(s)?\r\n\r\nThis cannot be undone.",
                "Confirm Folder Cleanup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            int cleaned = CleanFolderEntries();

            MessageBox.Show(
                $"Cleaned {cleaned} of {_foundFolders.Count} folder(s).\r\n\r\n" +
                "Avira has been completely removed from this system.",
                "Folder Cleanup Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.Exit();
        }

        // Red button: Same as yellow - full cleanup with backup
        private void buttonCleanAll_Click(object sender, EventArgs e)
        {
            buttonCleanRegistry_Click(sender, e);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ko-fi.com/thanhnguyen150993");
        }

        // ===== BACKUP =====

        // Create a System Restore point using WMI
        private bool CreateSystemRestorePoint()
        {
            try
            {
                ManagementScope scope = new ManagementScope(@"\\localhost\root\default");
                ManagementPath path = new ManagementPath("SystemRestore");
                ObjectGetOptions options = new ObjectGetOptions();

                using (ManagementClass mgmtClass = new ManagementClass(scope, path, options))
                {
                    ManagementBaseObject inParams = mgmtClass.GetMethodParameters("CreateRestorePoint");
                    inParams["Description"] = "Before Avira Cleanup - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    inParams["RestorePointType"] = 12; // MODIFY_SETTINGS
                    inParams["EventType"] = 100; // BEGIN_SYSTEM_CHANGE

                    ManagementBaseObject outParams = mgmtClass.InvokeMethod("CreateRestorePoint", inParams, null);
                    int retVal = Convert.ToInt32(outParams["ReturnValue"]);
                    return retVal == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Backup found registry keys to .reg files on Desktop
        private string BackupRegistryKeys()
        {
            if (_foundRegistryKeys.Count == 0)
                return null;

            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string backupDir = Path.Combine(desktop, "AviraBackup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                Directory.CreateDirectory(backupDir);

                int index = 0;
                foreach (string fullPath in _foundRegistryKeys)
                {
                    try
                    {
                        index++;
                        string safeName = fullPath.Replace(@"\", "_").Replace(":", "").Replace("{", "").Replace("}", "");
                        if (safeName.Length > 80) safeName = safeName.Substring(0, 80);
                        string regFile = Path.Combine(backupDir, index + "_" + safeName + ".reg");

                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "reg.exe";
                        psi.Arguments = "export \"" + fullPath + "\" \"" + regFile + "\" /y";
                        psi.Verb = "runas";
                        psi.UseShellExecute = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        Process proc = Process.Start(psi);
                        proc.WaitForExit(10000);
                    }
                    catch { }
                }

                return backupDir;
            }
            catch
            {
                return null;
            }
        }

        // ===== CLEANUP LOGIC =====

        private int CleanRegistryEntries()
        {
            int cleaned = 0;

            // Delete registry keys
            foreach (string fullPath in _foundRegistryKeys)
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "reg.exe";
                    psi.Arguments = "delete \"" + fullPath + "\" /f";
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    Process proc = Process.Start(psi);
                    proc.WaitForExit(5000);

                    if (proc.ExitCode == 0)
                        cleaned++;
                }
                catch { }
            }

            // Delete registry values
            foreach (string entry in _foundRegistryValues)
            {
                try
                {
                    string[] parts = entry.Split('|');
                    if (parts.Length != 2) continue;

                    string keyPath = parts[0];
                    string valueName = parts[1];

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "reg.exe";
                    psi.Arguments = "delete \"" + keyPath + "\" /v \"" + valueName + "\" /f";
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    Process proc = Process.Start(psi);
                    proc.WaitForExit(5000);

                    if (proc.ExitCode == 0)
                        cleaned++;
                }
                catch { }
            }

            return cleaned;
        }

        private int CleanFolderEntries()
        {
            int cleaned = 0;

            foreach (string folderPath in _foundFolders)
            {
                try
                {
                    // Take ownership first
                    RunHidden("takeown", "/f \"" + folderPath + "\" /r /d y");
                    RunHidden("icacls", "\"" + folderPath + "\" /grant *S-1-5-32-544:F /t /q");

                    // Remove attributes on all files (including hidden/system)
                    try
                    {
                        foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                        {
                            try { File.SetAttributes(file, FileAttributes.Normal); } catch { }
                        }
                    }
                    catch { }

                    // Try .NET delete
                    try
                    {
                        Directory.Delete(folderPath, true);
                        cleaned++;
                        continue;
                    }
                    catch { }

                    // Fallback: cmd rd
                    RunHidden("cmd.exe", "/c rd /s /q \"" + folderPath + "\"");
                    if (!Directory.Exists(folderPath))
                    {
                        cleaned++;
                    }
                }
                catch { }
            }

            return cleaned;
        }

        // ===== UTILITIES =====

        private RegistryKey GetHive(string hiveName)
        {
            switch (hiveName.ToUpperInvariant())
            {
                case "HKLM": return Registry.LocalMachine;
                case "HKCU": return Registry.CurrentUser;
                case "HKCR": return Registry.ClassesRoot;
                default: return null;
            }
        }

        private void RunHidden(string fileName, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = fileName;
                psi.Arguments = arguments;
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(psi);
                proc.WaitForExit(15000);
            }
            catch { }
        }

        private string FormatSize(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("0.#") + " KB";
            if (bytes < 1024 * 1024 * 1024) return (bytes / (1024.0 * 1024)).ToString("0.#") + " MB";
            return (bytes / (1024.0 * 1024 * 1024)).ToString("0.##") + " GB";
        }
    }
}
