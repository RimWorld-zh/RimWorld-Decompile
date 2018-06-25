using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		private const float MaxNuzzleDistance = 40f;

		public JobGiver_Nuzzle()
		{
		}

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

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn p)
			{
				return !p.NonHumanlikeOrWildMan() && p != this.pawn && p.Position.InHorDistOf(this.pawn.Position, 40f) && this.pawn.GetRoom(RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable) && !p.Position.IsForbidden(this.pawn) && p.CanCasuallyInteractNow(false);
			}
		}
	}
}
