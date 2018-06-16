using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CE0 RID: 3296
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x0600488B RID: 18571 RVA: 0x00260B00 File Offset: 0x0025EF00
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

		// Token: 0x0400310F RID: 12559
		private PatchOperation match;

		// Token: 0x04003110 RID: 12560
		private PatchOperation nomatch;
	}
}
