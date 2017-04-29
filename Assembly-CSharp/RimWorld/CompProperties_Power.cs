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
			CompProperties_Power.<ConfigErrors>c__Iterator7A <ConfigErrors>c__Iterator7A = new CompProperties_Power.<ConfigErrors>c__Iterator7A();
			<ConfigErrors>c__Iterator7A.parentDef = parentDef;
			<ConfigErrors>c__Iterator7A.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator7A.<>f__this = this;
			CompProperties_Power.<ConfigErrors>c__Iterator7A expr_1C = <ConfigErrors>c__Iterator7A;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
