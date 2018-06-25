using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000387 RID: 903
	public class FireWatcher
	{
		// Token: 0x04000992 RID: 2450
		private Map map;

		// Token: 0x04000993 RID: 2451
		private float fireDanger = -1f;

		// Token: 0x04000994 RID: 2452
		private const int UpdateObservationsInterval = 426;

		// Token: 0x04000995 RID: 2453
		private const float BaseDangerPerFire = 0.5f;

		// Token: 0x06000F9F RID: 3999 RVA: 0x00083D84 File Offset: 0x00082184
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x00083DA0 File Offset: 0x000821A0
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00083DBC File Offset: 0x000821BC
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

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00083DF4 File Offset: 0x000821F4
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00083E14 File Offset: 0x00082214
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
