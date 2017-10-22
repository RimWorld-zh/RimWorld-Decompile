using System.Xml;

namespace Verse
{
	public class PatchOperationRemove : PatchOperationPathed
	{
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object item in xml.SelectNodes(base.xpath))
			{
				result = true;
				XmlNode xmlNode = item as XmlNode;
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}
	}
}
