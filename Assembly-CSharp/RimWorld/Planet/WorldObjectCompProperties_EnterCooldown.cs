using System;

namespace RimWorld.Planet
{
	// Token: 0x0200027C RID: 636
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x06000AE4 RID: 2788 RVA: 0x00062A39 File Offset: 0x00060E39
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x04000559 RID: 1369
		public bool autoStartOnMapRemoved = true;

		// Token: 0x0400055A RID: 1370
		public float durationDays = 4f;
	}
}
