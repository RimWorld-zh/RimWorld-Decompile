using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDC RID: 3292
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x0600489A RID: 18586 RVA: 0x00261EF0 File Offset: 0x002602F0
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

		// Token: 0x04003118 RID: 12568
		private PatchOperation match;

		// Token: 0x04003119 RID: 12569
		private PatchOperation nomatch;
	}
}
