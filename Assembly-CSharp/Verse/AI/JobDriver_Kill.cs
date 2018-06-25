using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3C RID: 2620
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x04002502 RID: 9474
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x06003A21 RID: 14881 RVA: 0x001EC140 File Offset: 0x001EA540
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x001EC178 File Offset: 0x001EA578
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
