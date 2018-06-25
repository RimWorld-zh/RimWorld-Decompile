using System;

namespace RimWorld.Planet
{
	// Token: 0x02000581 RID: 1409
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x06001ADE RID: 6878 RVA: 0x000E7414 File Offset: 0x000E5814
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
