using System;

namespace Verse
{
	// Token: 0x02000AEC RID: 2796
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06003DF4 RID: 15860 RVA: 0x0020B0EC File Offset: 0x002094EC
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
