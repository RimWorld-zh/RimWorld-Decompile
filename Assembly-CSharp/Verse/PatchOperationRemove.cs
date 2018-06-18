using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD6 RID: 3286
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06004875 RID: 18549 RVA: 0x002604C8 File Offset: 0x0025E8C8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			IEnumerator enumerator = xml.SelectNodes(this.xpath).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					result = true;
					XmlNode xmlNode = obj as XmlNode;
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
