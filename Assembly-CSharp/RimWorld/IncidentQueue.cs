using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031A RID: 794
	public class IncidentQueue : IExposable
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00073948 File Offset: 0x00071D48
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00073968 File Offset: 0x00071D68
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

		// Token: 0x06000D79 RID: 3449 RVA: 0x00073A14 File Offset: 0x00071E14
		public IEnumerator GetEnumerator()
		{
			foreach (QueuedIncident inc in this.queuedIncidents)
			{
				yield return inc;
			}
			yield break;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00073A36 File Offset: 0x00071E36
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00073A44 File Offset: 0x00071E44
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, new object[0]);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00073A60 File Offset: 0x00071E60
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00073AAC File Offset: 0x00071EAC
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null)
		{
			FiringIncident firingInc = new FiringIncident(def, null, parms);
			QueuedIncident qi = new QueuedIncident(firingInc, fireTick);
			this.Add(qi);
			return true;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00073ADC File Offset: 0x00071EDC
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

		// Token: 0x040008AC RID: 2220
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();
	}
}
