using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Power : CompProperties
	{
		public bool transmitsPower = false;

		public float basePowerConsumption = 0f;

		public bool startElectricalFires = false;

		public bool shortCircuitInRain = true;

		public SoundDef soundPowerOn = null;

		public SoundDef soundPowerOff = null;

		public SoundDef soundAmbientPowered = null;

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00c3:
			/*Error near IL_00c4: Unexpected return in MoveNext()*/;
		}
	}
}
