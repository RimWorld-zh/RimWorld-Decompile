using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDD RID: 3293
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x0600489B RID: 18587 RVA: 0x00261F9C File Offset: 0x0026039C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
