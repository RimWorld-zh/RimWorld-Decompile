using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GameCondition_ClimateCycle : GameCondition
	{
		private int ticksOffset = 0;

		private const float PeriodYears = 4f;

		private const float MaxTempOffset = 20f;

		public override void Init()
		{
			this.ticksOffset = ((!(Rand.Value < 0.5)) ? 7200000 : 0);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		public override float TemperatureOffset()
		{
			return (float)(Mathf.Sin((float)(GenDate.YearsPassedFloat / 4.0 * 3.1415927410125732 * 2.0)) * 20.0);
		}
	}
}
