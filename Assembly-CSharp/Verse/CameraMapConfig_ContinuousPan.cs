using System;

namespace Verse
{
	// Token: 0x02000AEB RID: 2795
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x06003DF1 RID: 15857 RVA: 0x0020AAE4 File Offset: 0x00208EE4
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
