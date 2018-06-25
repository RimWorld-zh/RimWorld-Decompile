using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D55 RID: 3413
	public class Pawn_CallTracker
	{
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

		// Token: 0x06004C30 RID: 19504 RVA: 0x0027BF58 File Offset: 0x0027A358
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004C31 RID: 19505 RVA: 0x0027BF70 File Offset: 0x0027A370
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x0027C02C File Offset: 0x0027A42C
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

		// Token: 0x06004C33 RID: 19507 RVA: 0x0027C0A2 File Offset: 0x0027A4A2
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

		// Token: 0x06004C34 RID: 19508 RVA: 0x0027C0DF File Offset: 0x0027A4DF
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x0027C11C File Offset: 0x0027A51C
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

		// Token: 0x06004C36 RID: 19510 RVA: 0x0027C1AC File Offset: 0x0027A5AC
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

		// Token: 0x06004C37 RID: 19511 RVA: 0x0027C23C File Offset: 0x0027A63C
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x0027C260 File Offset: 0x0027A660
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x0027C290 File Offset: 0x0027A690
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}
	}
}
