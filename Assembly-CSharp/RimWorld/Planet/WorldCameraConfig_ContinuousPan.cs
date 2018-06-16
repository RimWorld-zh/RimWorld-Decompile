using System;

namespace RimWorld.Planet
{
	// Token: 0x02000580 RID: 1408
	public class WorldCameraConfig_ContinuousPan : WorldCameraConfig
	{
		// Token: 0x06001ADF RID: 6879 RVA: 0x000E6DE4 File Offset: 0x000E51E4
		public WorldCameraConfig_ContinuousPan()
		{
			this.dollyRateKeys = 34f;
			this.dollyRateMouseDrag = 15.4f;
			this.dollyRateScreenEdge = 17.85f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}
	}
}
