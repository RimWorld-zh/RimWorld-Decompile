using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD7 RID: 3287
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x06004877 RID: 18551 RVA: 0x00260558 File Offset: 0x0025E958
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

		// Token: 0x04003106 RID: 12550
		private XmlContainer value;
	}
}
