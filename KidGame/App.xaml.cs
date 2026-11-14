using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Resources;

namespace KidGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += ResolveBundledAssembly;
                string dataRoot = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KidGame");
                string databaseDirectory = Path.Combine(dataRoot, "db");
                string databasePath = Path.Combine(databaseDirectory, "dbGame.accdb");
                Directory.CreateDirectory(databaseDirectory);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataRoot);

                if (!File.Exists(databasePath))
                {
                    byte[] databaseBytes = ReadEmbeddedFile("dbGame.accdb");
                    using (MemoryStream database = new MemoryStream(databaseBytes))
                    {
                        using (FileStream output = File.Create(databasePath))
                            database.CopyTo(output);
                    }
                }

                if (!CanOpenDatabase(databasePath))
                {
                    string installer = ExtractInstaller();
                    Process process = Process.Start(new ProcessStartInfo
                    {
                        FileName = installer,
                        Arguments = "/quiet /norestart",
                        UseShellExecute = true,
                        Verb = "runas"
                    });
                    process.WaitForExit();
                    if (process.ExitCode != 0 && process.ExitCode != 3010)
                        throw new InvalidOperationException("نصب موتور دیتابیس ناموفق بود. کد خطا: " + process.ExitCode);
                    if (!CanOpenDatabase(databasePath))
                        throw new InvalidOperationException("موتور Microsoft Access Database Engine نصب یا در دسترس نیست.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("آماده‌سازی برنامه انجام نشد:\n" + ex.Message, "KidGame",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            base.OnStartup(e);
        }

        private static Assembly ResolveBundledAssembly(object sender, ResolveEventArgs e)
        {
            string name = new AssemblyName(e.Name).Name;
            if (name != "KidGameBiz" && name != "KidGameData")
                return null;
            try
            {
                return Assembly.Load(ReadEmbeddedFile(name + ".dll"));
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private static bool CanOpenDatabase(string databasePath)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + databasePath + ";"))
                    connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                // Only a missing provider should trigger the installer. Other database errors
                // (for example a damaged file) must be reported instead of installing ACE again.
                if (IsProviderMissing(ex))
                    return false;
                throw;
            }
        }

        private static bool IsProviderMissing(Exception ex)
        {
            for (Exception current = ex; current != null; current = current.InnerException)
            {
                if (Marshal.GetHRForException(current) == unchecked((int)0x80040154))
                    return true;
                if (current.Message.IndexOf("provider is not registered", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    current.Message.IndexOf("class not registered", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        private static string ExtractInstaller()
        {
            string installerPath = Path.Combine(Path.GetTempPath(), "KidGame_AccessDatabaseEngine_X64.exe");
            byte[] installerBytes = ReadEmbeddedFile("AccessDatabaseEngine_X64.exe");
            using (MemoryStream installer = new MemoryStream(installerBytes))
            using (FileStream output = File.Create(installerPath))
                installer.CopyTo(output);
            return installerPath;
        }

        private static byte[] ReadEmbeddedFile(string name)
        {
            ResourceManager resources = new ResourceManager("KidGame.Bootstrap", Assembly.GetExecutingAssembly());
            byte[] bytes = resources.GetObject(name) as byte[];
            if (bytes == null)
                throw new FileNotFoundException("فایل داخلی برنامه پیدا نشد: " + name);
            return bytes;
        }
    }
}
