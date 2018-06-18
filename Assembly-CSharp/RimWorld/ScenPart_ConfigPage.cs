using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064C RID: 1612
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06002178 RID: 8568 RVA: 0x0011BB68 File Offset: 0x00119F68
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x0011BB92 File Offset: 0x00119F92
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
