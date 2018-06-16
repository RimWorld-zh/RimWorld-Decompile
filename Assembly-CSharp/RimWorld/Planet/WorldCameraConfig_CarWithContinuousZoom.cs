using System;

namespace RimWorld.Planet
{
	// Token: 0x02000583 RID: 1411
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x06001AE3 RID: 6883 RVA: 0x000E6F9C File Offset: 0x000E539C
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
