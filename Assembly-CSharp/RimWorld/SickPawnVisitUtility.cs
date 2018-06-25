using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
			Pawn pawn2;
			Pawn result;
			if (!source.TryRandomElementByWeight((Pawn x) => SickPawnVisitUtility.VisitChanceScore(pawn, x), out pawn2))
			{
				result = null;
			}
			else
			{
				result = pawn2;
			}
			return result;
		}

		public static bool CanVisit(Pawn pawn, Pawn sick, JoyCategory maxPatientJoy)
		{
			return sick.IsColonist && !sick.Dead && pawn != sick && sick.InBed() && sick.Awake() && !sick.IsForbidden(pawn) && sick.needs.joy != null && sick.needs.joy.CurCategory <= maxPatientJoy && InteractionUtility.CanReceiveInteraction(sick) && !sick.needs.food.Starving && sick.needs.rest.CurLevel > 0.33f && pawn.CanReserveAndReach(sick, PathEndMode.InteractionCell, Danger.None, 1, -1, null, false) && !SickPawnVisitUtility.AboutToRecover(sick);
		}

		public static Thing FindChair(Pawn forPawn, Pawn nearPawn)
		{
			Predicate<Thing> validator = delegate(Thing x)
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
						if (num > 95f)
						{
							return false;
						}
					}
					result = true;
				}
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
				result = (num < 8f * pawn.RaceProps.baseHealthScale);
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

		[CompilerGenerated]
		private sealed class <FindRandomSickPawn>c__AnonStorey0
		{
			internal Pawn pawn;

			internal JoyCategory maxPatientJoy;

			public <FindRandomSickPawn>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return SickPawnVisitUtility.CanVisit(this.pawn, x, this.maxPatientJoy);
			}

			internal float <>m__1(Pawn x)
			{
				return SickPawnVisitUtility.VisitChanceScore(this.pawn, x);
			}
		}

		[CompilerGenerated]
		private sealed class <FindChair>c__AnonStorey1
		{
			internal Pawn forPawn;

			internal Pawn nearPawn;

			public <FindChair>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing x)
			{
				bool result;
				if (!x.def.building.isSittable)
				{
					result = false;
				}
				else if (x.IsForbidden(this.forPawn))
				{
					result = false;
				}
				else if (!GenSight.LineOfSight(x.Position, this.nearPawn.Position, this.nearPawn.Map, false, null, 0, 0))
				{
					result = false;
				}
				else if (!this.forPawn.CanReserve(x, 1, -1, null, false))
				{
					result = false;
				}
				else
				{
					if (x.def.rotatable)
					{
						float num = GenGeo.AngleDifferenceBetween(x.Rotation.AsAngle, (this.nearPawn.Position - x.Position).AngleFlat);
						if (num > 95f)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}
	}
}
