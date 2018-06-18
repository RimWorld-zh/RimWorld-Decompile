using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CE0 RID: 3296
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x0600488B RID: 18571 RVA: 0x00260B4C File Offset: 0x0025EF4C
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

		// Token: 0x0600488C RID: 18572 RVA: 0x00260BEC File Offset: 0x0025EFEC
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false));
		}

		// Token: 0x0400310F RID: 12559
		private List<string> mods;

		// Token: 0x04003110 RID: 12560
		private PatchOperation match;

		// Token: 0x04003111 RID: 12561
		private PatchOperation nomatch;
	}
}
