using System;

namespace RimWorld
{
	// Token: 0x02000313 RID: 787
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x04000890 RID: 2192
		private const int LerpTicks = 12000;

		// Token: 0x04000891 RID: 2193
		private const float MaxTempOffset = -20f;

		// Token: 0x06000D5A RID: 3418 RVA: 0x000732DC File Offset: 0x000716DC
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, -20f);
		}
	}
}
