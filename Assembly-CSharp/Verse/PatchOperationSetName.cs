using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD8 RID: 3288
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x06004879 RID: 18553 RVA: 0x00260640 File Offset: 0x0025EA40
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

		// Token: 0x04003107 RID: 12551
		protected string name;
	}
}
