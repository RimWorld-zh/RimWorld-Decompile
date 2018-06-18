using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD4 RID: 3284
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06004873 RID: 18547 RVA: 0x00260380 File Offset: 0x0025E780
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

		// Token: 0x04003101 RID: 12545
		private XmlContainer value;

		// Token: 0x04003102 RID: 12546
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x02000CD5 RID: 3285
		private enum Order
		{
			// Token: 0x04003104 RID: 12548
			Append,
			// Token: 0x04003105 RID: 12549
			Prepend
		}
	}
}
