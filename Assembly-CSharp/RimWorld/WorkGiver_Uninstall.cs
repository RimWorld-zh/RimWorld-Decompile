using System;
using Verse;

namespace RimWorld
{
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		public WorkGiver_Uninstall()
		{
		}

		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

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
