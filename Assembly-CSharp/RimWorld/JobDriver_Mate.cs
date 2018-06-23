using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200002E RID: 46
	public class JobDriver_Mate : JobDriver
	{
		// Token: 0x040001AD RID: 429
		private const int MateDuration = 500;

		// Token: 0x040001AE RID: 430
		private const TargetIndex FemInd = TargetIndex.A;

		// Token: 0x040001AF RID: 431
		private const int TicksBetweenHeartMotes = 100;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000122B4 File Offset: 0x000106B4
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000122E4 File Offset: 0x000106E4
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000122FC File Offset: 0x000106FC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil prepare = Toils_General.WaitWith(TargetIndex.A, 500, false, false);
			prepare.tickAction = delegate()
			{
				if (this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
				if (this.Female.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.Female.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			};
			yield return prepare;
			yield return Toils_General.Do(delegate
			{
				PawnUtility.Mated(this.pawn, this.Female);
			});
			yield break;
		}
	}
}
