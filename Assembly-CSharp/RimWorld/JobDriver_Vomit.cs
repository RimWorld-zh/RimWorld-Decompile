using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000088 RID: 136
	public class JobDriver_Vomit : JobDriver
	{
		// Token: 0x06000384 RID: 900 RVA: 0x0002778B File Offset: 0x00025B8B
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0002778E File Offset: 0x00025B8E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000277AC File Offset: 0x00025BAC
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000277C4 File Offset: 0x00025BC4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil to = new Toil();
			to.initAction = delegate()
			{
				this.ticksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						break;
					}
					if (c.InBounds(this.pawn.Map) && c.Standable(this.pawn.Map))
					{
						goto IL_A5;
					}
				}
				c = this.pawn.Position;
				IL_A5:
				this.job.targetA = c;
				this.pawn.pather.StopDead();
			};
			to.tickAction = delegate()
			{
				if (this.ticksLeft % 150 == 149)
				{
					FilthMaker.MakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_Vomit, this.pawn.LabelIndefinite(), 1);
					if (this.pawn.needs.food.CurLevelPercentage > 0.1f)
					{
						this.pawn.needs.food.CurLevel -= this.pawn.needs.food.MaxLevel * 0.04f;
					}
				}
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
					{
						this.pawn
					});
				}
			};
			to.defaultCompleteMode = ToilCompleteMode.Never;
			to.WithEffect(EffecterDefOf.Vomit, TargetIndex.A);
			to.PlaySustainerOrSound(() => SoundDefOf.Vomit);
			yield return to;
			yield break;
		}

		// Token: 0x04000249 RID: 585
		private int ticksLeft;
	}
}
