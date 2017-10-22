using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Power : CompProperties
	{
		public bool transmitsPower;

		public float basePowerConsumption;

		public bool startElectricalFires;

		public bool shortCircuitInRain = true;

		public SoundDef soundPowerOn;

		public SoundDef soundPowerOff;

		public SoundDef soundAmbientPowered;

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string item in base.ConfigErrors(parentDef))
			{
				yield return item;
			}
		}
	}
}
