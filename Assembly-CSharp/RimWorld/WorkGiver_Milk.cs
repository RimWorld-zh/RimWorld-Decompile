using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200011F RID: 287
	public class WorkGiver_Milk : WorkGiver_GatherAnimalBodyResources
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0003F96C File Offset: 0x0003DD6C
		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Milk;
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0003F988 File Offset: 0x0003DD88
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
