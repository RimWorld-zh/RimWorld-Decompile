using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Toils_Ingest
	{
		private static List<IntVec3> spotSearchList = new List<IntVec3>();

		private static List<IntVec3> cardinals = GenAdj.CardinalDirections.ToList();

		private static List<IntVec3> diagonals = GenAdj.DiagonalDirections.ToList();

		public static Toil TakeMealFromDispenser(TargetIndex ind, Pawn eater)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = (Building_NutrientPasteDispenser)curJob.GetTarget(ind).Thing;
				Thing thing = building_NutrientPasteDispenser.TryDispenseFood();
				if (thing == null)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					actor.carryTracker.TryStartCarry(thing);
					actor.CurJob.SetTarget(ind, actor.carryTracker.CarriedThing);
				}
			};
			toil.FailOnCannotTouch(ind, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = Building_NutrientPasteDispenser.CollectDuration;
			return toil;
		}

		public static Toil PickupIngestible(TargetIndex ind, Pawn eater)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				if (curJob.count <= 0)
				{
					Log.Error("Tried to do PickupIngestible toil with job.maxNumToCarry = " + curJob.count);
					actor.jobs.EndCurrentJob(JobCondition.Errored, true);
				}
				else
				{
					int count = Mathf.Min(thing.stackCount, curJob.count);
					actor.carryTracker.TryStartCarry(thing, count, true);
					if (thing != actor.carryTracker.CarriedThing && actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
					{
						actor.Map.reservationManager.Release(thing, actor, curJob);
					}
					actor.jobs.curJob.targetA = actor.carryTracker.CarriedThing;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		public static Toil CarryIngestibleToChewSpot(Pawn pawn, TargetIndex ingestibleInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				IntVec3 intVec = IntVec3.Invalid;
				Thing thing = null;
				Thing thing2 = actor.CurJob.GetTarget(ingestibleInd).Thing;
				Predicate<Thing> baseChairValidator = delegate(Thing t)
				{
					if (t.def.building != null && t.def.building.isSittable)
					{
						if (t.IsForbidden(pawn))
						{
							return false;
						}
						if (!actor.CanReserve(t, 1, -1, null, false))
						{
							return false;
						}
						if (!t.IsSociallyProper(actor))
						{
							return false;
						}
						if (t.IsBurning())
						{
							return false;
						}
						if (t.HostileTo(pawn))
						{
							return false;
						}
						bool flag = false;
						for (int i = 0; i < 4; i++)
						{
							IntVec3 c = t.Position + GenAdj.CardinalDirections[i];
							Building edifice = c.GetEdifice(t.Map);
							if (edifice != null && edifice.def.surfaceType == SurfaceType.Eat)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return false;
						}
						return true;
					}
					return false;
				};
				if (thing2.def.ingestible.chairSearchRadius > 0.0)
				{
					thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false);
				}
				if (thing == null)
				{
					intVec = RCellFinder.SpotToChewStandingNear(actor, actor.CurJob.GetTarget(ingestibleInd).Thing);
					Danger chewSpotDanger = intVec.GetDangerFor(pawn, actor.Map);
					if (chewSpotDanger != Danger.None)
					{
						thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && (int)t.Position.GetDangerFor(pawn, t.Map) <= (int)chewSpotDanger, null, 0, -1, false, RegionType.Set_Passable, false);
					}
				}
				if (thing != null)
				{
					intVec = thing.Position;
					actor.Reserve(thing, actor.CurJob, 1, -1, null);
				}
				actor.Map.pawnDestinationReservationManager.Reserve(actor, actor.CurJob, intVec);
				actor.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		public static bool TryFindAdjacentIngestionPlaceSpot(IntVec3 root, ThingDef ingestibleDef, Pawn pawn, out IntVec3 placeSpot)
		{
			placeSpot = IntVec3.Invalid;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirections[i];
				if (intVec.HasEatSurface(pawn.Map) && !(from t in pawn.Map.thingGrid.ThingsAt(intVec)
				where t.def == ingestibleDef
				select t).Any() && !intVec.IsForbidden(pawn))
				{
					placeSpot = intVec;
					return true;
				}
			}
			if (!placeSpot.IsValid)
			{
				Toils_Ingest.spotSearchList.Clear();
				Toils_Ingest.cardinals.Shuffle();
				for (int j = 0; j < 4; j++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.cardinals[j]);
				}
				Toils_Ingest.diagonals.Shuffle();
				for (int k = 0; k < 4; k++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.diagonals[k]);
				}
				Toils_Ingest.spotSearchList.Add(IntVec3.Zero);
				for (int l = 0; l < Toils_Ingest.spotSearchList.Count; l++)
				{
					IntVec3 intVec2 = root + Toils_Ingest.spotSearchList[l];
					if (intVec2.Walkable(pawn.Map) && !intVec2.IsForbidden(pawn) && !(from t in pawn.Map.thingGrid.ThingsAt(intVec2)
					where t.def == ingestibleDef
					select t).Any())
					{
						placeSpot = intVec2;
						return true;
					}
				}
			}
			return false;
		}

		public static Toil FindAdjacentEatSurface(TargetIndex eatSurfaceInd, TargetIndex foodInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				IntVec3 position = actor.Position;
				Map map = actor.Map;
				int num = 0;
				IntVec3 intVec;
				while (true)
				{
					if (num < 4)
					{
						intVec = position + new Rot4(num).FacingCell;
						if (!intVec.HasEatSurface(map))
						{
							num++;
							continue;
						}
						break;
					}
					return;
				}
				toil.actor.CurJob.SetTarget(eatSurfaceInd, intVec);
				toil.actor.jobs.curDriver.rotateToFace = eatSurfaceInd;
				Thing thing = toil.actor.CurJob.GetTarget(foodInd).Thing;
				if (thing.def.rotatable)
				{
					thing.Rotation = Rot4.FromIntVec3(intVec - toil.actor.Position);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		public static Toil ChewIngestible(Pawn chewer, float durationMultiplier, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor2 = toil.actor;
				Thing thing4 = actor2.CurJob.GetTarget(ingestibleInd).Thing;
				if (!thing4.IngestibleNow)
				{
					chewer.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					actor2.jobs.curDriver.ticksLeftThisToil = Mathf.RoundToInt((float)thing4.def.ingestible.baseIngestTicks * durationMultiplier);
					if (thing4.Spawned)
					{
						thing4.Map.physicalInteractionReservationManager.Reserve(chewer, actor2.CurJob, thing4);
					}
				}
			};
			toil.tickAction = delegate
			{
				if (chewer != toil.actor)
				{
					toil.actor.rotationTracker.FaceCell(chewer.Position);
				}
				else
				{
					Thing thing3 = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
					if (thing3 != null && thing3.Spawned)
					{
						toil.actor.rotationTracker.FaceCell(thing3.Position);
					}
					else if (eatSurfaceInd != 0 && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
					{
						toil.actor.rotationTracker.FaceCell(toil.actor.CurJob.GetTarget(eatSurfaceInd).Cell);
					}
				}
				toil.actor.GainComfortFromCellIfPossible();
			};
			toil.WithProgressBar(ingestibleInd, delegate
			{
				Pawn actor = toil.actor;
				Thing thing2 = actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (thing2 == null)
				{
					return 1f;
				}
				return (float)(1.0 - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / Mathf.Round((float)thing2.def.ingestible.baseIngestTicks * durationMultiplier));
			}, false, -0.5f);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.FailOnDestroyedOrNull(ingestibleInd);
			toil.AddFinishAction(delegate
			{
				if (chewer != null && chewer.CurJob != null)
				{
					Thing thing = chewer.CurJob.GetTarget(ingestibleInd).Thing;
					if (thing != null && chewer.Map.physicalInteractionReservationManager.IsReservedBy(chewer, thing))
					{
						chewer.Map.physicalInteractionReservationManager.Release(chewer, toil.actor.CurJob, thing);
					}
				}
			});
			toil.handlingFacing = true;
			Toils_Ingest.AddIngestionEffects(toil, chewer, ingestibleInd, eatSurfaceInd);
			return toil;
		}

		public static Toil AddIngestionEffects(Toil toil, Pawn chewer, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd)
		{
			toil.WithEffect(delegate
			{
				LocalTargetInfo target2 = toil.actor.CurJob.GetTarget(ingestibleInd);
				if (!target2.HasThing)
				{
					return null;
				}
				EffecterDef result = target2.Thing.def.ingestible.ingestEffect;
				if ((int)chewer.RaceProps.intelligence < 1 && target2.Thing.def.ingestible.ingestEffectEat != null)
				{
					result = target2.Thing.def.ingestible.ingestEffectEat;
				}
				return result;
			}, delegate
			{
				if (!toil.actor.CurJob.GetTarget(ingestibleInd).HasThing)
				{
					return null;
				}
				Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (chewer != toil.actor)
				{
					return chewer;
				}
				if (eatSurfaceInd != 0 && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
				{
					return toil.actor.CurJob.GetTarget(eatSurfaceInd);
				}
				return thing;
			});
			toil.PlaySustainerOrSound(delegate
			{
				if (!chewer.RaceProps.Humanlike)
				{
					return null;
				}
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
				if (!target.HasThing)
				{
					return null;
				}
				return target.Thing.def.ingestible.ingestSound;
			});
			return toil;
		}

		public static Toil FinalizeIngest(Pawn ingester, TargetIndex ingestibleInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ingestibleInd).Thing;
				if (ingester.needs.mood != null && thing.def.IsNutritionGivingIngestible && thing.def.ingestible.chairSearchRadius > 10.0)
				{
					if (!(ingester.Position + ingester.Rotation.FacingCell).HasEatSurface(actor.Map) && ingester.GetPosture() == PawnPosture.Standing)
					{
						ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AteWithoutTable, null);
					}
					Room room = ingester.GetRoom(RegionType.Set_Passable);
					if (room != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
						if (ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
						{
							ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.AteInImpressiveDiningRoom, scoreStageIndex), null);
						}
					}
				}
				float num = ingester.needs.food.NutritionWanted;
				if (curJob.overeat)
				{
					num = Mathf.Max(num, 0.75f);
				}
				float num2 = thing.Ingested(ingester, num);
				if (!ingester.Dead)
				{
					ingester.needs.food.CurLevel += num2;
				}
				ingester.records.AddTo(RecordDefOf.NutritionEaten, num2);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
