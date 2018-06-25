using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000258 RID: 600
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x040004B9 RID: 1209
		public float massCapacity = 150f;

		// Token: 0x040004BA RID: 1210
		public float restEffectiveness;

		// Token: 0x06000A97 RID: 2711 RVA: 0x0005FF93 File Offset: 0x0005E393
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}
	}
}
