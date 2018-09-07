using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationAdd : PatchOperationPathed
	{
		private XmlContainer value;

		private PatchOperationAdd.Order order;

		public PatchOperationAdd()
		{
		}

		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			IEnumerator enumerator = xml.SelectNodes(this.xpath).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					result = true;
					XmlNode xmlNode = obj as XmlNode;
					if (this.order == PatchOperationAdd.Order.Append)
					{
						for (int i = 0; i < node.ChildNodes.Count; i++)
						{
							xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
						}
					}
					else if (this.order == PatchOperationAdd.Order.Prepend)
					{
						for (int j = node.ChildNodes.Count - 1; j >= 0; j--)
						{
							xmlNode.PrependChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[j], true));
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}

		private enum Order
		{
			Append,
			Prepend
		}
	}
}
