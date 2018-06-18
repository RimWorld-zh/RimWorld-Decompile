using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDF RID: 3295
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x06004889 RID: 18569 RVA: 0x00260AD8 File Offset: 0x0025EED8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return false;
		}

		// Token: 0x0400310D RID: 12557
		private PatchOperation match;

		// Token: 0x0400310E RID: 12558
		private PatchOperation nomatch;
	}
}
