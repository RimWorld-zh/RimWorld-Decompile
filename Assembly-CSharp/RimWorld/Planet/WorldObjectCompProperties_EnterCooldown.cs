using System;

namespace RimWorld.Planet
{
	// Token: 0x0200027E RID: 638
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x04000559 RID: 1369
		public bool autoStartOnMapRemoved = true;

		// Token: 0x0400055A RID: 1370
		public float durationDays = 4f;

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00062B89 File Offset: 0x00060F89
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}
	}
}
