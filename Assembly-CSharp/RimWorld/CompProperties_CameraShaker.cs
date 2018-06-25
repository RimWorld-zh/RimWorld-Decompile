using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000242 RID: 578
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x04000460 RID: 1120
		public float mag = 0.05f;

		// Token: 0x06000A6C RID: 2668 RVA: 0x0005E92E File Offset: 0x0005CD2E
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}
	}
}
