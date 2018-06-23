using System;

namespace RimWorld
{
	// Token: 0x02000312 RID: 786
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x0400088E RID: 2190
		private const int LerpTicks = 12000;

		// Token: 0x0400088F RID: 2191
		private const float MaxTempOffset = 17f;

		// Token: 0x06000D58 RID: 3416 RVA: 0x000732AC File Offset: 0x000716AC
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, 17f);
		}
	}
}
