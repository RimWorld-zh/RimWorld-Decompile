using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Power.<ConfigErrors>c__Iterator79 <ConfigErrors>c__Iterator = new CompProperties_Power.<ConfigErrors>c__Iterator79();
			<ConfigErrors>c__Iterator.parentDef = parentDef;
			<ConfigErrors>c__Iterator.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator.<>f__this = this;
			CompProperties_Power.<ConfigErrors>c__Iterator79 expr_1C = <ConfigErrors>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
