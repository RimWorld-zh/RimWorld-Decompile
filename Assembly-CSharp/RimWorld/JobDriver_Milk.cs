using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000033 RID: 51
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00014118 File Offset: 0x00012518
		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00014134 File Offset: 0x00012534
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
