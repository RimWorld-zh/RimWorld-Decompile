using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDF RID: 3295
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06004889 RID: 18569 RVA: 0x00260AD0 File Offset: 0x0025EED0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
