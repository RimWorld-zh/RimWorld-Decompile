using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDE RID: 3294
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x0600489B RID: 18587 RVA: 0x0026227C File Offset: 0x0026067C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
