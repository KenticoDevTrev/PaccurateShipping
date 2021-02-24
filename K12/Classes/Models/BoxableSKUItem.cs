using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaccurateShipping.Models
{
    /// <summary>
    /// Stores Boxable Sku Configurations to be used to overwrite the Settings defaults if present on a Sku, used only if the item can't be packaged in an existing box (is a leftover)
    /// </summary>
    [Serializable]
    public class BoxableSKUItem
    {
        public const string _BoxableSKUItemSettingKey = "BoxableSKUItemSettings";
        public ShippableSeparateType IsShippableSeparate { get; set; }

        public bool IsShippableAsIs { get; set; }

        public bool UseSpecifiedRates { get; set; }

        public string WeightRates { get; set; }

        public decimal? CostPerWeightUnit { get; set; }

        public BoxableSKUItem()
        {

        }

        public enum ShippableSeparateType
        {
            UseSettingsDefault, Yes, No
        }

        public string ToXML()
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                x.Serialize(writer, this);
            }
            return builder.ToString();
        }

        public static BoxableSKUItem XmlToObject(string Xml)
        {
            if(string.IsNullOrWhiteSpace(Xml))
            {
                return null;
            }
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(BoxableSKUItem));
            BoxableSKUItem item = null;
            using (StringReader reader = new StringReader(Xml))
            {
                item = (BoxableSKUItem)x.Deserialize(reader);
            }
            return item;
        }
    }
}