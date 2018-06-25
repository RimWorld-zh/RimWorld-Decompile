using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDF RID: 3295
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x0400311A RID: 12570
		private List<string> mods;

		// Token: 0x0400311B RID: 12571
		private PatchOperation match;

		// Token: 0x0400311C RID: 12572
		private PatchOperation nomatch;

		// Token: 0x0600489F RID: 18591 RVA: 0x00262040 File Offset: 0x00260440
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

		// Token: 0x060048A0 RID: 18592 RVA: 0x002620E0 File Offset: 0x002604E0
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false));
		}
	}
}
