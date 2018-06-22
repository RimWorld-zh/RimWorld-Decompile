using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E9 RID: 233
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		// Token: 0x06000500 RID: 1280 RVA: 0x00037B1C File Offset: 0x00035F1C
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

		// Token: 0x06000501 RID: 1281 RVA: 0x00037BA8 File Offset: 0x00035FA8
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

		// Token: 0x040002C8 RID: 712
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;
	}
}
