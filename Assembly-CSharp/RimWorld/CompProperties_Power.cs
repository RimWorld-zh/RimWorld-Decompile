using System;
using Verse;

namespace RimWorld
{
	public class CompProperties_Power : CompProperties
	{
		public bool transmitsPower = false;

		public float basePowerConsumption = 0f;

		public bool shortCircuitInRain = false;

		public SoundDef soundPowerOn = null;

		public SoundDef soundPowerOff = null;

		public SoundDef soundAmbientPowered = null;

		public CompProperties_Power()
		{
		}
	}
}
