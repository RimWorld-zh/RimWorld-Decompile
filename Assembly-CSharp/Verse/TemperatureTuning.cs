using System;

namespace Verse
{
	// Token: 0x02000CAD RID: 3245
	public static class TemperatureTuning
	{
		// Token: 0x04003073 RID: 12403
		public const float MinimumTemperature = -273.15f;

		// Token: 0x04003074 RID: 12404
		public const float MaximumTemperature = 2000f;

		// Token: 0x04003075 RID: 12405
		public const float DefaultTemperature = 21f;

		// Token: 0x04003076 RID: 12406
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04003077 RID: 12407
		public static readonly SimpleCurve SeasonalTempVariationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(0.1f, 4f),
				true
			},
			{
				new CurvePoint(1f, 28f),
				true
			}
		};

		// Token: 0x04003078 RID: 12408
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x04003079 RID: 12409
		public const float DailySunEffect = 14f;

		// Token: 0x0400307A RID: 12410
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x0400307B RID: 12411
		public const float FoodFreezingTemp = 0f;

		// Token: 0x0400307C RID: 12412
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x0400307D RID: 12413
		public const int Door_TempEqualizeIntervalOpen = 22;

		// Token: 0x0400307E RID: 12414
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x0400307F RID: 12415
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x04003080 RID: 12416
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x04003081 RID: 12417
		public const float InventoryTemperature = 14f;

		// Token: 0x04003082 RID: 12418
		public const float DropPodTemperature = 14f;

		// Token: 0x04003083 RID: 12419
		public const float TradeShipTemperature = 14f;
	}
}
