using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A7 RID: 167
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		// Token: 0x04000271 RID: 625
		private const float MaxNuzzleDistance = 40f;

		// Token: 0x06000416 RID: 1046 RVA: 0x00030DBC File Offset: 0x0002F1BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.RaceProps.nuzzleMtbHours <= 0f)
			{
				result = null;
			}
			else
			{
				List<Pawn> source = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
				Pawn t;
				if (!(from p in source
				where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 40f) && pawn.GetRoom(RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false)
				select p).TryRandomElement(out t))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Nuzzle, t)
					{
						locomotionUrgency = LocomotionUrgency.Walk,
						expiryInterval = 3000
					};
				}
			}
			return result;
		}
	}
}
