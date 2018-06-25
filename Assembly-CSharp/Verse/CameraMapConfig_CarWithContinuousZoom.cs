using System;

namespace Verse
{
	// Token: 0x02000AED RID: 2797
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06003DF4 RID: 15860 RVA: 0x0020B3CC File Offset: 0x002097CC
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
