using System;

namespace Verse
{
	// Token: 0x02000AEC RID: 2796
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06003DF0 RID: 15856 RVA: 0x0020AA50 File Offset: 0x00208E50
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
