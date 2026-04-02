# Avira Spotlight Uninstaller Tool

A Windows utility to help users completely uninstall **Avira Spotlight / Avira Security** when the standard uninstaller fails or leaves residual files.

---

## ✨ Features

- **Automatic detection** of Avira installation on both x86 and x64 systems
- **Safe Mode uninstall** — reboots into Safe Mode to run `unins000.exe` cleanly
- **Residual folder scanner** — finds and removes leftover Avira folders after uninstallation
- **Registry cleaner** — scans and removes Avira registry keys, uninstall entries, services and startup entries
- **System Restore point** creation before any cleanup
- **Registry backup** exported to `.reg` files on your Desktop before deletion
- **Force delete** fallback when uninstallers crash or fail
- Runs with **Administrator privileges** automatically

---

## 🖥️ Requirements

| Requirement | Value |
|---|---|
| OS | Windows 7 / 8 / 8.1 / 10 / 11 |
| .NET Framework | 4.6 or later |
| Privileges | Administrator |
| Architecture | x86 / x64 |

---

## 🚀 How to Use

1. **Download** the latest release `.exe`
2. **Run as Administrator** (the tool will prompt for UAC if needed)
3. **Form 1 — Uninstall Process**
   - Detects whether the system is in **Normal Mode** or **Safe Mode**
   - Checks if Avira is installed at the known paths
   - If `unins000.exe` is found → click **Safe Mode** button to configure and reboot
   - After rebooting into Safe Mode, run the tool again → click **Uninstall**
   - After uninstall completes, opens Form 3 automatically
4. **Form 2 — Registry & Folder Cleaner**
   - Scans Avira registry keys, uninstall entries, services and startup entries
   - Scans residual folders in `Program Files`, `ProgramData`, `AppData` and `Temp`
   - Click **🗑️ Clean All** to remove all residuals:
     - Creates a **System Restore point** first
     - Exports **registry backup** `.reg` files to Desktop
     - Deletes registry keys/values via `reg.exe`
     - Force-deletes folders using `takeown` + `icacls`
5. **Form 3 — Residual Folder Scanner**
   - Scans specific known Avira sub-folders (`Fallback Updater`, `Optimizer Host`, `System Speedup`, `VPN`, `Endpoint Protection SDK`, etc.)
   - **🔧 Run Uninstallers** — runs `unins000.exe` for folders that have one
   - **🗑️ Delete No-Uninstaller** — force-deletes folders that have no uninstaller
   - If an uninstaller crashes or fails, offers to force-delete the folder instead

---

## 📁 Project Structure

```
frmaviraunin/
├── Form1.cs / Designer     — Main uninstall process & Safe Mode reboot
├── Form2.cs / Designer     — Registry & folder residual cleaner
├── Form3.cs / Designer     — Residual folder scanner (per-product)
├── Program.cs              — Entry point with UAC admin check
├── app.manifest            — Requires administrator execution level
└── Properties/
    └── AssemblyInfo.cs
```

---

## ⚠️ Disclaimer

This tool modifies the Windows Registry and deletes files/folders. Always ensure you have a **system backup** before use. The tool creates a System Restore point automatically before cleaning.

> This is an independent community tool and is **not affiliated with Avira Operations GmbH & Co. KG**.

---

## ☕ Support the Project

If this tool helped you, consider buying me a coffee!

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/thanhnguyen150993)

> **[https://ko-fi.com/thanhnguyen150993](https://ko-fi.com/thanhnguyen150993)**

Your support helps keep this project maintained and improved. Thank you! 🙏

---

## 📄 License

MIT License — see [LICENSE](LICENSE)
