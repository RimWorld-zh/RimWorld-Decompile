using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200012E RID: 302
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00041D8C File Offset: 0x0004018C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x00041DA8 File Offset: 0x000401A8
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00041DC4 File Offset: 0x000401C4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
