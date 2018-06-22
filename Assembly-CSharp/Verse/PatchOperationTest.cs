using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDB RID: 3291
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06004898 RID: 18584 RVA: 0x00261EC0 File Offset: 0x002602C0
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
