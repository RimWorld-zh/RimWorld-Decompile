using System;

namespace Verse
{
	// Token: 0x02000AE7 RID: 2791
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x06003DEC RID: 15852 RVA: 0x0020AE08 File Offset: 0x00209208
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
