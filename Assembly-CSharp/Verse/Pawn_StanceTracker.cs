using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5C RID: 3420
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x04003322 RID: 13090
		public Pawn pawn;

		// Token: 0x04003323 RID: 13091
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04003324 RID: 13092
		private int staggerUntilTick = -1;

		// Token: 0x04003325 RID: 13093
		public StunHandler stunner;

		// Token: 0x04003326 RID: 13094
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x04003327 RID: 13095
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x04003328 RID: 13096
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04003329 RID: 13097
		public bool debugLog = false;

		// Token: 0x06004CAC RID: 19628 RVA: 0x0027FD81 File Offset: 0x0027E181
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x0027FDBC File Offset: 0x0027E1BC
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004CAE RID: 19630 RVA: 0x0027FDF0 File Offset: 0x0027E1F0
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x0027FE17 File Offset: 0x0027E217
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0027FE40 File Offset: 0x0027E240
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0027FE50 File Offset: 0x0027E250
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

		// Token: 0x06004CB2 RID: 19634 RVA: 0x0027FEC7 File Offset: 0x0027E2C7
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x0027FEDC File Offset: 0x0027E2DC
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x0027FEFA File Offset: 0x0027E2FA
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x0027FF08 File Offset: 0x0027E308
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

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0027FFAA File Offset: 0x0027E3AA
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}
	}
}
