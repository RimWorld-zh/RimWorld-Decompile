using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD6 RID: 3286
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x04003111 RID: 12561
		private XmlContainer value;

		// Token: 0x0600488B RID: 18571 RVA: 0x00261A4C File Offset: 0x0025FE4C
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
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
					}
					parentNode.RemoveChild(xmlNode);
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
	}
}
