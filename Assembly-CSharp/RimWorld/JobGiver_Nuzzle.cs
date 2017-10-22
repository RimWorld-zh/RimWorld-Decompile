using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		private const float MaxNuzzleDistance = 15f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.RaceProps.nuzzleMtbHours <= 0.0)
			{
				result = null;
			}
			else
			{
				List<Pawn> source = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
				Pawn t = default(Pawn);
				if (!(from p in source
				where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 15f) && pawn.GetRoom(RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false)
				select p).TryRandomElement<Pawn>(out t))
				{
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.Nuzzle, (Thing)t);
					job.locomotionUrgency = LocomotionUrgency.Walk;
					job.expiryInterval = 3000;
					result = job;
				}
			}
			return result;
		}
	}
}
