using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD1 RID: 3281
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06004884 RID: 18564 RVA: 0x00261798 File Offset: 0x0025FB98
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

		// Token: 0x0400310C RID: 12556
		private XmlContainer value;

		// Token: 0x0400310D RID: 12557
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x02000CD2 RID: 3282
		private enum Order
		{
			// Token: 0x0400310F RID: 12559
			Append,
			// Token: 0x04003110 RID: 12560
			Prepend
		}
	}
}
