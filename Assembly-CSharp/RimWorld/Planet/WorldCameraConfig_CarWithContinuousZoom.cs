using System;

namespace RimWorld.Planet
{
	// Token: 0x02000583 RID: 1411
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x06001AE4 RID: 6884 RVA: 0x000E7008 File Offset: 0x000E5408
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
