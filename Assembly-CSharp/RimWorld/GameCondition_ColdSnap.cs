using System;

namespace RimWorld
{
	// Token: 0x02000315 RID: 789
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x04000890 RID: 2192
		private const int LerpTicks = 12000;

		// Token: 0x04000891 RID: 2193
		private const float MaxTempOffset = -20f;

		// Token: 0x06000D5E RID: 3422 RVA: 0x0007342C File Offset: 0x0007182C
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, -20f);
		}
	}
}
