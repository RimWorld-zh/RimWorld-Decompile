using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationInsert : PatchOperationPathed
	{
		private XmlContainer value;

		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		public PatchOperationInsert()
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
					XmlNode parentNode = xmlNode.ParentNode;
					if (this.order == PatchOperationInsert.Order.Append)
					{
						for (int i = 0; i < node.ChildNodes.Count; i++)
						{
							parentNode.InsertAfter(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
						}
					}
					else if (this.order == PatchOperationInsert.Order.Prepend)
					{
						for (int j = node.ChildNodes.Count - 1; j >= 0; j--)
						{
							parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[j], true), xmlNode);
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
