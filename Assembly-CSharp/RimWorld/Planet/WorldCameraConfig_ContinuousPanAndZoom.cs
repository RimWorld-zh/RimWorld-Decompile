using System;

namespace RimWorld.Planet
{
	// Token: 0x02000581 RID: 1409
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x06001AE1 RID: 6881 RVA: 0x000E6E90 File Offset: 0x000E5290
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
