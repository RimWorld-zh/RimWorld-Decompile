using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD0 RID: 3280
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x04003106 RID: 12550
		private XmlContainer value;

		// Token: 0x04003107 RID: 12551
		private PatchOperationAdd.Order order = PatchOperationAdd.Order.Append;

		// Token: 0x06004883 RID: 18563 RVA: 0x00261628 File Offset: 0x0025FA28
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

		// Token: 0x02000CD1 RID: 3281
		private enum Order
		{
			// Token: 0x04003109 RID: 12553
			Append,
			// Token: 0x0400310A RID: 12554
			Prepend
		}
	}
}
