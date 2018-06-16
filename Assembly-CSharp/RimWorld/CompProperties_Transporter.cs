using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000256 RID: 598
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x0005FDE7 File Offset: 0x0005E1E7
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x040004BB RID: 1211
		public float massCapacity = 150f;

		// Token: 0x040004BC RID: 1212
		public float restEffectiveness;
	}
}
