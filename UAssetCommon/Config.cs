using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI.Unversioned;

namespace UAssetCommon
{
    public class Config
    {
        private string MappingPath = string.Empty;
        private Usmap? Mapping;


        public Config() { }
        public Config(string mappingPath)
        {
            this.Load(mappingPath);
        }


        public void Load(string mappingPath)
        {
            if (MappingPath != mappingPath)
            {
                MappingPath = mappingPath;

                try
                {
                    Mapping = new Usmap(MappingPath);
                }
                catch
                {
                    Logger.Error("Failed to parse mappings: " + MappingPath);
                }
            }
        }

        public bool HasMapping()
        {
            return Mapping != null;
        }

        public Usmap? GetMapping()
        {
            return Mapping;
        }

        public string GetMappingPath()
        {
            return MappingPath;
        }
    }
}
