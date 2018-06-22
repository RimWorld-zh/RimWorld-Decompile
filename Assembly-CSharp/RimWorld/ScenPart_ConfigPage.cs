using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000648 RID: 1608
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06002170 RID: 8560 RVA: 0x0011BC68 File Offset: 0x0011A068
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x0011BC92 File Offset: 0x0011A092
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
