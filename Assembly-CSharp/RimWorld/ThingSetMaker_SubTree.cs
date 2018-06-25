using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006EE RID: 1774
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x04001583 RID: 5507
		public ThingSetMakerDef def;

		// Token: 0x0600269D RID: 9885 RVA: 0x0014AE6C File Offset: 0x0014926C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0014AE92 File Offset: 0x00149292
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0014AEAC File Offset: 0x001492AC
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}
	}
}
