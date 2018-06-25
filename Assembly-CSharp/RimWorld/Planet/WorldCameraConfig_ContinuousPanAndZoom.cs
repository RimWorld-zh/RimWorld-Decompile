using System;

namespace RimWorld.Planet
{
	// Token: 0x0200057F RID: 1407
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x06001ADC RID: 6876 RVA: 0x000E7034 File Offset: 0x000E5434
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
