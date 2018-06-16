using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035C RID: 860
	public class StoryWatcher_RampUp : IExposable
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x0007DBFC File Offset: 0x0007BFFC
		public float TotalThreatPointsFactor
		{
			get
			{
				return this.shortTermFactor * this.longTermFactor;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x0007DC20 File Offset: 0x0007C020
		public float ShortTermFactor
		{
			get
			{
				return this.shortTermFactor;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x0007DC3C File Offset: 0x0007C03C
		public float LongTermFactor
		{
			get
			{
				return this.longTermFactor;
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0007DC58 File Offset: 0x0007C058
		public void Notify_ColonistViolentlyDownedOrKilled(Pawn p)
		{
			if (p.RaceProps.Humanlike)
			{
				float num = this.shortTermFactor - 1f;
				float num2 = this.longTermFactor - 1f;
				int num3 = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count<Pawn>();
				switch (num3)
				{
				case 0:
					num *= 0f;
					num2 *= 0f;
					break;
				case 1:
					num *= 0f;
					num2 *= 0f;
					break;
				case 2:
					num *= 0f;
					num2 *= 0f;
					break;
				case 3:
					num *= 0f;
					num2 *= 0.2f;
					break;
				case 4:
					num *= 0.15f;
					num2 *= 0.4f;
					break;
				case 5:
					num *= 0.25f;
					num2 *= 0.6f;
					break;
				case 6:
					num *= 0.3f;
					num2 *= 0.7f;
					break;
				case 7:
					num *= 0.35f;
					num2 *= 0.75f;
					break;
				case 8:
					num *= 0.4f;
					num2 *= 0.8f;
					break;
				case 9:
					num *= 0.45f;
					num2 *= 0.85f;
					break;
				case 10:
					num *= 0.5f;
					num2 *= 0.9f;
					break;
				case 11:
					num *= 0.55f;
					num2 *= 0.91f;
					break;
				case 12:
					num *= 0.6f;
					num2 *= 0.92f;
					break;
				case 13:
					num *= 0.65f;
					num2 *= 0.93f;
					break;
				case 14:
					num *= 0.7f;
					num2 *= 0.94f;
					break;
				case 15:
					num *= 0.75f;
					num2 *= 0.95f;
					break;
				default:
					num *= GenMath.LerpDoubleClamped(16f, 30f, 0.8f, 1f, (float)num3);
					num2 *= GenMath.LerpDoubleClamped(16f, 30f, 0.95f, 1f, (float)num3);
					break;
				}
				this.shortTermFactor = 1f + num;
				this.longTermFactor = 1f + num2;
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0007DE98 File Offset: 0x0007C298
		public void RampUpWatcherTick()
		{
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if ((float)GenDate.DaysPassed >= 21f)
				{
					this.shortTermFactor += 0.000356125354f;
				}
				if ((float)GenDate.DaysPassed >= 42f)
				{
					this.longTermFactor += 0.000231481492f;
				}
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0007DF01 File Offset: 0x0007C301
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.shortTermFactor, "shortTermFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.longTermFactor, "longTermFactor", 0f, false);
		}

		// Token: 0x04000929 RID: 2345
		private float shortTermFactor = 1f;

		// Token: 0x0400092A RID: 2346
		private float longTermFactor = 1f;

		// Token: 0x0400092B RID: 2347
		private const int UpdateInterval = 5000;

		// Token: 0x0400092C RID: 2348
		private const float ShortFactor_GameStartGraceDays = 21f;

		// Token: 0x0400092D RID: 2349
		private const float ShortFactor_DaysToDouble = 234f;

		// Token: 0x0400092E RID: 2350
		private const float LongFactor_GameStartGraceDays = 42f;

		// Token: 0x0400092F RID: 2351
		private const float LongFactor_DaysToDouble = 360f;
	}
}
