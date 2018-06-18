using System;

namespace Verse
{
	// Token: 0x02000AEE RID: 2798
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06003DF5 RID: 15861 RVA: 0x0020AC9C File Offset: 0x0020909C
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
