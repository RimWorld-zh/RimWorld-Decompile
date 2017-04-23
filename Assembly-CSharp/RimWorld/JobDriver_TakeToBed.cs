using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeToBed.<MakeNewToils>c__Iterator3E <MakeNewToils>c__Iterator3E = new JobDriver_TakeToBed.<MakeNewToils>c__Iterator3E();
			<MakeNewToils>c__Iterator3E.<>f__this = this;
			JobDriver_TakeToBed.<MakeNewToils>c__Iterator3E expr_0E = <MakeNewToils>c__Iterator3E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private void CheckMakeTakeePrisoner()
		{
			if (base.CurJob.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.released)
				{
					this.Takee.guest.released = false;
					this.Takee.guest.interactionMode = PrisonerInteractionMode.NoInteraction;
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					if (this.Takee.Faction != null)
					{
						this.Takee.Faction.Notify_MemberCaptured(this.Takee, this.pawn.Faction);
					}
					this.Takee.guest.SetGuestStatus(Faction.OfPlayer, true);
					if (this.Takee.guest.IsPrisoner)
					{
						TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
						{
							this.pawn,
							this.Takee
						});
						this.pawn.records.Increment(RecordDefOf.PeopleCaptured);
					}
				}
			}
		}

		private void CheckMakeTakeeGuest()
		{
			if (!base.CurJob.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null)
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, false);
			}
		}
	}
}
