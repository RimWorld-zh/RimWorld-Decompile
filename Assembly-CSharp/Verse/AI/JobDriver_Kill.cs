using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3A RID: 2618
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x04002501 RID: 9473
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x06003A1D RID: 14877 RVA: 0x001EC014 File Offset: 0x001EA414
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x001EC04C File Offset: 0x001EA44C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Succeeded);
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, false, 0.95f);
			yield return gotoCastPos;
			Toil jumpIfCannotHit = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return jumpIfCannotHit;
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield return Toils_Jump.Jump(jumpIfCannotHit);
			yield break;
		}
	}
}
