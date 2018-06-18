using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3E RID: 2622
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x06003A23 RID: 14883 RVA: 0x001EBDD4 File Offset: 0x001EA1D4
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x001EBE0C File Offset: 0x001EA20C
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

		// Token: 0x04002506 RID: 9478
		private const TargetIndex VictimInd = TargetIndex.A;
	}
}
