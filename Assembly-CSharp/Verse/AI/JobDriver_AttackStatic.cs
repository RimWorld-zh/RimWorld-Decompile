using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A39 RID: 2617
	public class JobDriver_AttackStatic : JobDriver
	{
		// Token: 0x040024FF RID: 9471
		private bool startedIncapacitated;

		// Token: 0x04002500 RID: 9472
		private int numAttacksMade;

		// Token: 0x06003A19 RID: 14873 RVA: 0x001EBBF9 File Offset: 0x001E9FF9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x001EBC28 File Offset: 0x001EA028
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x001EBC40 File Offset: 0x001EA040
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
	}
}
