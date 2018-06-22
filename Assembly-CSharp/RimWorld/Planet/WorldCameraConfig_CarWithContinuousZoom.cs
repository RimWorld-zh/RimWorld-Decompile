using System;

namespace RimWorld.Planet
{
	// Token: 0x0200057F RID: 1407
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x06001ADB RID: 6875 RVA: 0x000E705C File Offset: 0x000E545C
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
