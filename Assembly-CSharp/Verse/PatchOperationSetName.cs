using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD7 RID: 3287
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x04003112 RID: 12562
		protected string name;

		// Token: 0x0600488D RID: 18573 RVA: 0x00261B34 File Offset: 0x0025FF34
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
