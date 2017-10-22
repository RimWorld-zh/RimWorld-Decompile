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
			Job result;
			if (this.ShouldStartEscaping(pawn) && RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				if (!pawn.guest.Released)
				{
					Messages.Message("MessagePrisonerIsEscaping".Translate(pawn.NameStringShort), (Thing)pawn, MessageTypeDefOf.ThreatSmall);
				}
				Job job = new Job(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				result = job;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private bool ShouldStartEscaping(Pawn pawn)
		{
			bool result;
			if (!pawn.guest.IsPrisoner || pawn.guest.HostFaction != Faction.OfPlayer || !pawn.guest.PrisonerIsSecure)
			{
				result = false;
			}
			else
			{
				Room room = pawn.GetRoom(RegionType.Set_Passable);
				if (room.TouchesMapEdge)
				{
					result = true;
				}
				else
				{
					bool found = false;
					RegionTraverser.BreadthFirstTraverse(room.Regions[0], (RegionEntryPredicate)((Region from, Region reg) => (byte)((reg.portal == null || reg.portal.FreePassage) ? 1 : 0) != 0), (RegionProcessor)delegate(Region reg)
					{
						bool result2;
						if (reg.Room.TouchesMapEdge)
						{
							found = true;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
						return result2;
					}, 25, RegionType.Set_Passable);
					result = ((byte)(found ? 1 : 0) != 0);
				}
			}
			return result;
		}
	}
}
