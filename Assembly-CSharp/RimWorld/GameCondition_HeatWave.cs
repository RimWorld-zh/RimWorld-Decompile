using System;

namespace RimWorld
{
	public class GameCondition_HeatWave : GameCondition
	{
		private const int LerpTicks = 12000;

		private const float MaxTempOffset = 20f;

		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue((float)base.TicksPassed, (float)base.TicksLeft, 12000f, 20f);
		}
	}
}
