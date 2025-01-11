using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.UnrealTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UAssetCommon
{
    public class NameRefList: List<NameRef>
    {
        public readonly Asset Asset;
        
        public NameRefList(Asset asset)
        {
            Asset = asset;
            ReloadNameMap();
        }

        public NameRef Add()
        {
            int index = Asset.Data.AddNameReference(new FString(), true);
            ReloadNameMap();
            return this[index];
        }

        public NameRef Add(string value)
        {
            int index = Asset.Data.AddNameReference(new FString(value), true);
            ReloadNameMap();
            return this[index];
        }

        public bool CheckNameUnused(NameRef name)
        {
            return CheckNameUnused(name.Index);
        }

        public bool CheckNameUnused(int index)
        {
            return GetNameUsageCount(index) == 0;
        }

        public int GetNameUsageCount(int index)
        {
            int count = 0;
            foreach (var prop in Asset.Properties)
            {
                if (prop.Data is NamePropertyData name)
                {
                    int i = Utils.GetIndexFromFName(name.Value);
                    if (index == i)
                    {
                        count += 1;
                    }
                }
            }
            return count;
        }

        public int GetNameUsageCount(FName name)
        {
            return GetNameUsageCount(Utils.GetIndexFromFName(name));
        }

        public int GetNameUsageCount(NameRef name)
        {
            return GetNameUsageCount(name.Index);
        }

        public bool GetIndex(string name, out int index)
        {
            index = -1;
            var n = new FString(name);
            if(Asset.Data.ContainsNameReference(n))
            {
                index = Asset.Data.SearchNameReference(n);
                return true;
            }
            return false;
        }

        public void ReloadNameMap()
        {
            this.Clear();
            var data = Asset.Data.GetNameMapIndexList();
            for (int i = 0; i < data.Count; i++)
            {
                this.Add(new NameRef(Asset.Data, i));
            }
        }
    }
}
