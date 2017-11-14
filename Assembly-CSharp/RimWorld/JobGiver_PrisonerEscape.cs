using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c = default(IntVec3);
			if (this.ShouldStartEscaping(pawn) && RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				if (!pawn.guest.Released)
				{
					Messages.Message("MessagePrisonerIsEscaping".Translate(pawn.NameStringShort), pawn, MessageTypeDefOf.ThreatSmall);
				}
				Job job = new Job(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				return job;
			}
			return null;
		}

		private bool ShouldStartEscaping(Pawn pawn)
		{
			if (pawn.guest.IsPrisoner && pawn.guest.HostFaction == Faction.OfPlayer && pawn.guest.PrisonerIsSecure)
			{
				Room room = pawn.GetRoom(RegionType.Set_Passable);
				if (room.TouchesMapEdge)
				{
					return true;
				}
				bool found = false;
				RegionTraverser.BreadthFirstTraverse(room.Regions[0], delegate(Region from, Region reg)
				{
					if (reg.portal != null && !reg.portal.FreePassage)
					{
						return false;
					}
					return true;
				}, delegate(Region reg)
				{
					if (reg.Room.TouchesMapEdge)
					{
						found = true;
						return true;
					}
					return false;
				}, 25, RegionType.Set_Passable);
				if (found)
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
