using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class LatitudeSectionUtility
	{
		private const float LerpDistance = 5f;

		public static LatitudeSection GetReportedLatitudeSection(float latitude)
		{
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			return (LatitudeSection)((num != 0.0 || num2 != 0.0 || num3 != 0.0) ? ((num == 1.0) ? 1 : ((num3 != 1.0) ? 2 : 3)) : 0);
		}

		public static LatitudeSection GetDominantLatitudeSection(float latitude)
		{
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			return (num != 0.0 || num2 != 0.0 || num3 != 0.0) ? GenMath.MaxBy(LatitudeSection.Equatorial, num, LatitudeSection.Seasonal, num2, LatitudeSection.Polar, num3) : LatitudeSection.Undefined;
		}

		public static void GetLatitudeSection(float latitude, out float equatorial, out float seasonal, out float polar)
		{
			float num = Mathf.Abs(latitude);
			float maxLatitude = LatitudeSection.Equatorial.GetMaxLatitude();
			float maxLatitude2 = LatitudeSection.Seasonal.GetMaxLatitude();
			float maxLatitude3 = LatitudeSection.Polar.GetMaxLatitude();
			if (num <= maxLatitude)
			{
				equatorial = 1f;
				seasonal = 0f;
				polar = 0f;
			}
			else if (num <= maxLatitude2)
			{
				equatorial = Mathf.InverseLerp((float)(maxLatitude + 5.0), maxLatitude, num);
				float a = (float)(1.0 - equatorial);
				polar = Mathf.InverseLerp((float)(maxLatitude2 - 5.0), maxLatitude2, num);
				float b = (float)(1.0 - polar);
				seasonal = Mathf.Min(a, b);
				GenMath.NormalizeToSum1(ref equatorial, ref seasonal, ref polar);
			}
			else if (num <= maxLatitude3)
			{
				equatorial = 0f;
				seasonal = 0f;
				polar = 1f;
			}
			else
			{
				equatorial = 0f;
				seasonal = 0f;
				polar = 0f;
			}
		}

		public static float GetMaxLatitude(this LatitudeSection latitudeSection)
		{
			float result;
			switch (Find.World.info.overallTemperature)
			{
			case OverallTemperature.VeryCold:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = -999f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = -999f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			case OverallTemperature.Cold:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = -999f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = 15f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			case OverallTemperature.LittleBitColder:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = -999f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = 40f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			case OverallTemperature.Normal:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = 15f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = 75f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			case OverallTemperature.LittleBitWarmer:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = 35f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = 999f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			case OverallTemperature.Hot:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = 65f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = 999f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			case OverallTemperature.VeryHot:
			{
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
				{
					result = 999f;
					goto IL_0203;
				}
				case LatitudeSection.Seasonal:
				{
					result = 999f;
					goto IL_0203;
				}
				case LatitudeSection.Polar:
				{
					result = 999f;
					goto IL_0203;
				}
				}
				break;
			}
			}
			result = -1f;
			goto IL_0203;
			IL_0203:
			return result;
		}
	}
}
