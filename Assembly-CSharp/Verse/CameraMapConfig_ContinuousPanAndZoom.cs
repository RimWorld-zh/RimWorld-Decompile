using System;

namespace Verse
{
	// Token: 0x02000AEC RID: 2796
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06003DF2 RID: 15858 RVA: 0x0020AB24 File Offset: 0x00208F24
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
