using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000242 RID: 578
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x0400045E RID: 1118
		public float mag = 0.05f;

		// Token: 0x06000A6D RID: 2669 RVA: 0x0005E932 File Offset: 0x0005CD32
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}
	}
}
