using System;

namespace RimWorld.Planet
{
	// Token: 0x02000580 RID: 1408
	public class WorldCameraConfig_ContinuousPan : WorldCameraConfig
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x000E6E50 File Offset: 0x000E5250
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
