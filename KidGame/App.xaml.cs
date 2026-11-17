using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Resources;

namespace KidGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string startupLogPath;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                startupLogPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "KidGame", "startup.log");
                Directory.CreateDirectory(Path.GetDirectoryName(startupLogPath));
                File.WriteAllText(startupLogPath, "KidGame startup: " + DateTime.Now.ToString("O") + Environment.NewLine);
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                    Log("Unhandled exception: " + args.ExceptionObject);
                AppDomain.CurrentDomain.AssemblyResolve += ResolveBundledAssembly;
                Log("Assembly resolver registered.");
                string dataRoot = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KidGame");
                string databaseDirectory = Path.Combine(dataRoot, "db");
                string databasePath = Path.Combine(databaseDirectory, "dbGame.accdb");
                Directory.CreateDirectory(databaseDirectory);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataRoot);
                Log("Data directory: " + dataRoot);

                if (!File.Exists(databasePath))
                {
                    byte[] databaseBytes = ReadEmbeddedFile("dbGame.accdb");
                    using (MemoryStream database = new MemoryStream(databaseBytes))
                    {
                        using (FileStream output = File.Create(databasePath))
                            database.CopyTo(output);
                    }
                    Log("Database extracted.");
                }
                else
                    Log("Database already exists.");

                if (!IsAceProviderRegistered())
                {
                    Log("ACE provider is not registered; starting installer.");
                    string installer = ExtractInstaller();
                    Process process = Process.Start(new ProcessStartInfo
                    {
                        FileName = installer,
                        Arguments = "/quiet /norestart",
                        UseShellExecute = true,
                        Verb = "runas"
                    });
                    process.WaitForExit();
                    Log("Installer finished with exit code " + process.ExitCode + ".");
                    if (process.ExitCode != 0 && process.ExitCode != 3010)
                        throw new InvalidOperationException("نصب موتور دیتابیس ناموفق بود. کد خطا: " + process.ExitCode);
                    if (!IsAceProviderRegistered())
                        throw new InvalidOperationException("موتور Microsoft Access Database Engine نصب یا در دسترس نیست.");
                }
                else
                    Log("ACE provider is registered.");
            }
            catch (Exception ex)
            {
                Log("Startup failed: " + ex);
                MessageBox.Show("آماده‌سازی برنامه انجام نشد:\n" + ex.Message, "KidGame",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            Log("Starting WPF application.");
            base.OnStartup(e);
        }

        private static Assembly ResolveBundledAssembly(object sender, ResolveEventArgs e)
        {
            string name = new AssemblyName(e.Name).Name;
            if (name != "KidGameBiz" && name != "KidGameData")
                return null;
            try
            {
                Assembly result = Assembly.Load(ReadEmbeddedFile(name + ".dll"));
                Log("Loaded bundled assembly " + name + ".dll.");
                return result;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        internal static void Log(string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(startupLogPath))
                    File.AppendAllText(startupLogPath, DateTime.Now.ToString("O") + " " + message + Environment.NewLine);
            }
            catch { }
        }

        private static bool IsAceProviderRegistered()
        {
            return Type.GetTypeFromProgID("Microsoft.ACE.OLEDB.12.0", false) != null;
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
