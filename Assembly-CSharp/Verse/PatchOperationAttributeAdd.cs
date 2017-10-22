using System.Xml;

namespace Verse
{
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		protected string value;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object item in xml.SelectNodes(base.xpath))
			{
				XmlNode xmlNode = item as XmlNode;
				if (xmlNode.Attributes[base.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(base.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}
	}
}
