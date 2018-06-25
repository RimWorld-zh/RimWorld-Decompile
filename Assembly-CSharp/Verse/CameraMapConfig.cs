using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AE7 RID: 2791
	public abstract class CameraMapConfig
	{
		// Token: 0x04002725 RID: 10021
		public float dollyRateKeys = 50f;

		// Token: 0x04002726 RID: 10022
		public float dollyRateMouseDrag = 6.5f;

		// Token: 0x04002727 RID: 10023
		public float dollyRateScreenEdge = 35f;

		// Token: 0x04002728 RID: 10024
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x04002729 RID: 10025
		public float moveSpeedScale = 2f;

		// Token: 0x0400272A RID: 10026
		public float zoomSpeed = 2.6f;

		// Token: 0x0400272B RID: 10027
		public float zoomPreserveFactor;

		// Token: 0x0400272C RID: 10028
		public bool smoothZoom;

		// Token: 0x06003DED RID: 15853 RVA: 0x0020AF25 File Offset: 0x00209325
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0020AF28 File Offset: 0x00209328
		public virtual void ConfigOnGUI()
		{
		}
	}
}
