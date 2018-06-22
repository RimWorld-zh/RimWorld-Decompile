using System;

namespace RimWorld.Planet
{
	// Token: 0x0200057D RID: 1405
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x06001AD8 RID: 6872 RVA: 0x000E6EE4 File Offset: 0x000E52E4
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
