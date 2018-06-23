using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000240 RID: 576
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x0400045E RID: 1118
		public float mag = 0.05f;

		// Token: 0x06000A69 RID: 2665 RVA: 0x0005E7E2 File Offset: 0x0005CBE2
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}
	}
}
