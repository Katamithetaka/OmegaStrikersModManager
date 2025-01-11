using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Objects;

namespace UAssetCommon
{
    public class ArrayProperty(Asset asset, ArrayPropertyData data) : Property(asset, data)
    {
        public ArrayPropertyData ArrayData { get => (ArrayPropertyData)Data; }

        public void Add(Property element)
        {
            var temp = ArrayData.Value.ToList();
            temp.Add(element.Data);
            Asset.Properties.Add(element);
            Children.Add(element);
            this.ArrayData.Value = [.. temp];
        }
    }
}
