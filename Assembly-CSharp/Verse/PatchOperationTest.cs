using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDE RID: 3294
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06004887 RID: 18567 RVA: 0x00260AA8 File Offset: 0x0025EEA8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
