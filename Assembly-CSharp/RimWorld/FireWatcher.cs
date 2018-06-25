using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000387 RID: 903
	public class FireWatcher
	{
		// Token: 0x04000995 RID: 2453
		private Map map;

		// Token: 0x04000996 RID: 2454
		private float fireDanger = -1f;

		// Token: 0x04000997 RID: 2455
		private const int UpdateObservationsInterval = 426;

		// Token: 0x04000998 RID: 2456
		private const float BaseDangerPerFire = 0.5f;

		// Token: 0x06000F9E RID: 3998 RVA: 0x00083D94 File Offset: 0x00082194
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00083DB0 File Offset: 0x000821B0
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x00083DCC File Offset: 0x000821CC
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

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00083E04 File Offset: 0x00082204
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00083E24 File Offset: 0x00082224
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
	}
}
