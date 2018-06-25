using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;

		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache0;

		public JobGiver_PrisonerEscape()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.ShouldStartEscaping(pawn))
			{
				IntVec3 c;
				if (RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
				{
					if (!pawn.guest.Released)
					{
						Messages.Message("MessagePrisonerIsEscaping".Translate(new object[]
						{
							pawn.LabelShort
						}), pawn, MessageTypeDefOf.ThreatSmall, true);
					}
					return new Job(JobDefOf.Goto, c)
					{
						exitMapOnArrival = true
					};
				}
			}
			return null;
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
					RegionTraverser.BreadthFirstTraverse(room.Regions[0], (Region from, Region reg) => reg.portal == null || reg.portal.FreePassage, delegate(Region reg)
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
					result = found;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <ShouldStartEscaping>m__0(Region from, Region reg)
		{
			return reg.portal == null || reg.portal.FreePassage;
		}

		[CompilerGenerated]
		private sealed class <ShouldStartEscaping>c__AnonStorey0
		{
			internal bool found;

			public <ShouldStartEscaping>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region reg)
			{
				bool result;
				if (reg.Room.TouchesMapEdge)
				{
					this.found = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
	}
}
