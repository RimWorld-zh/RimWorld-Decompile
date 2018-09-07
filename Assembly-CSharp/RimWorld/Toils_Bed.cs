using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_Bed
	{
		public static Toil GotoBed(TargetIndex bedIndex)
		{
			Toil gotoBed = new Toil();
			gotoBed.initAction = delegate()
			{
				Pawn actor = gotoBed.actor;
				Building_Bed bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(actor, bed);
				if (actor.Position == bedSleepingSlotPosFor)
				{
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					actor.pather.StartPath(bedSleepingSlotPosFor, PathEndMode.OnCell);
				}
			};
			gotoBed.tickAction = delegate()
			{
				Pawn actor = gotoBed.actor;
				Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn curOccupantAt = building_Bed.GetCurOccupantAt(actor.pather.Destination.Cell);
				if (curOccupantAt != null && curOccupantAt != actor)
				{
					actor.pather.StartPath(RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed), PathEndMode.OnCell);
				}
			};
			gotoBed.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoBed.FailOnBedNoLongerUsable(bedIndex);
			return gotoBed;
		}

		public static Toil ClaimBedIfNonMedical(TargetIndex ind, TargetIndex claimantIndex = TargetIndex.None)
		{
			Toil claim = new Toil();
			claim.initAction = delegate()
			{
				Pawn actor = claim.GetActor();
				Pawn pawn = (claimantIndex != TargetIndex.None) ? ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing) : actor;
				if (pawn.ownership != null)
				{
					pawn.ownership.ClaimBedIfNonMedical((Building_Bed)actor.CurJob.GetTarget(ind).Thing);
				}
			};
			claim.FailOnDespawnedOrNull(ind);
			return claim;
		}

		public static T FailOnNonMedicalBedNotOwned<T>(this T f, TargetIndex bedIndex, TargetIndex claimantIndex = TargetIndex.None) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Pawn pawn = (claimantIndex != TargetIndex.None) ? ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing) : actor;
				if (pawn.ownership != null)
				{
					Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
					if (building_Bed.Medical)
					{
						if ((!pawn.InBed() || pawn.CurrentBed() != building_Bed) && !building_Bed.AnyUnoccupiedSleepingSlot)
						{
							return JobCondition.Incompletable;
						}
					}
					else
					{
						if (!building_Bed.owners.Contains(pawn))
						{
							return JobCondition.Incompletable;
						}
						if (pawn.InBed() && pawn.CurrentBed() == building_Bed)
						{
							int curOccupantSlotIndex = building_Bed.GetCurOccupantSlotIndex(pawn);
							if (curOccupantSlotIndex >= building_Bed.owners.Count || building_Bed.owners[curOccupantSlotIndex] != pawn)
							{
								return JobCondition.Incompletable;
							}
						}
					}
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex)
		{
			toil.FailOnDespawnedOrNull(bedIndex);
			toil.FailOn(() => ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).IsBurning());
			toil.FailOn(() => ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).ForPrisoners != toil.actor.IsPrisoner);
			toil.FailOnNonMedicalBedNotOwned(bedIndex, TargetIndex.None);
			toil.FailOn(() => !HealthAIUtility.ShouldSeekMedicalRest(toil.actor) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(toil.actor) && ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).Medical);
			toil.FailOn(() => toil.actor.IsColonist && !toil.actor.CurJob.ignoreForbidden && !toil.actor.Downed && toil.actor.CurJob.GetTarget(bedIndex).Thing.IsForbidden(toil.actor));
		}

		[CompilerGenerated]
		private sealed class <GotoBed>c__AnonStorey0
		{
			internal Toil gotoBed;

			internal TargetIndex bedIndex;

			public <GotoBed>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.gotoBed.actor;
				Building_Bed bed = (Building_Bed)actor.CurJob.GetTarget(this.bedIndex).Thing;
				IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(actor, bed);
				if (actor.Position == bedSleepingSlotPosFor)
				{
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					actor.pather.StartPath(bedSleepingSlotPosFor, PathEndMode.OnCell);
				}
			}

			internal void <>m__1()
			{
				Pawn actor = this.gotoBed.actor;
				Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(this.bedIndex).Thing;
				Pawn curOccupantAt = building_Bed.GetCurOccupantAt(actor.pather.Destination.Cell);
				if (curOccupantAt != null && curOccupantAt != actor)
				{
					actor.pather.StartPath(RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed), PathEndMode.OnCell);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ClaimBedIfNonMedical>c__AnonStorey1
		{
			internal Toil claim;

			internal TargetIndex claimantIndex;

			internal TargetIndex ind;

			public <ClaimBedIfNonMedical>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.claim.GetActor();
				Pawn pawn = (this.claimantIndex != TargetIndex.None) ? ((Pawn)actor.CurJob.GetTarget(this.claimantIndex).Thing) : actor;
				if (pawn.ownership != null)
				{
					pawn.ownership.ClaimBedIfNonMedical((Building_Bed)actor.CurJob.GetTarget(this.ind).Thing);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <FailOnNonMedicalBedNotOwned>c__AnonStorey2<T> where T : IJobEndable
		{
			internal T f;

			internal TargetIndex claimantIndex;

			internal TargetIndex bedIndex;

			public <FailOnNonMedicalBedNotOwned>c__AnonStorey2()
			{
			}

			internal JobCondition <>m__0()
			{
				Pawn actor = this.f.GetActor();
				Pawn pawn = (this.claimantIndex != TargetIndex.None) ? ((Pawn)actor.CurJob.GetTarget(this.claimantIndex).Thing) : actor;
				if (pawn.ownership != null)
				{
					Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(this.bedIndex).Thing;
					if (building_Bed.Medical)
					{
						if ((!pawn.InBed() || pawn.CurrentBed() != building_Bed) && !building_Bed.AnyUnoccupiedSleepingSlot)
						{
							return JobCondition.Incompletable;
						}
					}
					else
					{
						if (!building_Bed.owners.Contains(pawn))
						{
							return JobCondition.Incompletable;
						}
						if (pawn.InBed() && pawn.CurrentBed() == building_Bed)
						{
							int curOccupantSlotIndex = building_Bed.GetCurOccupantSlotIndex(pawn);
							if (curOccupantSlotIndex >= building_Bed.owners.Count || building_Bed.owners[curOccupantSlotIndex] != pawn)
							{
								return JobCondition.Incompletable;
							}
						}
					}
				}
				return JobCondition.Ongoing;
			}
		}

		[CompilerGenerated]
		private sealed class <FailOnBedNoLongerUsable>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex bedIndex;

			public <FailOnBedNoLongerUsable>c__AnonStorey3()
			{
			}

			internal bool <>m__0()
			{
				return ((Building_Bed)this.toil.actor.CurJob.GetTarget(this.bedIndex).Thing).IsBurning();
			}

			internal bool <>m__1()
			{
				return ((Building_Bed)this.toil.actor.CurJob.GetTarget(this.bedIndex).Thing).ForPrisoners != this.toil.actor.IsPrisoner;
			}

			internal bool <>m__2()
			{
				return !HealthAIUtility.ShouldSeekMedicalRest(this.toil.actor) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(this.toil.actor) && ((Building_Bed)this.toil.actor.CurJob.GetTarget(this.bedIndex).Thing).Medical;
			}

			internal bool <>m__3()
			{
				return this.toil.actor.IsColonist && !this.toil.actor.CurJob.ignoreForbidden && !this.toil.actor.Downed && this.toil.actor.CurJob.GetTarget(this.bedIndex).Thing.IsForbidden(this.toil.actor);
			}
		}
	}
}
