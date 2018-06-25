using System;

namespace RimWorld
{
	// Token: 0x02000315 RID: 789
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x04000893 RID: 2195
		private const int LerpTicks = 12000;

		// Token: 0x04000894 RID: 2196
		private const float MaxTempOffset = -20f;

		// Token: 0x06000D5D RID: 3421 RVA: 0x00073434 File Offset: 0x00071834
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, 12000f, -20f);
		}
	}
}
