using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064C RID: 1612
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06002176 RID: 8566 RVA: 0x0011BAF0 File Offset: 0x00119EF0
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x0011BB1A File Offset: 0x00119F1A
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
