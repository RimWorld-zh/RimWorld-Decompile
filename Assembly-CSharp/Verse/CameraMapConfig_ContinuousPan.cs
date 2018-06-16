using System;

namespace Verse
{
	// Token: 0x02000AEB RID: 2795
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x06003DEF RID: 15855 RVA: 0x0020AA10 File Offset: 0x00208E10
		public CameraMapConfig_ContinuousPan()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateMouseDrag = 4f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}
	}
}
