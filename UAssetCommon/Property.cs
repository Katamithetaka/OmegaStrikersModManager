using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.PropertyTypes.Objects;

namespace UAssetCommon
{
    public class Property
    {
        public string Name { get; set; }

        public List<Property> Children = new List<Property>();
        public Property? Parent;
        public PropertyData Data;
        public Type Type = typeof(PropertyData);
        public readonly Asset Asset;
        protected Property(Asset asset, PropertyData data)
        {
            Data = data;
            if(data.Name != null)
            {
            Name = data.Name.Value.Value;
            }
            Asset = asset;
        }

        public Property? GetChildByName(string name)
        {
            foreach (var property in Children)
            {
                if (property.Name == name) return property;
            }

            return null;
        }

        public Property? GetChildByPath(string name)
        {
            return GetChildByPath(name.Split('.'));
        }

        public Property? GetChildByPath(string[] names)
        {
            Property? p = this;
            foreach (var part in names)
            {
                p = p.GetChildByName(part);
                if (p == null) return null;
            }

            return p; 
        }

        public override string ToString()
        {
            return string.Format("Property {0} (Children: {1}, Has a Parent: {2}, Type: {3})", Name, Children.Count, Parent != null, Type.Name);
        }

        public static Property MakeProperty(Asset asset, PropertyData data)
        {
            if (data is NamePropertyData n)
            {
                return new NameProperty(asset, n);
            }
            else if (data is ArrayPropertyData arrayProperty)
            {
                return new ArrayProperty(asset, arrayProperty);
            }

            return new Property(asset, data);
        }
    }
}
