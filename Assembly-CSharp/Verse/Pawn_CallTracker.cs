using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D56 RID: 3414
	public class Pawn_CallTracker
	{
		// Token: 0x06004C18 RID: 19480 RVA: 0x0027A890 File Offset: 0x00278C90
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x0027A8A8 File Offset: 0x00278CA8
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x0027A964 File Offset: 0x00278D64
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

		// Token: 0x06004C1B RID: 19483 RVA: 0x0027A9DA File Offset: 0x00278DDA
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

		// Token: 0x06004C1C RID: 19484 RVA: 0x0027AA17 File Offset: 0x00278E17
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x0027AA54 File Offset: 0x00278E54
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

		// Token: 0x06004C1E RID: 19486 RVA: 0x0027AAE4 File Offset: 0x00278EE4
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

		// Token: 0x06004C1F RID: 19487 RVA: 0x0027AB74 File Offset: 0x00278F74
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x0027AB98 File Offset: 0x00278F98
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x0027ABC8 File Offset: 0x00278FC8
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x040032F0 RID: 13040
		public Pawn pawn;

		// Token: 0x040032F1 RID: 13041
		private int ticksToNextCall = -1;

		// Token: 0x040032F2 RID: 13042
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x040032F3 RID: 13043
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x040032F4 RID: 13044
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x040032F5 RID: 13045
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
