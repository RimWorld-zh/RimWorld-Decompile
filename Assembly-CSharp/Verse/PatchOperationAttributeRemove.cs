using System.Xml;

namespace Verse
{
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object item in xml.SelectNodes(base.xpath))
			{
				XmlNode xmlNode = item as XmlNode;
				if (xmlNode.Attributes[base.attribute] != null)
				{
					xmlNode.Attributes.Remove(xmlNode.Attributes[base.attribute]);
					result = true;
				}
			}
			return result;
		}
	}
}
