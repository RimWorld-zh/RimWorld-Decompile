using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000120 RID: 288
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0003F9C4 File Offset: 0x0003DDC4
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0003F9E0 File Offset: 0x0003DDE0
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
