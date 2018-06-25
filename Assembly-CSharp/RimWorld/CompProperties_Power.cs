using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024F RID: 591
	public class CompProperties_Power : CompProperties
	{
		// Token: 0x0400049C RID: 1180
		public bool transmitsPower = false;

		// Token: 0x0400049D RID: 1181
		public float basePowerConsumption = 0f;

		// Token: 0x0400049E RID: 1182
		public bool shortCircuitInRain = false;

		// Token: 0x0400049F RID: 1183
		public SoundDef soundPowerOn = null;

		// Token: 0x040004A0 RID: 1184
		public SoundDef soundPowerOff = null;

		// Token: 0x040004A1 RID: 1185
		public SoundDef soundAmbientPowered = null;
	}
}
