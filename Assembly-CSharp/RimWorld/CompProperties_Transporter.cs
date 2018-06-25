using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000258 RID: 600
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x040004BB RID: 1211
		public float massCapacity = 150f;

		// Token: 0x040004BC RID: 1212
		public float restEffectiveness;

		// Token: 0x06000A96 RID: 2710 RVA: 0x0005FF8F File Offset: 0x0005E38F
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}
	}
}
