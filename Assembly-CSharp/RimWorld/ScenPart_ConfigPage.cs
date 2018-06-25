using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064A RID: 1610
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06002174 RID: 8564 RVA: 0x0011BDB8 File Offset: 0x0011A1B8
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x0011BDE2 File Offset: 0x0011A1E2
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
