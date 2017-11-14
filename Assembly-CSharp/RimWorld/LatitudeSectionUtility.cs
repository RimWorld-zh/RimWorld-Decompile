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
			if (num == 0.0 && num2 == 0.0 && num3 == 0.0)
			{
				return LatitudeSection.Undefined;
			}
			if (num == 1.0)
			{
				return LatitudeSection.Equatorial;
			}
			if (num3 == 1.0)
			{
				return LatitudeSection.Polar;
			}
			return LatitudeSection.Seasonal;
		}

		public static LatitudeSection GetDominantLatitudeSection(float latitude)
		{
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			if (num == 0.0 && num2 == 0.0 && num3 == 0.0)
			{
				return LatitudeSection.Undefined;
			}
			return GenMath.MaxBy(LatitudeSection.Equatorial, num, LatitudeSection.Seasonal, num2, LatitudeSection.Polar, num3);
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
			switch (Find.World.info.overallTemperature)
			{
			case OverallTemperature.VeryCold:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return -999f;
				case LatitudeSection.Seasonal:
					return -999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.Cold:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return -999f;
				case LatitudeSection.Seasonal:
					return 15f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.LittleBitColder:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return -999f;
				case LatitudeSection.Seasonal:
					return 40f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.Normal:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 15f;
				case LatitudeSection.Seasonal:
					return 75f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.LittleBitWarmer:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 35f;
				case LatitudeSection.Seasonal:
					return 999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.Hot:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 65f;
				case LatitudeSection.Seasonal:
					return 999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.VeryHot:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 999f;
				case LatitudeSection.Seasonal:
					return 999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			}
			return -1f;
		}
	}
}
