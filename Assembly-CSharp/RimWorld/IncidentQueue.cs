using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031C RID: 796
	public class IncidentQueue : IExposable
	{
		// Token: 0x040008AF RID: 2223
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00073AA0 File Offset: 0x00071EA0
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00073AC0 File Offset: 0x00071EC0
		public string DebugQueueReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (QueuedIncident queuedIncident in this.queuedIncidents)
				{
					stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.FireTick - Find.TickManager.TicksGame).ToString() + " ticks)");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00073B6C File Offset: 0x00071F6C
		public IEnumerator GetEnumerator()
		{
			foreach (QueuedIncident inc in this.queuedIncidents)
			{
				yield return inc;
			}
			yield break;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00073B8E File Offset: 0x00071F8E
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00073B9C File Offset: 0x00071F9C
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, new object[0]);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00073BB8 File Offset: 0x00071FB8
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00073C04 File Offset: 0x00072004
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null)
		{
			FiringIncident firingInc = new FiringIncident(def, null, parms);
			QueuedIncident qi = new QueuedIncident(firingInc, fireTick);
			this.Add(qi);
			return true;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00073C34 File Offset: 0x00072034
		public void IncidentQueueTick()
		{
			for (int i = this.queuedIncidents.Count - 1; i >= 0; i--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[i];
				if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
				{
					Find.Storyteller.TryFire(queuedIncident.FiringIncident);
					this.queuedIncidents.Remove(queuedIncident);
				}
			}
		}
	}
}
