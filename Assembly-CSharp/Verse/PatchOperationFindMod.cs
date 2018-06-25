using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CE0 RID: 3296
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x04003121 RID: 12577
		private List<string> mods;

		// Token: 0x04003122 RID: 12578
		private PatchOperation match;

		// Token: 0x04003123 RID: 12579
		private PatchOperation nomatch;

		// Token: 0x0600489F RID: 18591 RVA: 0x00262320 File Offset: 0x00260720
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

		// Token: 0x060048A0 RID: 18592 RVA: 0x002623C0 File Offset: 0x002607C0
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false));
		}
	}
}
