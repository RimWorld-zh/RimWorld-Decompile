using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5D RID: 3421
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x06004C93 RID: 19603 RVA: 0x0027E6A5 File Offset: 0x0027CAA5
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0027E6E0 File Offset: 0x0027CAE0
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x0027E714 File Offset: 0x0027CB14
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0027E73B File Offset: 0x0027CB3B
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x0027E764 File Offset: 0x0027CB64
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x0027E774 File Offset: 0x0027CB74
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

		// Token: 0x06004C99 RID: 19609 RVA: 0x0027E7EB File Offset: 0x0027CBEB
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x0027E800 File Offset: 0x0027CC00
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x0027E81E File Offset: 0x0027CC1E
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0027E82C File Offset: 0x0027CC2C
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

		// Token: 0x06004C9D RID: 19613 RVA: 0x0027E8CE File Offset: 0x0027CCCE
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x04003317 RID: 13079
		public Pawn pawn;

		// Token: 0x04003318 RID: 13080
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04003319 RID: 13081
		private int staggerUntilTick = -1;

		// Token: 0x0400331A RID: 13082
		public StunHandler stunner;

		// Token: 0x0400331B RID: 13083
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x0400331C RID: 13084
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x0400331D RID: 13085
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x0400331E RID: 13086
		public bool debugLog = false;
	}
}
