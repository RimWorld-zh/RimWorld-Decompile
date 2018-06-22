using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD3 RID: 3283
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06004886 RID: 18566 RVA: 0x002618E0 File Offset: 0x0025FCE0
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
