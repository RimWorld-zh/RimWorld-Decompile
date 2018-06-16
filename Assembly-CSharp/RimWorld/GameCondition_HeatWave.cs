using System;

namespace RimWorld
{
	// Token: 0x02000312 RID: 786
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x06000D58 RID: 3416 RVA: 0x000731F8 File Offset: 0x000715F8
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, 17f);
		}

		// Token: 0x0400088C RID: 2188
		private const int LerpTicks = 12000;

		// Token: 0x0400088D RID: 2189
		private const float MaxTempOffset = 17f;
	}
}
