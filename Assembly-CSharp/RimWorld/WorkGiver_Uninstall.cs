using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000131 RID: 305
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00041FDC File Offset: 0x000403DC
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00041FF8 File Offset: 0x000403F8
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00042014 File Offset: 0x00040414
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.Claimable)
			{
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}
	}
}
