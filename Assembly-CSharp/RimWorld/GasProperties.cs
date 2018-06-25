using System;
using Verse;

namespace RimWorld
{
	public class GasProperties
	{
		public bool blockTurretTracking = false;

		public float accuracyPenalty = 0f;

		public FloatRange expireSeconds = new FloatRange(30f, 30f);

		public float rotationSpeed = 0f;

		public GasProperties()
		{
		}
	}
}
