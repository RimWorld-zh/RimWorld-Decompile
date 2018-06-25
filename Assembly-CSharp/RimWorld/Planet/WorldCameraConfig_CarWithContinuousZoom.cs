using System;

namespace RimWorld.Planet
{
	// Token: 0x02000581 RID: 1409
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x06001ADF RID: 6879 RVA: 0x000E71AC File Offset: 0x000E55AC
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
