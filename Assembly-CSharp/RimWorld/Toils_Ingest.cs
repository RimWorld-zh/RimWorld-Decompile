using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Toils_Ingest
	{
		private static List<IntVec3> spotSearchList = new List<IntVec3>();

		private static List<IntVec3> cardinals = GenAdj.CardinalDirections.ToList<IntVec3>();

		private static List<IntVec3> diagonals = GenAdj.DiagonalDirections.ToList<IntVec3>();

		public Toils_Ingest()
		{
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Toils_Ingest()
		{
		}

		[CompilerGenerated]
		private sealed class <TakeMealFromDispenser>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <TakeMealFromDispenser>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = (Building_NutrientPasteDispenser)curJob.GetTarget(this.ind).Thing;
				Thing thing = building_NutrientPasteDispenser.TryDispenseFood();
				if (thing == null)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					actor.carryTracker.TryStartCarry(thing);
					actor.CurJob.SetTarget(this.ind, actor.carryTracker.CarriedThing);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PickupIngestible>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <PickupIngestible>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(this.ind).Thing;
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
			}
		}

		[CompilerGenerated]
		private sealed class <CarryIngestibleToChewSpot>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ingestibleInd;

			internal Pawn pawn;

			public <CarryIngestibleToChewSpot>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				IntVec3 intVec = IntVec3.Invalid;
				Thing thing = null;
				Thing thing2 = actor.CurJob.GetTarget(this.ingestibleInd).Thing;
				Predicate<Thing> baseChairValidator = delegate(Thing t)
				{
					bool result;
					if (t.def.building == null || !t.def.building.isSittable)
					{
						result = false;
					}
					else if (t.IsForbidden(this.pawn))
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
					else if (t.HostileTo(this.pawn))
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
					thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(this.pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false);
				}
				if (thing == null)
				{
					intVec = RCellFinder.SpotToChewStandingNear(actor, actor.CurJob.GetTarget(this.ingestibleInd).Thing);
					Danger chewSpotDanger = intVec.GetDangerFor(this.pawn, actor.Map);
					if (chewSpotDanger != Danger.None)
					{
						thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(this.pawn, t.Map) <= chewSpotDanger, null, 0, -1, false, RegionType.Set_Passable, false);
					}
				}
				if (thing != null)
				{
					intVec = thing.Position;
					actor.Reserve(thing, actor.CurJob, 1, -1, null);
				}
				actor.Map.pawnDestinationReservationManager.Reserve(actor, actor.CurJob, intVec);
				actor.pather.StartPath(intVec, PathEndMode.OnCell);
			}

			private sealed class <CarryIngestibleToChewSpot>c__AnonStorey3
			{
				internal Pawn actor;

				internal Predicate<Thing> baseChairValidator;

				internal Toils_Ingest.<CarryIngestibleToChewSpot>c__AnonStorey2 <>f__ref$2;

				public <CarryIngestibleToChewSpot>c__AnonStorey3()
				{
				}

				internal bool <>m__0(Thing t)
				{
					bool result;
					if (t.def.building == null || !t.def.building.isSittable)
					{
						result = false;
					}
					else if (t.IsForbidden(this.<>f__ref$2.pawn))
					{
						result = false;
					}
					else if (!this.actor.CanReserve(t, 1, -1, null, false))
					{
						result = false;
					}
					else if (!t.IsSociallyProper(this.actor))
					{
						result = false;
					}
					else if (t.IsBurning())
					{
						result = false;
					}
					else if (t.HostileTo(this.<>f__ref$2.pawn))
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
				}

				internal bool <>m__1(Thing t)
				{
					return this.baseChairValidator(t) && t.Position.GetDangerFor(this.<>f__ref$2.pawn, t.Map) == Danger.None;
				}
			}

			private sealed class <CarryIngestibleToChewSpot>c__AnonStorey4
			{
				internal Danger chewSpotDanger;

				internal Toils_Ingest.<CarryIngestibleToChewSpot>c__AnonStorey2 <>f__ref$2;

				internal Toils_Ingest.<CarryIngestibleToChewSpot>c__AnonStorey2.<CarryIngestibleToChewSpot>c__AnonStorey3 <>f__ref$3;

				public <CarryIngestibleToChewSpot>c__AnonStorey4()
				{
				}

				internal bool <>m__0(Thing t)
				{
					return this.<>f__ref$3.baseChairValidator(t) && t.Position.GetDangerFor(this.<>f__ref$2.pawn, t.Map) <= this.chewSpotDanger;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindAdjacentIngestionPlaceSpot>c__AnonStorey5
		{
			internal ThingDef ingestibleDef;

			public <TryFindAdjacentIngestionPlaceSpot>c__AnonStorey5()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return t.def == this.ingestibleDef;
			}

			internal bool <>m__1(Thing t)
			{
				return t.def == this.ingestibleDef;
			}
		}

		[CompilerGenerated]
		private sealed class <FindAdjacentEatSurface>c__AnonStorey6
		{
			internal Toil toil;

			internal TargetIndex eatSurfaceInd;

			internal TargetIndex foodInd;

			public <FindAdjacentEatSurface>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				IntVec3 position = actor.Position;
				Map map = actor.Map;
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					IntVec3 intVec = position + rot.FacingCell;
					if (intVec.HasEatSurface(map))
					{
						this.toil.actor.CurJob.SetTarget(this.eatSurfaceInd, intVec);
						this.toil.actor.jobs.curDriver.rotateToFace = this.eatSurfaceInd;
						Thing thing = this.toil.actor.CurJob.GetTarget(this.foodInd).Thing;
						if (thing.def.rotatable)
						{
							thing.Rotation = Rot4.FromIntVec3(intVec - this.toil.actor.Position);
						}
						break;
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ChewIngestible>c__AnonStorey7
		{
			internal Toil toil;

			internal TargetIndex ingestibleInd;

			internal Pawn chewer;

			internal float durationMultiplier;

			internal TargetIndex eatSurfaceInd;

			public <ChewIngestible>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Thing thing = actor.CurJob.GetTarget(this.ingestibleInd).Thing;
				if (!thing.IngestibleNow)
				{
					this.chewer.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					actor.jobs.curDriver.ticksLeftThisToil = Mathf.RoundToInt((float)thing.def.ingestible.baseIngestTicks * this.durationMultiplier);
					if (thing.Spawned)
					{
						thing.Map.physicalInteractionReservationManager.Reserve(this.chewer, actor.CurJob, thing);
					}
				}
			}

			internal void <>m__1()
			{
				if (this.chewer != this.toil.actor)
				{
					this.toil.actor.rotationTracker.FaceCell(this.chewer.Position);
				}
				else
				{
					Thing thing = this.toil.actor.CurJob.GetTarget(this.ingestibleInd).Thing;
					if (thing != null && thing.Spawned)
					{
						this.toil.actor.rotationTracker.FaceCell(thing.Position);
					}
					else if (this.eatSurfaceInd != TargetIndex.None && this.toil.actor.CurJob.GetTarget(this.eatSurfaceInd).IsValid)
					{
						this.toil.actor.rotationTracker.FaceCell(this.toil.actor.CurJob.GetTarget(this.eatSurfaceInd).Cell);
					}
				}
				this.toil.actor.GainComfortFromCellIfPossible();
			}

			internal float <>m__2()
			{
				Pawn actor = this.toil.actor;
				Thing thing = actor.CurJob.GetTarget(this.ingestibleInd).Thing;
				float result;
				if (thing == null)
				{
					result = 1f;
				}
				else
				{
					result = 1f - (float)this.toil.actor.jobs.curDriver.ticksLeftThisToil / Mathf.Round((float)thing.def.ingestible.baseIngestTicks * this.durationMultiplier);
				}
				return result;
			}

			internal void <>m__3()
			{
				if (this.chewer != null)
				{
					if (this.chewer.CurJob != null)
					{
						Thing thing = this.chewer.CurJob.GetTarget(this.ingestibleInd).Thing;
						if (thing != null)
						{
							if (this.chewer.Map.physicalInteractionReservationManager.IsReservedBy(this.chewer, thing))
							{
								this.chewer.Map.physicalInteractionReservationManager.Release(this.chewer, this.toil.actor.CurJob, thing);
							}
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <AddIngestionEffects>c__AnonStorey8
		{
			internal Toil toil;

			internal TargetIndex ingestibleInd;

			internal Pawn chewer;

			internal TargetIndex eatSurfaceInd;

			public <AddIngestionEffects>c__AnonStorey8()
			{
			}

			internal EffecterDef <>m__0()
			{
				LocalTargetInfo target = this.toil.actor.CurJob.GetTarget(this.ingestibleInd);
				EffecterDef result;
				if (!target.HasThing)
				{
					result = null;
				}
				else
				{
					EffecterDef effecterDef = target.Thing.def.ingestible.ingestEffect;
					if (this.chewer.RaceProps.intelligence < Intelligence.ToolUser && target.Thing.def.ingestible.ingestEffectEat != null)
					{
						effecterDef = target.Thing.def.ingestible.ingestEffectEat;
					}
					result = effecterDef;
				}
				return result;
			}

			internal LocalTargetInfo <>m__1()
			{
				LocalTargetInfo result;
				if (!this.toil.actor.CurJob.GetTarget(this.ingestibleInd).HasThing)
				{
					result = null;
				}
				else
				{
					Thing thing = this.toil.actor.CurJob.GetTarget(this.ingestibleInd).Thing;
					if (this.chewer != this.toil.actor)
					{
						result = this.chewer;
					}
					else if (this.eatSurfaceInd != TargetIndex.None && this.toil.actor.CurJob.GetTarget(this.eatSurfaceInd).IsValid)
					{
						result = this.toil.actor.CurJob.GetTarget(this.eatSurfaceInd);
					}
					else
					{
						result = thing;
					}
				}
				return result;
			}

			internal SoundDef <>m__2()
			{
				SoundDef result;
				if (!this.chewer.RaceProps.Humanlike)
				{
					result = null;
				}
				else
				{
					LocalTargetInfo target = this.toil.actor.CurJob.GetTarget(this.ingestibleInd);
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
			}
		}

		[CompilerGenerated]
		private sealed class <FinalizeIngest>c__AnonStorey9
		{
			internal Toil toil;

			internal TargetIndex ingestibleInd;

			internal Pawn ingester;

			public <FinalizeIngest>c__AnonStorey9()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(this.ingestibleInd).Thing;
				if (this.ingester.needs.mood != null)
				{
					if (thing.def.IsNutritionGivingIngestible && thing.def.ingestible.chairSearchRadius > 10f)
					{
						if (!(this.ingester.Position + this.ingester.Rotation.FacingCell).HasEatSurface(actor.Map) && this.ingester.GetPosture() == PawnPosture.Standing)
						{
							this.ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AteWithoutTable, null);
						}
						Room room = this.ingester.GetRoom(RegionType.Set_Passable);
						if (room != null)
						{
							int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
							if (ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
							{
								this.ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.AteInImpressiveDiningRoom, scoreStageIndex), null);
							}
						}
					}
				}
				float num = this.ingester.needs.food.NutritionWanted;
				if (curJob.overeat)
				{
					num = Mathf.Max(num, 0.75f);
				}
				float num2 = thing.Ingested(this.ingester, num);
				if (!this.ingester.Dead)
				{
					this.ingester.needs.food.CurLevel += num2;
				}
				this.ingester.records.AddTo(RecordDefOf.NutritionEaten, num2);
			}
		}
	}
}
