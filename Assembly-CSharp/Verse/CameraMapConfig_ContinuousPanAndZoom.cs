using System;

namespace Verse
{
	// Token: 0x02000AEA RID: 2794
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06003DF1 RID: 15857 RVA: 0x0020AF74 File Offset: 0x00209374
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
