using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CE1 RID: 3297
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x0600488D RID: 18573 RVA: 0x00260B74 File Offset: 0x0025EF74
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool flag = false;
			for (int i = 0; i < this.mods.Count; i++)
			{
				if (ModLister.HasActiveModWithName(this.mods[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
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

		// Token: 0x0600488E RID: 18574 RVA: 0x00260C14 File Offset: 0x0025F014
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false));
		}

		// Token: 0x04003111 RID: 12561
		private List<string> mods;

		// Token: 0x04003112 RID: 12562
		private PatchOperation match;

		// Token: 0x04003113 RID: 12563
		private PatchOperation nomatch;
	}
}
