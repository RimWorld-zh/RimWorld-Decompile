using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000063 RID: 99
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0001DECC File Offset: 0x0001C2CC
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0001DEFC File Offset: 0x0001C2FC
		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001DF2C File Offset: 0x0001C32C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null) && this.pawn.Reserve(this.DropPod, this.job, 1, -1, null);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001DF88 File Offset: 0x0001C388
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.DropPod.Accepts(this.Takee));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.DropPod.GetDirectlyHeldThings().Count > 0).FailOn(() => !this.Takee.Downed).FailOn(() => !this.pawn.CanReach(this.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
			Toil prepare = Toils_General.Wait(500);
			prepare.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return prepare;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.DropPod.TryAcceptThing(this.Takee, true);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001DFB4 File Offset: 0x0001C3B4
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				this.Takee
			};
		}

		// Token: 0x040001FF RID: 511
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04000200 RID: 512
		private const TargetIndex DropPodInd = TargetIndex.B;
	}
}
