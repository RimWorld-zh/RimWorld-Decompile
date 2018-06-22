using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000034 RID: 52
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00014158 File Offset: 0x00012558
		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00014174 File Offset: 0x00012574
		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
