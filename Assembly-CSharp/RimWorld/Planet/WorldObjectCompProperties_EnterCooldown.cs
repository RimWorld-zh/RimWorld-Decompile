using System;

namespace RimWorld.Planet
{
	// Token: 0x0200027C RID: 636
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x06000AE6 RID: 2790 RVA: 0x000629DD File Offset: 0x00060DDD
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x0400055B RID: 1371
		public bool autoStartOnMapRemoved = true;

		// Token: 0x0400055C RID: 1372
		public float durationDays = 4f;
	}
}
