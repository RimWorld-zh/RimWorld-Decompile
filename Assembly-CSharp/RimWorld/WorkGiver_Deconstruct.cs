using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200012E RID: 302
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x00041D74 File Offset: 0x00040174
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00041D90 File Offset: 0x00040190
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00041DAC File Offset: 0x000401AC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
