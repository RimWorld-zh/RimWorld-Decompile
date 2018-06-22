using System;

namespace Verse
{
	// Token: 0x02000AEA RID: 2794
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06003DF0 RID: 15856 RVA: 0x0020AFC0 File Offset: 0x002093C0
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
