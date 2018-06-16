using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000385 RID: 901
	public class FireWatcher
	{
		// Token: 0x06000F9B RID: 3995 RVA: 0x00083A48 File Offset: 0x00081E48
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x00083A64 File Offset: 0x00081E64
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x00083A80 File Offset: 0x00081E80
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

		// Token: 0x06000F9E RID: 3998 RVA: 0x00083AB8 File Offset: 0x00081EB8
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00083AD8 File Offset: 0x00081ED8
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

		// Token: 0x04000990 RID: 2448
		private Map map;

		// Token: 0x04000991 RID: 2449
		private float fireDanger = -1f;

		// Token: 0x04000992 RID: 2450
		private const int UpdateObservationsInterval = 426;

		// Token: 0x04000993 RID: 2451
		private const float BaseDangerPerFire = 0.5f;
	}
}
