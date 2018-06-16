using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F0 RID: 1776
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x060026A0 RID: 9888 RVA: 0x0014A8A0 File Offset: 0x00148CA0
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0014A8C6 File Offset: 0x00148CC6
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0014A8E0 File Offset: 0x00148CE0
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x04001581 RID: 5505
		public ThingSetMakerDef def;
	}
}
