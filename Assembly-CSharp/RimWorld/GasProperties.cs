using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000264 RID: 612
	public class GasProperties
	{
		// Token: 0x040004CF RID: 1231
		public bool blockTurretTracking = false;

		// Token: 0x040004D0 RID: 1232
		public float accuracyPenalty = 0f;

		// Token: 0x040004D1 RID: 1233
		public FloatRange expireSeconds = new FloatRange(30f, 30f);

		// Token: 0x040004D2 RID: 1234
		public float rotationSpeed = 0f;
	}
}
