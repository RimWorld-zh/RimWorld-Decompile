using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000065 RID: 101
	public class JobDriver_ClearSnow : JobDriver
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0001E91C File Offset: 0x0001CD1C
		private float TotalNeededWork
		{
			get
			{
				return 100f * base.Map.snowGrid.GetDepth(base.TargetLocA);
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001E950 File Offset: 0x0001CD50
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001E984 File Offset: 0x0001CD84
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil clearToil = new Toil();
			clearToil.tickAction = delegate()
			{
				Pawn actor = clearToil.actor;
				float statValue = actor.GetStatValue(StatDefOf.WorkSpeedGlobal, true);
				float num = statValue;
				this.workDone += num;
				if (this.workDone >= this.TotalNeededWork)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
					this.ReadyForNextToil();
				}
			};
			clearToil.defaultCompleteMode = ToilCompleteMode.Never;
			clearToil.WithEffect(EffecterDefOf.ClearSnow, TargetIndex.A);
			clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
			clearToil.WithProgressBar(TargetIndex.A, () => this.workDone / this.TotalNeededWork, true, -0.5f);
			clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return clearToil;
			yield break;
		}

		// Token: 0x04000205 RID: 517
		private float workDone = 0f;

		// Token: 0x04000206 RID: 518
		private const float ClearWorkPerSnowDepth = 100f;
	}
}
