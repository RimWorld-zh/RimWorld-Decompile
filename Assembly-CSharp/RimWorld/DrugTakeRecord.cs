using Verse;

namespace RimWorld
{
	public class DrugTakeRecord : IExposable
	{
		public ThingDef drug;

		public int lastTakenTicks;

		private int timesTakenThisDayInt;

		private int thisDay;

		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		public int TimesTakenThisDay
		{
			get
			{
				return (this.thisDay == GenDate.DaysPassed) ? this.timesTakenThisDayInt : 0;
			}
			set
			{
				this.timesTakenThisDayInt = value;
				this.thisDay = GenDate.DaysPassed;
			}
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}
	}
}
