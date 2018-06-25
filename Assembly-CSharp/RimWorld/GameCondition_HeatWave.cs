using System;

namespace RimWorld
{
	// Token: 0x02000314 RID: 788
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x0400088E RID: 2190
		private const int LerpTicks = 12000;

		// Token: 0x0400088F RID: 2191
		private const float MaxTempOffset = 17f;

		// Token: 0x06000D5C RID: 3420 RVA: 0x000733FC File Offset: 0x000717FC
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, 17f);
		}
	}
}
