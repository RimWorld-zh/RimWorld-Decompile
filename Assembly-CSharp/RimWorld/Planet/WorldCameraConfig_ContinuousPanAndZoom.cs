using System;

namespace RimWorld.Planet
{
	// Token: 0x0200057F RID: 1407
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x06001ADB RID: 6875 RVA: 0x000E729C File Offset: 0x000E569C
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
