using System;

namespace Verse
{
	// Token: 0x02000AEE RID: 2798
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06003DF3 RID: 15859 RVA: 0x0020ABC8 File Offset: 0x00208FC8
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
