using System;
using UnityEngine;

namespace Verse
{
	public static class Altitudes
	{
		private const int NumAltitudeLayers = 31;

		private static readonly float[] Alts = new float[31];

		private const float LayerSpacing = 0.46875f;

		public const float AltInc = 0.046875f;

		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.046875f, 0f);

		static Altitudes()
		{
			for (int i = 0; i < 31; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.46875f;
			}
		}

		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}
	}
}
