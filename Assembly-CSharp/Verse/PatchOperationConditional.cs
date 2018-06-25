using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDF RID: 3295
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x0400311F RID: 12575
		private PatchOperation match;

		// Token: 0x04003120 RID: 12576
		private PatchOperation nomatch;

		// Token: 0x0600489D RID: 18589 RVA: 0x002622AC File Offset: 0x002606AC
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
	}
}
