using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064A RID: 1610
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06002173 RID: 8563 RVA: 0x0011C020 File Offset: 0x0011A420
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x0011C04A File Offset: 0x0011A44A
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
