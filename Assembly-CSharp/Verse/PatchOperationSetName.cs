using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD8 RID: 3288
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x04003119 RID: 12569
		protected string name;

		// Token: 0x0600488D RID: 18573 RVA: 0x00261E14 File Offset: 0x00260214
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			IEnumerator enumerator = xml.SelectNodes(this.xpath).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode = obj as XmlNode;
					XmlNode xmlNode2 = xmlNode.OwnerDocument.CreateElement(this.name);
					xmlNode2.InnerXml = xmlNode.InnerXml;
					xmlNode.ParentNode.InsertBefore(xmlNode2, xmlNode);
					xmlNode.ParentNode.RemoveChild(xmlNode);
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
