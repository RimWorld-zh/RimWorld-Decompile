using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_SocialRelax : JoyGiver
	{
		private static List<CompGatherSpot> workingSpots = new List<CompGatherSpot>();

		private const float GatherRadius = 3.9f;

		private static readonly int NumRadiusCells = GenRadial.NumCellsInRadius(3.9f);

		private static readonly List<IntVec3> RadialPatternMiddleOutward = (from c in GenRadial.RadialPattern.Take(JoyGiver_SocialRelax.NumRadiusCells)
		orderby Mathf.Abs((float)((c - IntVec3.Zero).LengthHorizontal - 1.9500000476837158))
		select c).ToList();

		private static List<ThingDef> nurseableDrugs = new List<ThingDef>();

		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInt(pawn, null);
		}

		public override Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return this.TryGiveJobInt(pawn, (Predicate<CompGatherSpot>)((CompGatherSpot x) => PartyUtility.InPartyArea(x.parent.Position, partySpot, pawn.Map)));
		}

		private Job TryGiveJobInt(Pawn pawn, Predicate<CompGatherSpot> gatherSpotValidator)
		{
			Job result;
			if (pawn.Map.gatherSpotLister.activeSpots.Count == 0)
			{
				result = null;
				goto IL_0271;
			}
			JoyGiver_SocialRelax.workingSpots.Clear();
			for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
			{
				JoyGiver_SocialRelax.workingSpots.Add(pawn.Map.gatherSpotLister.activeSpots[i]);
			}
			goto IL_0074;
			IL_011c:
			CompGatherSpot compGatherSpot = default(CompGatherSpot);
			Job job;
			if (compGatherSpot.parent.def.surfaceType == SurfaceType.Eat)
			{
				Thing t = default(Thing);
				if (JoyGiver_SocialRelax.TryFindChairBesideTable((Thing)compGatherSpot.parent, pawn, out t))
				{
					job = new Job(base.def.jobDef, (Thing)compGatherSpot.parent, t);
					goto IL_0204;
				}
				result = null;
			}
			else
			{
				Thing t2 = default(Thing);
				if (JoyGiver_SocialRelax.TryFindChairNear(compGatherSpot.parent.Position, pawn, out t2))
				{
					job = new Job(base.def.jobDef, (Thing)compGatherSpot.parent, t2);
					goto IL_0204;
				}
				IntVec3 c = default(IntVec3);
				if (JoyGiver_SocialRelax.TryFindSitSpotOnGroundNear(compGatherSpot.parent.Position, pawn, out c))
				{
					job = new Job(base.def.jobDef, (Thing)compGatherSpot.parent, c);
					goto IL_0204;
				}
				result = null;
			}
			goto IL_0271;
			IL_0117:
			goto IL_0074;
			IL_0271:
			return result;
			IL_0074:
			while (((IEnumerable<CompGatherSpot>)JoyGiver_SocialRelax.workingSpots).TryRandomElement<CompGatherSpot>(out compGatherSpot))
			{
				JoyGiver_SocialRelax.workingSpots.Remove(compGatherSpot);
				if (compGatherSpot.parent.IsForbidden(pawn))
					continue;
				if (!pawn.CanReach((Thing)compGatherSpot.parent, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn))
					continue;
				if (!compGatherSpot.parent.IsSociallyProper(pawn))
					continue;
				if (!compGatherSpot.parent.IsPoliticallyProper(pawn))
					continue;
				if ((object)gatherSpotValidator != null && !gatherSpotValidator(compGatherSpot))
					continue;
				goto IL_011c;
			}
			result = null;
			goto IL_0271;
			IL_0204:
			Thing thing = default(Thing);
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && JoyGiver_SocialRelax.TryFindIngestibleToNurse(compGatherSpot.parent.Position, pawn, out thing))
			{
				job.targetC = thing;
				job.count = Mathf.Min(thing.stackCount, thing.def.ingestible.maxNumToIngestAtOnce);
			}
			result = job;
			goto IL_0271;
		}

		private static bool TryFindIngestibleToNurse(IntVec3 center, Pawn ingester, out Thing ingestible)
		{
			bool result;
			if (ingester.IsTeetotaler())
			{
				ingestible = null;
				result = false;
			}
			else if (ingester.drugs == null)
			{
				ingestible = null;
				result = false;
			}
			else
			{
				JoyGiver_SocialRelax.nurseableDrugs.Clear();
				DrugPolicy currentPolicy = ingester.drugs.CurrentPolicy;
				for (int i = 0; i < currentPolicy.Count; i++)
				{
					if (currentPolicy[i].allowedForJoy && currentPolicy[i].drug.ingestible.nurseable)
					{
						JoyGiver_SocialRelax.nurseableDrugs.Add(currentPolicy[i].drug);
					}
				}
				JoyGiver_SocialRelax.nurseableDrugs.Shuffle();
				for (int j = 0; j < JoyGiver_SocialRelax.nurseableDrugs.Count; j++)
				{
					List<Thing> list = ingester.Map.listerThings.ThingsOfDef(JoyGiver_SocialRelax.nurseableDrugs[j]);
					if (list.Count > 0)
					{
						Predicate<Thing> validator = (Predicate<Thing>)((Thing t) => ingester.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(ingester));
						ingestible = GenClosest.ClosestThing_Global_Reachable(center, ingester.Map, list, PathEndMode.OnCell, TraverseParms.For(ingester, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null);
						if (ingestible != null)
							goto IL_013f;
					}
				}
				ingestible = null;
				result = false;
			}
			goto IL_0169;
			IL_013f:
			result = true;
			goto IL_0169;
			IL_0169:
			return result;
		}

		private static bool TryFindChairBesideTable(Thing table, Pawn sitter, out Thing chair)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < 30)
				{
					IntVec3 c = table.RandomAdjacentCellCardinal();
					Building edifice = c.GetEdifice(table.Map);
					if (edifice != null && edifice.def.building.isSittable && sitter.CanReserve((Thing)edifice, 1, -1, null, false))
					{
						chair = edifice;
						result = true;
						break;
					}
					num++;
					continue;
				}
				chair = null;
				result = false;
				break;
			}
			return result;
		}

		private static bool TryFindChairNear(IntVec3 center, Pawn sitter, out Thing chair)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < JoyGiver_SocialRelax.RadialPatternMiddleOutward.Count)
				{
					IntVec3 c = center + JoyGiver_SocialRelax.RadialPatternMiddleOutward[num];
					Building edifice = c.GetEdifice(sitter.Map);
					if (edifice != null && edifice.def.building.isSittable && sitter.CanReserve((Thing)edifice, 1, -1, null, false) && !edifice.IsForbidden(sitter) && GenSight.LineOfSight(center, edifice.Position, sitter.Map, true, null, 0, 0))
					{
						chair = edifice;
						result = true;
						break;
					}
					num++;
					continue;
				}
				chair = null;
				result = false;
				break;
			}
			return result;
		}

		private static bool TryFindSitSpotOnGroundNear(IntVec3 center, Pawn sitter, out IntVec3 result)
		{
			int num = 0;
			bool result2;
			while (true)
			{
				if (num < 30)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[Rand.Range(1, JoyGiver_SocialRelax.NumRadiusCells)];
					if (sitter.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && intVec.GetEdifice(sitter.Map) == null && GenSight.LineOfSight(center, intVec, sitter.Map, true, null, 0, 0))
					{
						result = intVec;
						result2 = true;
						break;
					}
					num++;
					continue;
				}
				result = IntVec3.Invalid;
				result2 = false;
				break;
			}
			return result2;
		}
	}
}
