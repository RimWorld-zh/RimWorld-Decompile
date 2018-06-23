using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200009B RID: 155
	public class Toils_Ingest
	{
		// Token: 0x04000262 RID: 610
		private static List<IntVec3> spotSearchList = new List<IntVec3>();

		// Token: 0x04000263 RID: 611
		private static List<IntVec3> cardinals = GenAdj.CardinalDirections.ToList<IntVec3>();

		// Token: 0x04000264 RID: 612
		private static List<IntVec3> diagonals = GenAdj.DiagonalDirections.ToList<IntVec3>();

		// Token: 0x060003E8 RID: 1000 RVA: 0x0002D114 File Offset: 0x0002B514
		public static Toil TakeMealFromDispenser(TargetIndex ind, Pawn eater)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
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

		// Token: 0x060003E9 RID: 1001 RVA: 0x0002D190 File Offset: 0x0002B590
		public static Toil PickupIngestible(TargetIndex ind, Pawn eater)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				if (curJob.count <= 0)
				{
					Log.Error("Tried to do PickupIngestible toil with job.maxNumToCarry = " + curJob.count, false);
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

		// Token: 0x060003EA RID: 1002 RVA: 0x0002D1E8 File Offset: 0x0002B5E8
		public static Toil CarryIngestibleToChewSpot(Pawn pawn, TargetIndex ingestibleInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IntVec3 intVec = IntVec3.Invalid;
				Thing thing = null;
				Thing thing2 = actor.CurJob.GetTarget(ingestibleInd).Thing;
				Predicate<Thing> baseChairValidator = delegate(Thing t)
				{
					bool result;
					if (t.def.building == null || !t.def.building.isSittable)
					{
						result = false;
					}
					else if (t.IsForbidden(pawn))
					{
						result = false;
					}
					else if (!actor.CanReserve(t, 1, -1, null, false))
					{
						result = false;
					}
					else if (!t.IsSociallyProper(actor))
					{
						result = false;
					}
					else if (t.IsBurning())
					{
						result = false;
					}
					else if (t.HostileTo(pawn))
					{
						result = false;
					}
					else
					{
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
						result = flag;
					}
					return result;
				};
				if (thing2.def.ingestible.chairSearchRadius > 0f)
				{
					thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false);
				}
				if (thing == null)
				{
					intVec = RCellFinder.SpotToChewStandingNear(actor, actor.CurJob.GetTarget(ingestibleInd).Thing);
					Danger chewSpotDanger = intVec.GetDangerFor(pawn, actor.Map);
					if (chewSpotDanger != Danger.None)
					{
						thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) <= chewSpotDanger, null, 0, -1, false, RegionType.Set_Passable, false);
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

		// Token: 0x060003EB RID: 1003 RVA: 0x0002D248 File Offset: 0x0002B648
		public static bool TryFindAdjacentIngestionPlaceSpot(IntVec3 root, ThingDef ingestibleDef, Pawn pawn, out IntVec3 placeSpot)
		{
			placeSpot = IntVec3.Invalid;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirections[i];
				if (intVec.HasEatSurface(pawn.Map))
				{
					if (!(from t in pawn.Map.thingGrid.ThingsAt(intVec)
					where t.def == ingestibleDef
					select t).Any<Thing>())
					{
						if (!intVec.IsForbidden(pawn))
						{
							placeSpot = intVec;
							return true;
						}
					}
				}
			}
			if (!placeSpot.IsValid)
			{
				Toils_Ingest.spotSearchList.Clear();
				Toils_Ingest.cardinals.Shuffle<IntVec3>();
				for (int j = 0; j < 4; j++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.cardinals[j]);
				}
				Toils_Ingest.diagonals.Shuffle<IntVec3>();
				for (int k = 0; k < 4; k++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.diagonals[k]);
				}
				Toils_Ingest.spotSearchList.Add(IntVec3.Zero);
				for (int l = 0; l < Toils_Ingest.spotSearchList.Count; l++)
				{
					IntVec3 intVec2 = root + Toils_Ingest.spotSearchList[l];
					if (intVec2.Walkable(pawn.Map) && !intVec2.IsForbidden(pawn))
					{
						if (!(from t in pawn.Map.thingGrid.ThingsAt(intVec2)
						where t.def == ingestibleDef
						select t).Any<Thing>())
						{
							placeSpot = intVec2;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0002D438 File Offset: 0x0002B838
		public static Toil FindAdjacentEatSurface(TargetIndex eatSurfaceInd, TargetIndex foodInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IntVec3 position = actor.Position;
				Map map = actor.Map;
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					IntVec3 intVec = position + rot.FacingCell;
					if (intVec.HasEatSurface(map))
					{
						toil.actor.CurJob.SetTarget(eatSurfaceInd, intVec);
						toil.actor.jobs.curDriver.rotateToFace = eatSurfaceInd;
						Thing thing = toil.actor.CurJob.GetTarget(foodInd).Thing;
						if (thing.def.rotatable)
						{
							thing.Rotation = Rot4.FromIntVec3(intVec - toil.actor.Position);
						}
						break;
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0002D498 File Offset: 0x0002B898
		public static Toil ChewIngestible(Pawn chewer, float durationMultiplier, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (!thing.IngestibleNow)
				{
					chewer.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					actor.jobs.curDriver.ticksLeftThisToil = Mathf.RoundToInt((float)thing.def.ingestible.baseIngestTicks * durationMultiplier);
					if (thing.Spawned)
					{
						thing.Map.physicalInteractionReservationManager.Reserve(chewer, actor.CurJob, thing);
					}
				}
			};
			toil.tickAction = delegate()
			{
				if (chewer != toil.actor)
				{
					toil.actor.rotationTracker.FaceCell(chewer.Position);
				}
				else
				{
					Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
					if (thing != null && thing.Spawned)
					{
						toil.actor.rotationTracker.FaceCell(thing.Position);
					}
					else if (eatSurfaceInd != TargetIndex.None && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
					{
						toil.actor.rotationTracker.FaceCell(toil.actor.CurJob.GetTarget(eatSurfaceInd).Cell);
					}
				}
				toil.actor.GainComfortFromCellIfPossible();
			};
			toil.WithProgressBar(ingestibleInd, delegate
			{
				Pawn actor = toil.actor;
				Thing thing = actor.CurJob.GetTarget(ingestibleInd).Thing;
				float result;
				if (thing == null)
				{
					result = 1f;
				}
				else
				{
					result = 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / Mathf.Round((float)thing.def.ingestible.baseIngestTicks * durationMultiplier);
				}
				return result;
			}, false, -0.5f);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.FailOnDestroyedOrNull(ingestibleInd);
			toil.AddFinishAction(delegate
			{
				if (chewer != null)
				{
					if (chewer.CurJob != null)
					{
						Thing thing = chewer.CurJob.GetTarget(ingestibleInd).Thing;
						if (thing != null)
						{
							if (chewer.Map.physicalInteractionReservationManager.IsReservedBy(chewer, thing))
							{
								chewer.Map.physicalInteractionReservationManager.Release(chewer, toil.actor.CurJob, thing);
							}
						}
					}
				}
			});
			toil.handlingFacing = true;
			Toils_Ingest.AddIngestionEffects(toil, chewer, ingestibleInd, eatSurfaceInd);
			return toil;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0002D594 File Offset: 0x0002B994
		public static Toil AddIngestionEffects(Toil toil, Pawn chewer, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd)
		{
			toil.WithEffect(delegate()
			{
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
				EffecterDef result;
				if (!target.HasThing)
				{
					result = null;
				}
				else
				{
					EffecterDef effecterDef = target.Thing.def.ingestible.ingestEffect;
					if (chewer.RaceProps.intelligence < Intelligence.ToolUser && target.Thing.def.ingestible.ingestEffectEat != null)
					{
						effecterDef = target.Thing.def.ingestible.ingestEffectEat;
					}
					result = effecterDef;
				}
				return result;
			}, delegate()
			{
				LocalTargetInfo result;
				if (!toil.actor.CurJob.GetTarget(ingestibleInd).HasThing)
				{
					result = null;
				}
				else
				{
					Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
					if (chewer != toil.actor)
					{
						result = chewer;
					}
					else if (eatSurfaceInd != TargetIndex.None && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
					{
						result = toil.actor.CurJob.GetTarget(eatSurfaceInd);
					}
					else
					{
						result = thing;
					}
				}
				return result;
			});
			toil.PlaySustainerOrSound(delegate()
			{
				SoundDef result;
				if (!chewer.RaceProps.Humanlike)
				{
					result = null;
				}
				else
				{
					LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
					if (!target.HasThing)
					{
						result = null;
					}
					else
					{
						result = target.Thing.def.ingestible.ingestSound;
					}
				}
				return result;
			});
			return toil;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0002D610 File Offset: 0x0002BA10
		public static Toil FinalizeIngest(Pawn ingester, TargetIndex ingestibleInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ingestibleInd).Thing;
				if (ingester.needs.mood != null)
				{
					if (thing.def.IsNutritionGivingIngestible && thing.def.ingestible.chairSearchRadius > 10f)
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
