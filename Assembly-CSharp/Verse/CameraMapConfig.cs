using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AE5 RID: 2789
	public abstract class CameraMapConfig
	{
		// Token: 0x04002724 RID: 10020
		public float dollyRateKeys = 50f;

		// Token: 0x04002725 RID: 10021
		public float dollyRateMouseDrag = 6.5f;

		// Token: 0x04002726 RID: 10022
		public float dollyRateScreenEdge = 35f;

		// Token: 0x04002727 RID: 10023
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x04002728 RID: 10024
		public float moveSpeedScale = 2f;

		// Token: 0x04002729 RID: 10025
		public float zoomSpeed = 2.6f;

		// Token: 0x0400272A RID: 10026
		public float zoomPreserveFactor;

		// Token: 0x0400272B RID: 10027
		public bool smoothZoom;

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0020ADF9 File Offset: 0x002091F9
		public virtual void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0020ADFC File Offset: 0x002091FC
		public virtual void ConfigOnGUI()
		{
		}
	}
}
