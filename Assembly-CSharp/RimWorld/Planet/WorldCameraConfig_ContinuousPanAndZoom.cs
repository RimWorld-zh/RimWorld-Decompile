using System;

namespace RimWorld.Planet
{
	// Token: 0x02000581 RID: 1409
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x000E6E24 File Offset: 0x000E5224
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
