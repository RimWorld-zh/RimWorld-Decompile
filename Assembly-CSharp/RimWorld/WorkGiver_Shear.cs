using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000120 RID: 288
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0003F9AC File Offset: 0x0003DDAC
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0003F9C8 File Offset: 0x0003DDC8
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
