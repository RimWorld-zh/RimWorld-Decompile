using System;

namespace RimWorld
{
	// Token: 0x02000313 RID: 787
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x06000D5A RID: 3418 RVA: 0x00073228 File Offset: 0x00071628
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, -20f);
		}

		// Token: 0x0400088E RID: 2190
		private const int LerpTicks = 12000;

		// Token: 0x0400088F RID: 2191
		private const float MaxTempOffset = -20f;
	}
}
