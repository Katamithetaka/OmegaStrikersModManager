using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml.Linq;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UAssetCommon
{
    public class Asset
    {
        public UAsset Data = new UAsset(EngineVersion.VER_UE5_1);
        public NormalExport Export;
        public List<Property> Properties = new List<Property>();
        public List<Property> RootProperties = new List<Property>();
        public List<Property> UnknownProperties = new List<Property>();
        public NameRefList NameReferences;

        public Asset(Config config, string assetPath)
        {
            if (config.HasMapping())
            {
                Data.Mappings = config.GetMapping();
            }

            var strmRaw = Data.PathToStream(assetPath);
            Data.Read(new AssetBinaryReader(strmRaw, Data));
            Logger.Trace(assetPath);
            Logger.Trace(Data.PackageGuid);
             
            try
            {
                Export = (NormalExport)Data.Exports[0];
            }
            catch
            {
                Logger.Error("Mapping unsupported!");
            }

            if(Export != null)
            {
                LoadProperties();
            }

            NameReferences = new NameRefList(this);
            Logger.Trace(this);

            var s = "";
            foreach(var import in Data.Imports)
            {
                s += import.OuterIndex + " " + import.ObjectName + "\n";
            }
            Logger.Trace(s);

        }

        public void ReloadNamePath()
        {
            this.NameReferences = new NameRefList(this);
        }

        // Takes a path split with .
        public Property? GetPropertyByPath(string path)
        {
            var parts = path.Split('.');

            return GetPropertyFromNames(parts);
        }

        public Property? GetPropertyFromNames(string[] names)
        {
            Property? p = null;
            var name = names[0];
            foreach (var property in RootProperties)
            {
                if (property.Name == name) p = property;
            }

            foreach (var part in names.Skip(1))
            {
                if (p == null) return null;
                p = p.GetChildByName(part);
            }

            return null;
        }

        public Property? GetPropertyByName(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name == name) return property;
            }

            return null;
        }


        private void LoadProperties()
        {
            foreach (PropertyData dat in Export.Data)
            {
                Property property = Property.MakeProperty(this, dat);
                RootProperties.Add(property);
                LoadPropertyChildren(property);
            }
        }

        private void LoadPropertyChildren(Property property)
        {
            Properties.Add(property);

            if (property.Data is ArrayPropertyData arrDat)
            {
                property.Type = typeof(ArrayPropertyData);
                for (int i = 0; i < arrDat.Value.Length; i++)
                {
                    Property child = Property.MakeProperty(this, arrDat.Value[i]);
                    property.Children.Add(child);
                    LoadPropertyChildren(property.Children[i]);
                }
            }
            else if (property.Data is StructPropertyData strutData)
            {
                property.Type = typeof(StructPropertyData);
                for (int i = 0; i < strutData.Value.Count; i++)
                {
                    Property child = Property.MakeProperty(this, strutData.Value[i]);
                    property.Children.Add(child);
                    LoadPropertyChildren(property.Children[i]);
                }
            }
            else if (property.Data is MapPropertyData mapDat)
            {
                property.Type = typeof(MapPropertyData);

                foreach (var entry in mapDat.Value)
                {
                    Property key = Property.MakeProperty(this, entry.Key);
                    Property value = Property.MakeProperty(this, entry.Value);
                    property.Children.Add(key);
                    property.Children.Add(value);

                    LoadPropertyChildren(key);
                    LoadPropertyChildren(value);
                }
            }
            else
            {
                RecordUnknownProperty(property);
            }
        }

        public bool RecordUnknownProperty(Property dat)
        {
            if (dat == null) return false;

            if (dat.Data is UnknownPropertyData unknownDat)
            {
                Logger.Warning("Unknown Property Type: {0}", dat.Name);

                UnknownProperties.Add(dat);

            }
            if (dat.Data is RawStructPropertyData unknownDat2)
            {
                Logger.Warning("Unknown Property Type: {0}", dat.Name);

                UnknownProperties.Add(dat);
            }
            return false;
        }


        public override string ToString()  {
            return string.Format("Asset {0} (RootProperties: {1}, Properties: {2}, UnknownProperties: {3}, Names: {4})", Data.PackageGuid, RootProperties.Count, Properties.Count, UnknownProperties.Count, NameReferences.Count);
        }

    }
}
