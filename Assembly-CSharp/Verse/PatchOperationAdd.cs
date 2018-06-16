using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD2 RID: 3282
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x06004871 RID: 18545 RVA: 0x0026015C File Offset: 0x0025E55C
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

		// Token: 0x040030FD RID: 12541
		private XmlContainer value;

		// Token: 0x040030FE RID: 12542
		private PatchOperationAdd.Order order = PatchOperationAdd.Order.Append;

		// Token: 0x02000CD3 RID: 3283
		private enum Order
		{
			// Token: 0x04003100 RID: 12544
			Append,
			// Token: 0x04003101 RID: 12545
			Prepend
		}
	}
}
