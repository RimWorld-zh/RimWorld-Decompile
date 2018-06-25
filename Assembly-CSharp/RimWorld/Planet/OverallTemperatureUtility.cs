using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200057B RID: 1403
	public static class OverallTemperatureUtility
	{
		// Token: 0x04000FA6 RID: 4006
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04000FA7 RID: 4007
		private static readonly SimpleCurve Curve_VeryCold = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-50f, -75f),
				true
			},
			{
				new CurvePoint(-40f, -60f),
				true
			},
			{
				new CurvePoint(0f, -35f),
				true
			},
			{
				new CurvePoint(20f, -28f),
				true
			},
			{
				new CurvePoint(25f, -18f),
				true
			},
			{
				new CurvePoint(30f, -8.5f),
				true
			},
			{
				new CurvePoint(50f, -7f),
				true
			}
		};

		// Token: 0x04000FA8 RID: 4008
		private static readonly SimpleCurve Curve_Cold = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-50f, -70f),
				true
			},
			{
				new CurvePoint(-25f, -40f),
				true
			},
			{
				new CurvePoint(-20f, -25f),
				true
			},
			{
				new CurvePoint(-13f, -15f),
				true
			},
			{
				new CurvePoint(0f, -12f),
				true
			},
			{
				new CurvePoint(30f, -3f),
				true
			},
			{
				new CurvePoint(60f, 25f),
				true
			}
		};

		// Token: 0x04000FA9 RID: 4009
		private static readonly SimpleCurve Curve_LittleBitColder = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-20f, -22f),
				true
			},
			{
				new CurvePoint(-15f, -15f),
				true
			},
			{
				new CurvePoint(-5f, -13f),
				true
			},
			{
				new CurvePoint(40f, 30f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x04000FAA RID: 4010
		private static readonly SimpleCurve Curve_LittleBitWarmer = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-45f, -35f),
				true
			},
			{
				new CurvePoint(40f, 50f),
				true
			},
			{
				new CurvePoint(120f, 120f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x04000FAB RID: 4011
		private static readonly SimpleCurve Curve_Hot = new SimpleCurve
		{
			{
				new CurvePoint(-45f, -22f),
				true
			},
			{
				new CurvePoint(-25f, -12f),
				true
			},
			{
				new CurvePoint(-22f, 2f),
				true
			},
			{
				new CurvePoint(-10f, 25f),
				true
			},
			{
				new CurvePoint(40f, 57f),
				true
			},
			{
				new CurvePoint(120f, 120f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x04000FAC RID: 4012
		private static readonly SimpleCurve Curve_VeryHot = new SimpleCurve
		{
			{
				new CurvePoint(-45f, 25f),
				true
			},
			{
				new CurvePoint(0f, 40f),
				true
			},
			{
				new CurvePoint(33f, 80f),
				true
			},
			{
				new CurvePoint(40f, 88f),
				true
			},
			{
				new CurvePoint(120f, 120f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x000E6D70 File Offset: 0x000E5170
		public static int EnumValuesCount
		{
			get
			{
				if (OverallTemperatureUtility.cachedEnumValuesCount < 0)
				{
					OverallTemperatureUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallTemperature)).Length;
				}
				return OverallTemperatureUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000E6DAC File Offset: 0x000E51AC
		public static SimpleCurve GetTemperatureCurve(this OverallTemperature overallTemperature)
		{
			switch (overallTemperature)
			{
			case OverallTemperature.VeryCold:
				return OverallTemperatureUtility.Curve_VeryCold;
			case OverallTemperature.Cold:
				return OverallTemperatureUtility.Curve_Cold;
			case OverallTemperature.LittleBitColder:
				return OverallTemperatureUtility.Curve_LittleBitColder;
			case OverallTemperature.LittleBitWarmer:
				return OverallTemperatureUtility.Curve_LittleBitWarmer;
			case OverallTemperature.Hot:
				return OverallTemperatureUtility.Curve_Hot;
			case OverallTemperature.VeryHot:
				return OverallTemperatureUtility.Curve_VeryHot;
			}
			return null;
		}
	}
}
