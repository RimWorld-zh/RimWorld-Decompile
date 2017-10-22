using System.Xml;

namespace Verse
{
	public class PatchOperationInsert : PatchOperationPathed
	{
		private enum Order
		{
			Append = 0,
			Prepend = 1
		}

		private XmlContainer value;

		private Order order = Order.Prepend;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object item in xml.SelectNodes(base.xpath))
			{
				result = true;
				XmlNode xmlNode = item as XmlNode;
				XmlNode parentNode = xmlNode.ParentNode;
				if (this.order == Order.Append)
				{
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						parentNode.InsertAfter(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
					}
				}
				else if (this.order == Order.Prepend)
				{
					for (int num = node.ChildNodes.Count - 1; num >= 0; num--)
					{
						parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[num], true), xmlNode);
					}
				}
			}
			return result;
		}
	}
}
