using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD6 RID: 3286
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06004889 RID: 18569 RVA: 0x00261C9C File Offset: 0x0026009C
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
