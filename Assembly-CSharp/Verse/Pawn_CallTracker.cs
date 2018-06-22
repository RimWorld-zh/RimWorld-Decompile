using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D53 RID: 3411
	public class Pawn_CallTracker
	{
		// Token: 0x06004C2C RID: 19500 RVA: 0x0027BE2C File Offset: 0x0027A22C
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06004C2D RID: 19501 RVA: 0x0027BE44 File Offset: 0x0027A244
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004C2E RID: 19502 RVA: 0x0027BF00 File Offset: 0x0027A300
		private float IdleCallVolumeFactor
		{
			get
			{
				float result;
				switch (Find.TickManager.CurTimeSpeed)
				{
				case TimeSpeed.Paused:
					result = 1f;
					break;
				case TimeSpeed.Normal:
					result = 1f;
					break;
				case TimeSpeed.Fast:
					result = 1f;
					break;
				case TimeSpeed.Superfast:
					result = 0.25f;
					break;
				case TimeSpeed.Ultrafast:
					result = 0.25f;
					break;
				default:
					throw new NotImplementedException();
				}
				return result;
			}
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x0027BF76 File Offset: 0x0027A376
		public void CallTrackerTick()
		{
			if (this.ticksToNextCall < 0)
			{
				this.ResetTicksToNextCall();
			}
			this.ticksToNextCall--;
			if (this.ticksToNextCall <= 0)
			{
				this.TryDoCall();
				this.ResetTicksToNextCall();
			}
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0027BFB3 File Offset: 0x0027A3B3
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x0027BFF0 File Offset: 0x0027A3F0
		private void TryDoCall()
		{
			if (Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(this.pawn.Position))
			{
				if (!this.pawn.Downed && this.pawn.Awake())
				{
					if (!this.pawn.Position.Fogged(this.pawn.Map))
					{
						this.DoCall();
					}
				}
			}
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x0027C080 File Offset: 0x0027A480
		public void DoCall()
		{
			if (this.pawn.Spawned)
			{
				if (this.PawnAggressive)
				{
					LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundAngry, 1f);
				}
				else
				{
					LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundCall, this.IdleCallVolumeFactor);
				}
			}
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x0027C110 File Offset: 0x0027A510
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x0027C134 File Offset: 0x0027A534
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x0027C164 File Offset: 0x0027A564
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x040032FB RID: 13051
		public Pawn pawn;

		// Token: 0x040032FC RID: 13052
		private int ticksToNextCall = -1;

		// Token: 0x040032FD RID: 13053
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x040032FE RID: 13054
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x040032FF RID: 13055
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x04003300 RID: 13056
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
