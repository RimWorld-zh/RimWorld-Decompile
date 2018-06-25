using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3D RID: 2621
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x04002512 RID: 9490
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x06003A22 RID: 14882 RVA: 0x001EC46C File Offset: 0x001EA86C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x001EC4A4 File Offset: 0x001EA8A4
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
