using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200008F RID: 143
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0002919C File Offset: 0x0002759C
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
