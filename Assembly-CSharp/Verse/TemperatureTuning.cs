using System;

namespace Verse
{
	// Token: 0x02000CAC RID: 3244
	public static class TemperatureTuning
	{
		// Token: 0x0400307E RID: 12414
		public const float MinimumTemperature = -273.15f;

		// Token: 0x0400307F RID: 12415
		public const float MaximumTemperature = 2000f;

		// Token: 0x04003080 RID: 12416
		public const float DefaultTemperature = 21f;

		// Token: 0x04003081 RID: 12417
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04003082 RID: 12418
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

		// Token: 0x04003083 RID: 12419
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x04003084 RID: 12420
		public const float DailySunEffect = 14f;

		// Token: 0x04003085 RID: 12421
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x04003086 RID: 12422
		public const float FoodFreezingTemp = 0f;

		// Token: 0x04003087 RID: 12423
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x04003088 RID: 12424
		public const int Door_TempEqualizeIntervalOpen = 22;

		// Token: 0x04003089 RID: 12425
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x0400308A RID: 12426
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x0400308B RID: 12427
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x0400308C RID: 12428
		public const float InventoryTemperature = 14f;

		// Token: 0x0400308D RID: 12429
		public const float DropPodTemperature = 14f;

		// Token: 0x0400308E RID: 12430
		public const float TradeShipTemperature = 14f;
	}
}
