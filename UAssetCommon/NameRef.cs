using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.UnrealTypes;

namespace UAssetCommon
{
    public class NameRef
    {
        public string Value { 
            get
            {
                return Asset.GetNameReference(Index).Value;
            }
            set
            {
                Asset.SetNameReference(Index, new FString(value));
            }
        }

        public readonly UAsset Asset;
        public int Index;

        public NameRef(UAsset asset, int index)
        {
            Asset = asset;
            Index = index;
        }

        public NameRef(UAsset asset, FName name)
        {
            Asset = asset;

            Index = Utils.GetIndexFromFName(name);

        }
    }
}
