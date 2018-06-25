using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006EE RID: 1774
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x0400157F RID: 5503
		public ThingSetMakerDef def;

		// Token: 0x0600269E RID: 9886 RVA: 0x0014AC0C File Offset: 0x0014900C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0014AC32 File Offset: 0x00149032
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0014AC4C File Offset: 0x0014904C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}
	}
}
