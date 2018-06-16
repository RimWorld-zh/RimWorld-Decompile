using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5E RID: 3422
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x06004C95 RID: 19605 RVA: 0x0027E6C5 File Offset: 0x0027CAC5
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0027E700 File Offset: 0x0027CB00
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x0027E734 File Offset: 0x0027CB34
		public bool Staggered
		{
			get
			{
				return Find.TickManager.TicksGame < this.staggerUntilTick;
			}
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x0027E75B File Offset: 0x0027CB5B
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			if (!this.stunner.Stunned)
			{
				this.curStance.StanceTick();
			}
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x0027E784 File Offset: 0x0027CB84
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x0027E794 File Offset: 0x0027CB94
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

		// Token: 0x06004C9B RID: 19611 RVA: 0x0027E80B File Offset: 0x0027CC0B
		public void StaggerFor(int ticks)
		{
			this.staggerUntilTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0027E820 File Offset: 0x0027CC20
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0027E83E File Offset: 0x0027CC3E
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x0027E84C File Offset: 0x0027CC4C
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

		// Token: 0x06004C9F RID: 19615 RVA: 0x0027E8EE File Offset: 0x0027CCEE
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x04003319 RID: 13081
		public Pawn pawn;

		// Token: 0x0400331A RID: 13082
		public Stance curStance = new Stance_Mobile();

		// Token: 0x0400331B RID: 13083
		private int staggerUntilTick = -1;

		// Token: 0x0400331C RID: 13084
		public StunHandler stunner;

		// Token: 0x0400331D RID: 13085
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x0400331E RID: 13086
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x0400331F RID: 13087
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x04003320 RID: 13088
		public bool debugLog = false;
	}
}
