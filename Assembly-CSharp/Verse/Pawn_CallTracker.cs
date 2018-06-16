using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D57 RID: 3415
	public class Pawn_CallTracker
	{
		// Token: 0x06004C1A RID: 19482 RVA: 0x0027A8B0 File Offset: 0x00278CB0
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x0027A8C8 File Offset: 0x00278CC8
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06004C1C RID: 19484 RVA: 0x0027A984 File Offset: 0x00278D84
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

		// Token: 0x06004C1D RID: 19485 RVA: 0x0027A9FA File Offset: 0x00278DFA
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

		// Token: 0x06004C1E RID: 19486 RVA: 0x0027AA37 File Offset: 0x00278E37
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x0027AA74 File Offset: 0x00278E74
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

		// Token: 0x06004C20 RID: 19488 RVA: 0x0027AB04 File Offset: 0x00278F04
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

		// Token: 0x06004C21 RID: 19489 RVA: 0x0027AB94 File Offset: 0x00278F94
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x0027ABB8 File Offset: 0x00278FB8
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x0027ABE8 File Offset: 0x00278FE8
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x040032F2 RID: 13042
		public Pawn pawn;

		// Token: 0x040032F3 RID: 13043
		private int ticksToNextCall = -1;

		// Token: 0x040032F4 RID: 13044
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x040032F5 RID: 13045
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x040032F6 RID: 13046
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x040032F7 RID: 13047
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
