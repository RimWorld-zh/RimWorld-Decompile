using System;

namespace RimWorld.Planet
{
	// Token: 0x0200057C RID: 1404
	public class WorldCameraConfig_ContinuousPan : WorldCameraConfig
	{
		// Token: 0x06001AD7 RID: 6871 RVA: 0x000E6EA4 File Offset: 0x000E52A4
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
