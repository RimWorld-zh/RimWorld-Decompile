using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200057E RID: 1406
	public abstract class WorldCameraConfig
	{
		// Token: 0x06001ADD RID: 6877 RVA: 0x000E6E41 File Offset: 0x000E5241
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000E6E44 File Offset: 0x000E5244
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x04000FAC RID: 4012
		public float dollyRateKeys = 170f;

		// Token: 0x04000FAD RID: 4013
		public float dollyRateMouseDrag = 25f;

		// Token: 0x04000FAE RID: 4014
		public float dollyRateScreenEdge = 125f;

		// Token: 0x04000FAF RID: 4015
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x04000FB0 RID: 4016
		public float rotationSpeedScale = 0.3f;

		// Token: 0x04000FB1 RID: 4017
		public float zoomSpeed = 2.6f;

		// Token: 0x04000FB2 RID: 4018
		public float zoomPreserveFactor;

		// Token: 0x04000FB3 RID: 4019
		public bool smoothZoom;
	}
}
