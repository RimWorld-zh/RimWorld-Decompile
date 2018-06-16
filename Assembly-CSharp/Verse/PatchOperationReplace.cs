using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD8 RID: 3288
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x06004879 RID: 18553 RVA: 0x00260580 File Offset: 0x0025E980
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

		// Token: 0x04003108 RID: 12552
		private XmlContainer value;
	}
}
