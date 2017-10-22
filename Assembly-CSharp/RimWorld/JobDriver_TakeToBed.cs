using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TakeToBed : JobDriver
	{
		private const TargetIndex TakeeIndex = TargetIndex.A;

		private const TargetIndex BedIndex = TargetIndex.B;

		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Takee, base.job, 1, -1, null) && base.pawn.Reserve((Thing)this.DropBed, base.job, this.DropBed.SleepingSlotsCount, 0, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			this.FailOn((Func<bool>)delegate
			{
				bool result;
				if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006b: stateMachine*/)._0024this.job.def.makeTargetPrisoner)
				{
					if (!((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006b: stateMachine*/)._0024this.DropBed.ForPrisoners)
					{
						result = true;
						goto IL_0073;
					}
				}
				else if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006b: stateMachine*/)._0024this.DropBed.ForPrisoners != ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006b: stateMachine*/)._0024this.Takee.IsPrisoner)
				{
					result = true;
					goto IL_0073;
				}
				result = false;
				goto IL_0073;
				IL_0073:
				return result;
			});
			yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void CheckMakeTakeePrisoner()
		{
			if (base.job.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.Released)
				{
					this.Takee.guest.Released = false;
					this.Takee.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					if (this.Takee.Faction != null)
					{
						this.Takee.Faction.Notify_MemberCaptured(this.Takee, base.pawn.Faction);
					}
					this.Takee.guest.SetGuestStatus(Faction.OfPlayer, true);
					if (this.Takee.guest.IsPrisoner)
					{
						TaleRecorder.RecordTale(TaleDefOf.Captured, base.pawn, this.Takee);
						base.pawn.records.Increment(RecordDefOf.PeopleCaptured);
					}
				}
			}
		}

		private void CheckMakeTakeeGuest()
		{
			if (!base.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, false);
			}
		}
	}
}
