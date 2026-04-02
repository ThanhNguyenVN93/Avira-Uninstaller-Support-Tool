using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace frmaviraunin
{
    public partial class Form3 : Form
    {
        // Known Avira sub-folders to scan
        private static readonly string[] AviraSubFolders = new string[]
        {
            "Fallback Updater",
            "Optimizer Host",
            "System Speedup",
            "VPN",
            "Security"
        };

        // x64: also check Program Files for 64-bit components
        private static readonly string[] AviraSubFolders64 = new string[]
        {
            "Endpoint Protection SDK"
        };

        // P/Invoke for scheduling file deletion on reboot
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, int dwFlags);
        private const int MOVEFILE_DELAY_UNTIL_REBOOT = 0x4;

        // Tracks whether a reboot is needed to finish deletion
        private bool _rebootRequired = false;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ScanResidualFolders();
            CheckTransitionToForm2();
        }

        // ===== SCAN =====

        private void ScanResidualFolders()
        {
            listViewFolders.Items.Clear();
            int foundCount = 0;
            int uninstallerCount = 0;

            // Scan C:\Program Files (x86)\Avira\
            string basePath86 = @"C:\Program Files (x86)\Avira";
            if (Directory.Exists(basePath86))
            {
                foreach (string subFolder in AviraSubFolders)
                {
                    string fullPath = Path.Combine(basePath86, subFolder);
                    if (Directory.Exists(fullPath))
                    {
                        bool hasUninstaller = HasUninstaller(fullPath);
                        AddFolderItem(subFolder, fullPath, hasUninstaller);
                        foundCount++;
                        if (hasUninstaller) uninstallerCount++;
                    }
                }
            }

            // Scan C:\Program Files\Avira\
            string basePath64 = @"C:\Program Files\Avira";
            if (Directory.Exists(basePath64))
            {
                foreach (string subFolder in AviraSubFolders64)
                {
                    string fullPath = Path.Combine(basePath64, subFolder);
                    if (Directory.Exists(fullPath))
                    {
                        bool hasUninstaller = HasUninstaller(fullPath);
                        AddFolderItem(subFolder, fullPath, hasUninstaller);
                        foundCount++;
                        if (hasUninstaller) uninstallerCount++;
                    }
                }
            }

            // Update status and buttons
            if (foundCount == 0)
            {
                labelStatus.ForeColor = Color.FromArgb(40, 167, 69);
                labelStatus.Text = "✓ No residual Avira folders found. System is clean.";
                buttonUninstallSelected.Enabled = false;
                buttonDeleteSelected.Enabled = false;
            }
            else
            {
                int noUninstallerCount = foundCount - uninstallerCount;
                labelStatus.ForeColor = Color.FromArgb(220, 53, 69);
                labelStatus.Text = $"✗ Found {foundCount} residual folder(s): {uninstallerCount} with uninstaller, {noUninstallerCount} without";
                buttonUninstallSelected.Enabled = uninstallerCount > 0;
                buttonDeleteSelected.Enabled = noUninstallerCount > 0;
            }
        }

        // Check if folder contains unins000.exe
        private bool HasUninstaller(string folderPath)
        {
            return File.Exists(Path.Combine(folderPath, "unins000.exe"));
        }

        // Add folder item to ListView
        private void AddFolderItem(string name, string path, bool hasUninstaller)
        {
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add(path);
            item.SubItems.Add(hasUninstaller ? "✓ unins000.exe" : "✗ Not found");
            item.Tag = hasUninstaller;
            item.Checked = true;

            if (hasUninstaller)
            {
                item.ForeColor = Color.FromArgb(0, 123, 255);
            }
            else
            {
                item.ForeColor = Color.FromArgb(220, 53, 69);
            }

            listViewFolders.Items.Add(item);
        }

        // ===== ACTIONS =====

        // Run uninstallers for checked items that have unins000.exe
        private void buttonUninstallSelected_Click(object sender, EventArgs e)
        {
            List<ListViewItem> toUninstall = new List<ListViewItem>();
            foreach (ListViewItem item in listViewFolders.CheckedItems)
            {
                if (item.Tag != null && (bool)item.Tag == true)
                {
                    toUninstall.Add(item);
                }
            }

            if (toUninstall.Count == 0)
            {
                MessageBox.Show("No checked folders with uninstallers found.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Run uninstaller for {toUninstall.Count} folder(s)?\r\n\r\nEach uninstaller will run one at a time.",
                "Confirm Uninstall",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            buttonUninstallSelected.Enabled = false;
            buttonDeleteSelected.Enabled = false;

            foreach (ListViewItem item in toUninstall)
            {
                string folderPath = item.SubItems[1].Text;
                string uninstallerPath = Path.Combine(folderPath, "unins000.exe");
                bool uninstallSuccess = false;

                try
                {
                    // Prepare files
                    PrepareFile(uninstallerPath);
                    PrepareFile(Path.Combine(folderPath, "unins000.dat"));

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = uninstallerPath;
                    psi.Arguments = "/VERYSILENT /NORESTART";
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.WorkingDirectory = folderPath;

                    labelStatus.Text = $"Running uninstaller: {item.Text}...";
                    labelStatus.ForeColor = Color.FromArgb(0, 123, 255);
                    labelStatus.Refresh();

                    Process proc = Process.Start(psi);

                    // Timeout after 120 seconds to avoid freezing
                    if (!proc.WaitForExit(120000))
                    {
                        try { proc.Kill(); } catch { }
                        OfferForceDelete(item.Text, folderPath);
                        continue;
                    }

                    // Check if folder was removed by uninstaller
                    uninstallSuccess = !Directory.Exists(folderPath) || proc.ExitCode == 0;

                    if (!uninstallSuccess)
                    {
                        // Uninstaller returned error - offer to force delete
                        OfferForceDelete(item.Text, folderPath);
                    }
                }
                catch (Exception ex)
                {
                    // Uninstaller crashed or failed to start
                    DialogResult forceResult = MessageBox.Show(
                        $"Uninstaller for \"{item.Text}\" failed:\r\n{ex.Message}\r\n\r\n" +
                        "Do you want to force delete this folder instead?",
                        "Uninstaller Failed",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (forceResult == DialogResult.Yes)
                    {
                        ForceDeleteFolder(folderPath);
                    }
                }
            }

            // Rescan
            ScanResidualFolders();
            CheckTransitionToForm2();
        }

        // Offer to force delete a folder when uninstaller fails
        private void OfferForceDelete(string folderName, string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;

            DialogResult result = MessageBox.Show(
                $"Uninstaller for \"{folderName}\" failed or returned an error.\r\n\r\n" +
                "This can happen when Avira's uninstaller is damaged.\r\n" +
                "Do you want to force delete this folder instead?",
                "Uninstaller Failed",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ForceDeleteFolder(folderPath);
            }
        }

        // Force delete a folder and all its contents (10-step escalation)
        private void ForceDeleteFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;

            try
            {
                // Step 1: Kill processes running from this folder
                UpdateStatus("Step 1: Killing processes in folder...");
                KillProcessesInFolder(folderPath);

                // Step 2: Unregister AMSI provider from registry
                UpdateStatus("Step 2: Unregistering AMSI provider...");
                UnregisterAmsiProvider(folderPath);

                // Step 3: Unregister DLLs with regsvr32 /u
                UpdateStatus("Step 3: Unregistering DLLs (regsvr32 /u)...");
                UnregisterDlls(folderPath);

                // Step 4: Take ownership and grant full permissions
                UpdateStatus("Step 4: Taking ownership (takeown + icacls)...");
                TakeOwnership(folderPath);

                // Step 5: Remove read-only/hidden attributes
                UpdateStatus("Step 5: Removing file protections...");
                foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    try { File.SetAttributes(file, FileAttributes.Normal); } catch { }
                }

                // Step 6: Try .NET delete
                UpdateStatus("Step 6: Attempting .NET delete...");
                try
                {
                    Directory.Delete(folderPath, true);
                    UpdateStatus("✓ Folder deleted successfully.");
                    return;
                }
                catch { }

                // Step 7: Try cmd /c rd /s /q
                UpdateStatus("Step 7: Force deleting with CMD...");
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "cmd.exe";
                    psi.Arguments = "/c rd /s /q \"" + folderPath + "\"";
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    Process proc = Process.Start(psi);
                    proc.WaitForExit(15000);

                    if (!Directory.Exists(folderPath))
                    {
                        UpdateStatus("✓ Folder deleted successfully.");
                        return;
                    }
                }
                catch { }

                // Step 8: Kill explorer.exe, try delete, restart explorer
                UpdateStatus("Step 8: Stopping Explorer and retrying...");
                if (TryDeleteWithExplorerRestart(folderPath))
                {
                    UpdateStatus("✓ Folder deleted successfully.");
                    return;
                }

                // Step 9: Rename locked files to .bak so Windows can't load them
                UpdateStatus("Step 9: Renaming locked files to .bak...");
                RenameLockedFiles(folderPath);

                // Step 10: Schedule all remaining files for deletion on reboot
                UpdateStatus("Step 10: Scheduling deletion on next reboot...");
                ScheduleDeleteOnReboot(folderPath);
                _rebootRequired = true;

                MessageBox.Show(
                    "Files are locked by Windows (e.g. AMSI provider DLL).\r\n\r\n" +
                    "Locked files have been renamed to .bak and scheduled\r\n" +
                    "for deletion on next reboot.\r\n\r\n" +
                    "Please restart your computer to complete the cleanup.",
                    "Reboot Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not delete folder:\r\n{ex.Message}\r\n\r\n" +
                    "Some files may be in use. Try again after restarting.",
                    "Delete Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Kill any processes running from the specified folder
        private void KillProcessesInFolder(string folderPath)
        {
            try
            {
                foreach (Process proc in Process.GetProcesses())
                {
                    try
                    {
                        string processPath = proc.MainModule.FileName;
                        if (processPath != null && processPath.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                        {
                            proc.Kill();
                            proc.WaitForExit(5000);
                        }
                    }
                    catch
                    {
                        // Access denied or process already exited - skip
                    }
                }
            }
            catch
            {
                // Not critical
            }
        }

        // Unregister Avira AMSI provider from Windows registry
        // avamsi.dll is loaded by Windows as an AMSI provider, keeping it locked
        private void UnregisterAmsiProvider(string folderPath)
        {
            // Known Avira AMSI provider GUIDs
            string[] knownAviraGuids = new string[]
            {
                "{2781761E-28E0-4109-99FE-B9D127C57AFE}"
            };

            // Step A: Force-delete known Avira GUIDs directly using reg.exe
            foreach (string guid in knownAviraGuids)
            {
                RegDeleteTree(@"HKLM\SOFTWARE\Microsoft\AMSI\Providers\" + guid);
                RegDeleteTree(@"HKLM\SOFTWARE\Classes\CLSID\" + guid);
                RegDeleteTree(@"HKLM\SOFTWARE\WOW6432Node\Microsoft\AMSI\Providers\" + guid);
                RegDeleteTree(@"HKLM\SOFTWARE\WOW6432Node\Classes\CLSID\" + guid);
            }

            // Step B: Search all AMSI providers and match by folder path or "Avira" name
            string[] amsiRegPaths = new string[]
            {
                @"SOFTWARE\Microsoft\AMSI\Providers",
                @"SOFTWARE\WOW6432Node\Microsoft\AMSI\Providers"
            };

            foreach (string amsiRegPath in amsiRegPaths)
            {
                try
                {
                    using (RegistryKey amsiKey = Registry.LocalMachine.OpenSubKey(amsiRegPath, true))
                    {
                        if (amsiKey == null) continue;

                        foreach (string subKeyName in amsiKey.GetSubKeyNames())
                        {
                            bool shouldDelete = false;

                            // Check CLSID InprocServer32 for Avira path
                            string[] clsidSearchPaths = new string[]
                            {
                                @"SOFTWARE\Classes\CLSID\" + subKeyName + @"\InprocServer32",
                                @"SOFTWARE\WOW6432Node\Classes\CLSID\" + subKeyName + @"\InprocServer32"
                            };

                            foreach (string clsidPath in clsidSearchPaths)
                            {
                                try
                                {
                                    using (RegistryKey clsidKey = Registry.LocalMachine.OpenSubKey(clsidPath))
                                    {
                                        if (clsidKey != null)
                                        {
                                            string dllPath = (clsidKey.GetValue(null) ?? "").ToString();
                                            if (dllPath.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                dllPath.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                                            {
                                                shouldDelete = true;
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }

                            // Also check CLSID default value (description) for "Avira"
                            if (!shouldDelete)
                            {
                                try
                                {
                                    using (RegistryKey clsidKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\CLSID\" + subKeyName))
                                    {
                                        if (clsidKey != null)
                                        {
                                            string desc = (clsidKey.GetValue(null) ?? "").ToString();
                                            if (desc.IndexOf("Avira", StringComparison.OrdinalIgnoreCase) >= 0)
                                            {
                                                shouldDelete = true;
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }

                            if (shouldDelete)
                            {
                                try { amsiKey.DeleteSubKeyTree(subKeyName, false); } catch { }
                                RegDeleteTree(@"HKLM\SOFTWARE\Classes\CLSID\" + subKeyName);
                                RegDeleteTree(@"HKLM\SOFTWARE\WOW6432Node\Classes\CLSID\" + subKeyName);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        // Use reg.exe to force delete a registry key tree
        private void RegDeleteTree(string fullKeyPath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "reg.exe";
                psi.Arguments = "delete \"" + fullKeyPath + "\" /f";
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(psi);
                proc.WaitForExit(5000);
            }
            catch { }
        }

        // Take ownership and grant full permissions using system tools
        private void TakeOwnership(string folderPath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "takeown";
                psi.Arguments = "/f \"" + folderPath + "\" /r /d y";
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(psi);
                proc.WaitForExit(15000);
            }
            catch { }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "icacls";
                psi.Arguments = "\"" + folderPath + "\" /grant *S-1-5-32-544:F /t /q";
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(psi);
                proc.WaitForExit(15000);
            }
            catch { }
        }

        // Schedule folder and all contents for deletion on next reboot
        private void ScheduleDeleteOnReboot(string folderPath)
        {
            // Schedule all files first
            foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                MoveFileEx(file, null, MOVEFILE_DELAY_UNTIL_REBOOT);
            }

            // Schedule directories deepest-first
            string[] dirs = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories)
                .OrderByDescending(d => d.Length)
                .ToArray();
            foreach (string dir in dirs)
            {
                MoveFileEx(dir, null, MOVEFILE_DELAY_UNTIL_REBOOT);
            }

            // Schedule the root folder itself
            MoveFileEx(folderPath, null, MOVEFILE_DELAY_UNTIL_REBOOT);
        }

        // Unregister all DLLs in folder using regsvr32 /u /s
        private void UnregisterDlls(string folderPath)
        {
            try
            {
                foreach (string dll in Directory.GetFiles(folderPath, "*.dll", SearchOption.AllDirectories))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "regsvr32";
                        psi.Arguments = "/u /s \"" + dll + "\"";
                        psi.Verb = "runas";
                        psi.UseShellExecute = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        Process proc = Process.Start(psi);
                        proc.WaitForExit(10000);
                    }
                    catch { }
                }
            }
            catch { }
        }

        // Kill explorer.exe, attempt delete, then restart explorer
        private bool TryDeleteWithExplorerRestart(string folderPath)
        {
            try
            {
                // Kill all explorer.exe instances
                foreach (Process proc in Process.GetProcessesByName("explorer"))
                {
                    try { proc.Kill(); proc.WaitForExit(5000); } catch { }
                }

                // Wait for file handles to be released
                System.Threading.Thread.Sleep(2000);

                // Attempt delete while explorer is down
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "cmd.exe";
                        psi.Arguments = "/c rd /s /q \"" + folderPath + "\"";
                        psi.UseShellExecute = false;
                        psi.CreateNoWindow = true;
                        Process proc = Process.Start(psi);
                        proc.WaitForExit(10000);
                    }
                    catch { }
                }

                return !Directory.Exists(folderPath);
            }
            catch
            {
                return false;
            }
            finally
            {
                // Always restart explorer
                try { Process.Start("explorer.exe"); } catch { }
            }
        }

        // Rename locked files to .bak so Windows cannot load them on next boot
        private void RenameLockedFiles(string folderPath)
        {
            try
            {
                foreach (string file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    if (file.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                        continue;

                    try
                    {
                        string bakPath = file + ".bak";
                        File.Move(file, bakPath);
                    }
                    catch
                    {
                        // If direct rename fails, schedule rename on reboot
                        try
                        {
                            string bakPath = file + ".bak";
                            MoveFileEx(file, bakPath, MOVEFILE_DELAY_UNTIL_REBOOT);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        // Update status label with current operation
        private void UpdateStatus(string message)
        {
            labelStatus.Text = message;
            labelStatus.ForeColor = Color.FromArgb(0, 123, 255);
            labelStatus.Refresh();
        }

        // Delete checked folders that have NO uninstaller
        private void buttonDeleteSelected_Click(object sender, EventArgs e)
        {
            List<ListViewItem> toDelete = new List<ListViewItem>();
            foreach (ListViewItem item in listViewFolders.CheckedItems)
            {
                if (item.Tag != null && (bool)item.Tag == false)
                {
                    toDelete.Add(item);
                }
            }

            if (toDelete.Count == 0)
            {
                MessageBox.Show("No checked folders without uninstallers found.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Permanently delete {toDelete.Count} folder(s)?\r\n\r\nThis cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            _rebootRequired = false;
            int deleted = 0;
            int scheduled = 0;
            foreach (ListViewItem item in toDelete)
            {
                string folderPath = item.SubItems[1].Text;
                ForceDeleteFolder(folderPath);
                if (!Directory.Exists(folderPath))
                {
                    deleted++;
                }
                else if (_rebootRequired)
                {
                    scheduled++;
                }
            }

            if (scheduled > 0)
            {
                MessageBox.Show(
                    $"Deleted {deleted} of {toDelete.Count} folder(s).\r\n" +
                    $"{scheduled} folder(s) scheduled for deletion on reboot.",
                    "Done - Reboot Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Deleted {deleted} of {toDelete.Count} folder(s).",
                    "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Rescan
            ScanResidualFolders();
            CheckTransitionToForm2();
        }

        // Close
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            Process.Start("https://ko-fi.com/thanhnguyen150993");
        }

        // Check if all folders are clean and transition to Form2
        private void CheckTransitionToForm2()
        {
            if (listViewFolders.Items.Count == 0)
            {
                DialogResult result = MessageBox.Show(
                    "All Avira folders have been cleaned!\r\n\r\n" +
                    "Do you want to scan for residual registry entries\r\n" +
                    "and hidden files?",
                    "Cleanup Complete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Form2 form2 = new Form2();
                    form2.ShowDialog();
                }

                this.Close();
            }
        }

        // ===== UTILITIES =====

        // Remove Hidden and ReadOnly attributes from a file
        private void PrepareFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fi = new FileInfo(filePath);
                    fi.Attributes &= ~FileAttributes.Hidden;
                    fi.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
            catch
            {
                // Not critical
            }
        }
    }
}
