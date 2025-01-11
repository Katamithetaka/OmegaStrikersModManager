using System;
using System.Collections.Generic;
using System.Linq;
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


        public static void PackDirectory(string path)
        {
            string dir = Path.GetFullPath(path);
            PakBuilder pakBuilder = new();
            pakBuilder.Compression([PakCompression.Zlib]);

            using (var file = new FileStream(path + ".pak", FileMode.Create, FileAccess.Write))
            {
                using var writer = pakBuilder.Writer(file, PakVersion.V8B);


                void ReadDir(string path)
                {
                    var directories = Directory.GetDirectories(path);
                    foreach (var directory in directories)
                    {
                        ReadDir(directory);
                    }

                    var files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        var p = file.Replace(dir + "\\", "").Replace("\\", "/");
                        var bytes = File.ReadAllBytes(file);

                        writer.WriteFile(p, bytes);
                        Console.WriteLine(p);
                    }


                }


                ReadDir(path);
                writer.WriteIndex();
            }
        }
    }
}
