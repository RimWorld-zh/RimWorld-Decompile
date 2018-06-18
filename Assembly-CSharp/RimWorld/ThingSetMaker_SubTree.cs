using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F0 RID: 1776
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x060026A2 RID: 9890 RVA: 0x0014A918 File Offset: 0x00148D18
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0014A93E File Offset: 0x00148D3E
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0014A958 File Offset: 0x00148D58
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x04001581 RID: 5505
		public ThingSetMakerDef def;
	}
}
