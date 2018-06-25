using System;

namespace RimWorld.Planet
{
	// Token: 0x0200057E RID: 1406
	public class WorldCameraConfig_ContinuousPan : WorldCameraConfig
	{
		// Token: 0x06001ADB RID: 6875 RVA: 0x000E6FF4 File Offset: 0x000E53F4
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
