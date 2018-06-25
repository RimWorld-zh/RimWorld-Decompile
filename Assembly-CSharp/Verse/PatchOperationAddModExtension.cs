using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD3 RID: 3283
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		// Token: 0x04003112 RID: 12562
		private XmlContainer value;

		// Token: 0x06004885 RID: 18565 RVA: 0x00261A44 File Offset: 0x0025FE44
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
					XmlNode xmlNode = obj as XmlNode;
					XmlNode xmlNode2 = xmlNode["modExtensions"];
					if (xmlNode2 == null)
					{
						xmlNode2 = xmlNode.OwnerDocument.CreateElement("modExtensions");
						xmlNode.AppendChild(xmlNode2);
					}
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
					result = true;
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
