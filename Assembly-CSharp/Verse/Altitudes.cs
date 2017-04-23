using System;
using UnityEngine;

namespace Verse
{
	public static class Altitudes
	{
		private const int NumAltitudeLayers = 30;

		private const float LayerSpacing = 0.483870953f;

		public const float AltInc = 0.0483870953f;

		private static readonly float[] Alts;

		public static readonly Vector3 AltIncVect;

		static Altitudes()
		{
			Altitudes.Alts = new float[30];
			Altitudes.AltIncVect = new Vector3(0f, 0.0483870953f, 0f);
			if (Enum.GetValues(typeof(AltitudeLayer)).Length != 30)
			{
				Log.Message("Altitudes.NumAltitudeLayers should be " + Enum.GetValues(typeof(AltitudeLayer)).Length);
			}
			for (int i = 0; i < 30; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.483870953f;
			}
		}

		public static float AltitudeFor(AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}
	}
}
