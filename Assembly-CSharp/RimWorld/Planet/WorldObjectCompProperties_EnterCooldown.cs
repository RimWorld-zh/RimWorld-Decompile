using System;

namespace RimWorld.Planet
{
	// Token: 0x0200027E RID: 638
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x0400055B RID: 1371
		public bool autoStartOnMapRemoved = true;

		// Token: 0x0400055C RID: 1372
		public float durationDays = 4f;

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00062B85 File Offset: 0x00060F85
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}
	}
}
