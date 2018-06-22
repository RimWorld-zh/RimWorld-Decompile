using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200004C RID: 76
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00019644 File Offset: 0x00017A44
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00019670 File Offset: 0x00017A70
		public override string GetReport()
		{
			string result;
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				result = "ReportExtinguishingFireOn".Translate(new object[]
				{
					this.TargetFire.parent.LabelCap
				});
			}
			else
			{
				result = "ReportExtinguishingFire".Translate();
			}
			return result;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000196D4 File Offset: 0x00017AD4
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000196EC File Offset: 0x00017AEC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 150
			};
			Toil killFire = new Toil();
			killFire.initAction = delegate()
			{
				this.TargetFire.Destroy(DestroyMode.Vanish);
				this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
			};
			killFire.FailOnDestroyedOrNull(TargetIndex.A);
			killFire.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return killFire;
			yield break;
		}
	}
}
