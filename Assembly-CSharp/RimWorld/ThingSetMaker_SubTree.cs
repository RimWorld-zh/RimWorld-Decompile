using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006EC RID: 1772
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x0600269A RID: 9882 RVA: 0x0014AABC File Offset: 0x00148EBC
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0014AAE2 File Offset: 0x00148EE2
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0014AAFC File Offset: 0x00148EFC
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x0400157F RID: 5503
		public ThingSetMakerDef def;
	}
}
