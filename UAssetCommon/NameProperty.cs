using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Objects;

namespace UAssetCommon
{
    public class NameProperty(Asset asset, NamePropertyData data) : Property(asset, data)
    {
        public readonly NameRef nameRef = new(asset.Data, data.Value);

        public object[] GetItemArray()
        {
            return [
                nameRef.Index,
                nameRef.Value
            ];
        }

    }
}
