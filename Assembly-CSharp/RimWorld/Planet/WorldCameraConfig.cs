using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200057C RID: 1404
	public abstract class WorldCameraConfig
	{
		// Token: 0x04000FAD RID: 4013
		public float dollyRateKeys = 170f;

		// Token: 0x04000FAE RID: 4014
		public float dollyRateMouseDrag = 25f;

		// Token: 0x04000FAF RID: 4015
		public float dollyRateScreenEdge = 125f;

		// Token: 0x04000FB0 RID: 4016
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x04000FB1 RID: 4017
		public float rotationSpeedScale = 0.3f;

		// Token: 0x04000FB2 RID: 4018
		public float zoomSpeed = 2.6f;

		// Token: 0x04000FB3 RID: 4019
		public float zoomPreserveFactor;

		// Token: 0x04000FB4 RID: 4020
		public bool smoothZoom;

		// Token: 0x06001AD7 RID: 6871 RVA: 0x000E724D File Offset: 0x000E564D
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000E7250 File Offset: 0x000E5650
		public virtual void ConfigOnGUI()
		{
		}
	}
}
