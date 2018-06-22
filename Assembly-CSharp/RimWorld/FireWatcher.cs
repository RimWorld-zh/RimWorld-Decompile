using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000385 RID: 901
	public class FireWatcher
	{
		// Token: 0x06000F9B RID: 3995 RVA: 0x00083C34 File Offset: 0x00082034
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x00083C50 File Offset: 0x00082050
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x00083C6C File Offset: 0x0008206C
		public bool LargeFireDangerPresent
		{
			get
			{
				if (this.fireDanger < 0f)
				{
					this.UpdateObservations();
				}
				return this.fireDanger > 90f;
			}
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00083CA4 File Offset: 0x000820A4
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00083CC4 File Offset: 0x000820C4
		private void UpdateObservations()
		{
			this.fireDanger = 0f;
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.Fire);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				this.fireDanger += 0.5f + fire.fireSize;
			}
		}

		// Token: 0x04000992 RID: 2450
		private Map map;

		// Token: 0x04000993 RID: 2451
		private float fireDanger = -1f;

		// Token: 0x04000994 RID: 2452
		private const int UpdateObservationsInterval = 426;

		// Token: 0x04000995 RID: 2453
		private const float BaseDangerPerFire = 0.5f;
	}
}
