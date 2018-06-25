using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5D RID: 3421
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x04003329 RID: 13097
		public Pawn pawn;

		// Token: 0x0400332A RID: 13098
		public Stance curStance = new Stance_Mobile();

		// Token: 0x0400332B RID: 13099
		private int staggerUntilTick = -1;

		// Token: 0x0400332C RID: 13100
		public StunHandler stunner;

		// Token: 0x0400332D RID: 13101
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x0400332E RID: 13102
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x0400332F RID: 13103
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04003330 RID: 13104
		public bool debugLog = false;

		// Token: 0x06004CAC RID: 19628 RVA: 0x00280061 File Offset: 0x0027E461
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x0028009C File Offset: 0x0027E49C
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004CAE RID: 19630 RVA: 0x002800D0 File Offset: 0x0027E4D0
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x002800F7 File Offset: 0x0027E4F7
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x00280120 File Offset: 0x0027E520
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x00280130 File Offset: 0x0027E530
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.staggerUntilTick, "staggerUntilTick", 0, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<Stance>(ref this.curStance, "curStance", new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.curStance != null)
			{
				this.curStance.stanceTracker = this;
			}
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x002801A7 File Offset: 0x0027E5A7
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x002801BC File Offset: 0x0027E5BC
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x002801DA File Offset: 0x0027E5DA
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x002801E8 File Offset: 0x0027E5E8
		public void SetStance(Stance newStance)
		{
			if (this.debugLog)
			{
				Log.Message(string.Concat(new object[]
				{
					Find.TickManager.TicksGame,
					" ",
					this.pawn,
					" SetStance ",
					this.curStance,
					" -> ",
					newStance
				}), false);
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0028028A File Offset: 0x0027E68A
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}
	}
}
