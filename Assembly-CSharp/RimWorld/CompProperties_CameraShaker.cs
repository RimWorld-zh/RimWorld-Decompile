using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000240 RID: 576
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x0005E786 File Offset: 0x0005CB86
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		// Token: 0x04000460 RID: 1120
		public float mag = 0.05f;
	}
}
