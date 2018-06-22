using System;

namespace Verse
{
	// Token: 0x02000AE8 RID: 2792
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06003DED RID: 15853 RVA: 0x0020AE48 File Offset: 0x00209248
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
