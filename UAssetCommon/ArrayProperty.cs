using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;

namespace UAssetCommon
{
    public class StructProperty(Asset asset, StructPropertyData data) : Property(asset, data)
    {
        public StructPropertyData StructData { get => (StructPropertyData)Data; }

        public void Add(Property element)
        {
            var temp = StructData.Value.ToList();
            temp.Add(element.Data);
            Asset.Properties.Add(element);
            Children.Add(element);
            this.StructData.Value = [.. temp];
        }
    }
}
