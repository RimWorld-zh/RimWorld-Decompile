using System.Linq;
using Verse;

namespace RimWorld
{
	public class StoryWatcher_RampUp : IExposable
	{
		private const int UpdateInterval = 5000;

		private const float ShortFactor_GameStartGraceDays = 21f;

		private const float ShortFactor_DaysToDouble = 162f;

		private const float LongFactor_GameStartGraceDays = 42f;

		private const float LongFactor_DaysToDouble = 360f;

		private float shortTermFactor = 1f;

		private float longTermFactor = 1f;

		public float TotalThreatPointsFactor
		{
			get
			{
				return this.shortTermFactor * this.longTermFactor;
			}
		}

		public float ShortTermFactor
		{
			get
			{
				return this.shortTermFactor;
			}
		}

		public float LongTermFactor
		{
			get
			{
				return this.longTermFactor;
			}
		}

		public void Notify_PlayerPawnIncappedOrKilled(Pawn p)
		{
			if (p.RaceProps.Humanlike)
			{
				float num = (float)(this.shortTermFactor - 1.0);
				float num2 = (float)(this.longTermFactor - 1.0);
				switch (PawnsFinder.AllMapsCaravansAndTravelingTransportPods_FreeColonists.Count())
				{
				case 0:
				{
					num = (float)(num * 0.0);
					num2 = (float)(num2 * 0.0);
					break;
				}
				case 1:
				{
					num = (float)(num * 0.0);
					num2 = (float)(num2 * 0.0);
					break;
				}
				case 2:
				{
					num = (float)(num * 0.0);
					num2 = (float)(num2 * 0.0);
					break;
				}
				case 3:
				{
					num = (float)(num * 0.0);
					num2 = (float)(num2 * 0.20000000298023224);
					break;
				}
				case 4:
				{
					num = (float)(num * 0.15000000596046448);
					num2 = (float)(num2 * 0.40000000596046448);
					break;
				}
				case 5:
				{
					num = (float)(num * 0.25);
					num2 = (float)(num2 * 0.60000002384185791);
					break;
				}
				case 6:
				{
					num = (float)(num * 0.30000001192092896);
					num2 = (float)(num2 * 0.699999988079071);
					break;
				}
				case 7:
				{
					num = (float)(num * 0.34999999403953552);
					num2 = (float)(num2 * 0.75);
					break;
				}
				case 8:
				{
					num = (float)(num * 0.40000000596046448);
					num2 = (float)(num2 * 0.800000011920929);
					break;
				}
				case 9:
				{
					num = (float)(num * 0.44999998807907104);
					num2 = (float)(num2 * 0.85000002384185791);
					break;
				}
				case 10:
				{
					num = (float)(num * 0.5);
					num2 = (float)(num2 * 0.89999997615814209);
					break;
				}
				case 11:
				{
					num = (float)(num * 0.550000011920929);
					num2 = (float)(num2 * 0.9100000262260437);
					break;
				}
				case 12:
				{
					num = (float)(num * 0.60000002384185791);
					num2 = (float)(num2 * 0.92000001668930054);
					break;
				}
				case 13:
				{
					num = (float)(num * 0.64999997615814209);
					num2 = (float)(num2 * 0.93000000715255737);
					break;
				}
				case 14:
				{
					num = (float)(num * 0.699999988079071);
					num2 = (float)(num2 * 0.93999999761581421);
					break;
				}
				case 15:
				{
					num = (float)(num * 0.75);
					num2 = (float)(num2 * 0.949999988079071);
					break;
				}
				default:
				{
					num = (float)(num * 0.800000011920929);
					num2 = (float)(num2 * 0.949999988079071);
					break;
				}
				}
				this.shortTermFactor = (float)(1.0 + num);
				this.longTermFactor = (float)(1.0 + num2);
			}
		}

		public void RampUpWatcherTick()
		{
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if ((float)GenDate.DaysPassed >= 21.0)
				{
					this.shortTermFactor += 0.000514403335f;
				}
				if ((float)GenDate.DaysPassed >= 42.0)
				{
					this.longTermFactor += 0.000231481492f;
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.shortTermFactor, "shortTermFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.longTermFactor, "longTermFactor", 0f, false);
		}
	}
}
