using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000090 RID: 144
	public class JobDriver_PlantCut_Designated : JobDriver_PlantCut
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000291C0 File Offset: 0x000275C0
		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.CutPlant;
			}
		}
	}
}
