using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AE8 RID: 2792
	public abstract class CameraMapConfig
	{
		// Token: 0x0400272C RID: 10028
		public float dollyRateKeys = 50f;

		// Token: 0x0400272D RID: 10029
		public float dollyRateMouseDrag = 6.5f;

		// Token: 0x0400272E RID: 10030
		public float dollyRateScreenEdge = 35f;

		// Token: 0x0400272F RID: 10031
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x04002730 RID: 10032
		public float moveSpeedScale = 2f;

		// Token: 0x04002731 RID: 10033
		public float zoomSpeed = 2.6f;

		// Token: 0x04002732 RID: 10034
		public float zoomPreserveFactor;

		// Token: 0x04002733 RID: 10035
		public bool smoothZoom;

		// Token: 0x06003DED RID: 15853 RVA: 0x0020B205 File Offset: 0x00209605
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0020B208 File Offset: 0x00209608
		public virtual void ConfigOnGUI()
		{
		}
	}
}
