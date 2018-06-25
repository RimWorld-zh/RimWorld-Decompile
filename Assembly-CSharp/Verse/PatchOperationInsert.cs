using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD4 RID: 3284
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x04003113 RID: 12563
		private XmlContainer value;

		// Token: 0x04003114 RID: 12564
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x06004887 RID: 18567 RVA: 0x00261B54 File Offset: 0x0025FF54
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

		// Token: 0x02000CD5 RID: 3285
		private enum Order
		{
			// Token: 0x04003116 RID: 12566
			Append,
			// Token: 0x04003117 RID: 12567
			Prepend
		}
	}
}
