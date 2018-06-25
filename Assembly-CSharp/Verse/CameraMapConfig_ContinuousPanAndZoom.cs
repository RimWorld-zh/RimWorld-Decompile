using System;

namespace Verse
{
	// Token: 0x02000AEB RID: 2795
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06003DF1 RID: 15857 RVA: 0x0020B254 File Offset: 0x00209654
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
