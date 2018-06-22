using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200057A RID: 1402
	public abstract class WorldCameraConfig
	{
		// Token: 0x06001AD4 RID: 6868 RVA: 0x000E6E95 File Offset: 0x000E5295
		public virtual void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000E6E98 File Offset: 0x000E5298
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x04000FA9 RID: 4009
		public float dollyRateKeys = 170f;

		// Token: 0x04000FAA RID: 4010
		public float dollyRateMouseDrag = 25f;

		// Token: 0x04000FAB RID: 4011
		public float dollyRateScreenEdge = 125f;

		// Token: 0x04000FAC RID: 4012
		public float camRotationDecayFactor = 0.9f;

		// Token: 0x04000FAD RID: 4013
		public float rotationSpeedScale = 0.3f;

		// Token: 0x04000FAE RID: 4014
		public float zoomSpeed = 2.6f;

		// Token: 0x04000FAF RID: 4015
		public float zoomPreserveFactor;

		// Token: 0x04000FB0 RID: 4016
		public bool smoothZoom;
	}
}
