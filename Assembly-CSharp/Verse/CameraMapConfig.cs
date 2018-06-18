using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AE9 RID: 2793
	public abstract class CameraMapConfig
	{
		// Token: 0x06003DEE RID: 15854 RVA: 0x0020AAD5 File Offset: 0x00208ED5
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x0020AAD8 File Offset: 0x00208ED8
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x04002729 RID: 10025
		public float dollyRateKeys = 50f;

		// Token: 0x0400272A RID: 10026
		public float dollyRateMouseDrag = 6.5f;

		// Token: 0x0400272B RID: 10027
		public float dollyRateScreenEdge = 35f;

		// Token: 0x0400272C RID: 10028
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x0400272D RID: 10029
		public float moveSpeedScale = 2f;

		// Token: 0x0400272E RID: 10030
		public float zoomSpeed = 2.6f;

		// Token: 0x0400272F RID: 10031
		public float zoomPreserveFactor;

		// Token: 0x04002730 RID: 10032
		public bool smoothZoom;
	}
}
