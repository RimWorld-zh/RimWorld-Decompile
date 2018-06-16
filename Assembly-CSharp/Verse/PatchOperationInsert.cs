using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD5 RID: 3285
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06004875 RID: 18549 RVA: 0x002603A8 File Offset: 0x0025E7A8
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

		// Token: 0x04003103 RID: 12547
		private XmlContainer value;

		// Token: 0x04003104 RID: 12548
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x02000CD6 RID: 3286
		private enum Order
		{
			// Token: 0x04003106 RID: 12550
			Append,
			// Token: 0x04003107 RID: 12551
			Prepend
		}
	}
}
