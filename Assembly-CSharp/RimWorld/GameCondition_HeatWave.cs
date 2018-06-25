using System;

namespace RimWorld
{
	// Token: 0x02000314 RID: 788
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x04000891 RID: 2193
		private const int LerpTicks = 12000;

		// Token: 0x04000892 RID: 2194
		private const float MaxTempOffset = 17f;

		// Token: 0x06000D5B RID: 3419 RVA: 0x00073404 File Offset: 0x00071804
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, 17f);
		}
	}
}
