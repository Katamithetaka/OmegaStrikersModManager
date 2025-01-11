using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.UnrealTypes;

namespace UAssetCommon
{
    public class Utils
    {
        public static int GetIndexFromFName(FName name)
        {

#pragma warning disable CS8602
#pragma warning disable CS8605
            return (int)typeof(FName).GetField("_index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(name);
#pragma warning restore CS8602
#pragma warning restore CS8605
        }

        public static void SetIndexFromFName(FName name, int index)
        {

#pragma warning disable CS8602
#pragma warning disable CS8605
            typeof(FName).GetField("_index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(name, index);
#pragma warning restore CS8602
#pragma warning restore CS8605
        }

        public static string GetRepakExe()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "./repak.elf";
            }
            else
            {
                return "./repak.exe";
            }
        }

        public static void PackDirectory(string path)
        {
            string dir = Path.GetFullPath(path);
            
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string exe = GetRepakExe();

            ProcessStartInfo processStartInfo = new ProcessStartInfo(exe);
            processStartInfo.UseShellExecute = false;
            processStartInfo.ArgumentList.Add("pack");
            processStartInfo.ArgumentList.Add("--compression");
            processStartInfo.ArgumentList.Add("Zlib");
            processStartInfo.ArgumentList.Add("--version");
            processStartInfo.ArgumentList.Add("V8B");
            Process.Start(processStartInfo);
        }
    }
}
