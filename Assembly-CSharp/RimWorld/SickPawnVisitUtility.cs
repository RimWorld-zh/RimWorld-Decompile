using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class SickPawnVisitUtility
	{
		public static Pawn FindRandomSickPawn(Pawn pawn, JoyCategory maxPatientJoy)
		{
			IEnumerable<Pawn> source = from x in pawn.Map.mapPawns.FreeColonistsSpawned
			where SickPawnVisitUtility.CanVisit(pawn, x, maxPatientJoy)
			select x;
			Pawn pawn2 = default(Pawn);
			return source.TryRandomElementByWeight<Pawn>((Func<Pawn, float>)((Pawn x) => SickPawnVisitUtility.VisitChanceScore(pawn, x)), out pawn2) ? pawn2 : null;
		}

		public static bool CanVisit(Pawn pawn, Pawn sick, JoyCategory maxPatientJoy)
		{
			return sick.IsColonist && !sick.Dead && pawn != sick && sick.InBed() && sick.Awake() && !sick.IsForbidden(pawn) && sick.needs.joy != null && (int)sick.needs.joy.CurCategory <= (int)maxPatientJoy && InteractionUtility.CanReceiveInteraction(sick) && !sick.needs.food.Starving && sick.needs.rest.CurLevel > 0.33000001311302185 && pawn.CanReserveAndReach((Thing)sick, PathEndMode.InteractionCell, Danger.None, 1, -1, null, false) && !SickPawnVisitUtility.AboutToRecover(sick);
		}

		public static Thing FindChair(Pawn forPawn, Pawn nearPawn)
		{
			Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing x)
			{
				bool result;
				if (!x.def.building.isSittable)
				{
					result = false;
				}
				else if (x.IsForbidden(forPawn))
				{
					result = false;
				}
				else if (!GenSight.LineOfSight(x.Position, nearPawn.Position, nearPawn.Map, false, null, 0, 0))
				{
					result = false;
				}
				else if (!forPawn.CanReserve(x, 1, -1, null, false))
				{
					result = false;
				}
				else
				{
					if (x.def.rotatable)
					{
						float num = GenGeo.AngleDifferenceBetween(x.Rotation.AsAngle, (nearPawn.Position - x.Position).AngleFlat);
						if (num > 95.0)
						{
							result = false;
							goto IL_00e4;
						}
					}
					result = true;
				}
				goto IL_00e4;
				IL_00e4:
				return result;
			};
			return GenClosest.ClosestThingReachable(nearPawn.Position, nearPawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(forPawn, Danger.Deadly, TraverseMode.ByPawn, false), 2.2f, validator, null, 0, 5, false, RegionType.Set_Passable, false);
		}

		private static bool AboutToRecover(Pawn pawn)
		{
			bool result;
			if (pawn.Downed)
			{
				result = false;
			}
			else if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) && !HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				result = true;
			}
			else if (pawn.health.hediffSet.HasTendedImmunizableNotImmuneHediff())
			{
				result = false;
			}
			else
			{
				float num = 0f;
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && (hediff_Injury.CanHealFromTending() || hediff_Injury.CanHealNaturally() || hediff_Injury.Bleeding))
					{
						num += hediff_Injury.Severity;
					}
				}
				result = (num < 8.0 * pawn.RaceProps.baseHealthScale);
			}
			return result;
		}

		private static float VisitChanceScore(Pawn pawn, Pawn sick)
		{
			float num = GenMath.LerpDouble(-100f, 100f, 0.05f, 2f, (float)pawn.relations.OpinionOf(sick));
			float lengthHorizontal = (pawn.Position - sick.Position).LengthHorizontal;
			float num2 = Mathf.Clamp(GenMath.LerpDouble(0f, 150f, 1f, 0.2f, lengthHorizontal), 0.2f, 1f);
			return num * num2;
		}
	}
}
