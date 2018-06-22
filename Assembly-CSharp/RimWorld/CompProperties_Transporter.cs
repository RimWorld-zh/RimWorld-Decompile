using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000256 RID: 598
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x0005FE43 File Offset: 0x0005E243
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x040004B9 RID: 1209
		public float massCapacity = 150f;

		// Token: 0x040004BA RID: 1210
		public float restEffectiveness;
	}
}
