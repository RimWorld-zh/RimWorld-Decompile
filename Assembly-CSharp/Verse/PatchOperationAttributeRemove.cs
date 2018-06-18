using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDB RID: 3291
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x0600487F RID: 18559 RVA: 0x002607F8 File Offset: 0x0025EBF8
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
					if (xmlNode.Attributes[this.attribute] != null)
					{
						xmlNode.Attributes.Remove(xmlNode.Attributes[this.attribute]);
						result = true;
					}
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
