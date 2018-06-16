using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A3D RID: 2621
	public class JobDriver_AttackStatic : JobDriver
	{
		// Token: 0x06003A1D RID: 14877 RVA: 0x001EB8E5 File Offset: 0x001E9CE5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x001EB914 File Offset: 0x001E9D14
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x001EB92C File Offset: 0x001E9D2C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = base.TargetThingA as Pawn;
					if (pawn != null)
					{
						this.startedIncapacitated = pawn.Downed;
					}
					this.pawn.pather.StopDead();
				},
				tickAction = delegate()
				{
					if (!base.TargetA.IsValid)
					{
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						if (base.TargetA.HasThing)
						{
							Pawn pawn = base.TargetA.Thing as Pawn;
							if (base.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed))
							{
								base.EndJobWith(JobCondition.Succeeded);
								return;
							}
						}
						if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
						else if (this.pawn.TryStartAttack(base.TargetA))
						{
							this.numAttacksMade++;
						}
						else if (this.job.endIfCantShootTargetFromCurPos && !this.pawn.stances.FullBodyBusy)
						{
							Verb verb = this.pawn.TryGetAttackVerb(base.TargetA.Thing, !this.pawn.IsColonist);
							if (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, base.TargetA))
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		// Token: 0x04002504 RID: 9476
		private bool startedIncapacitated;

		// Token: 0x04002505 RID: 9477
		private int numAttacksMade;
	}
}
