using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class IncidentQueue : IExposable
	{
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();

		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

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

		public IEnumerator GetEnumerator()
		{
			using (List<QueuedIncident>.Enumerator enumerator = this.queuedIncidents.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					QueuedIncident inc = enumerator.Current;
					yield return (object)inc;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00b8:
			/*Error near IL_00b9: Unexpected return in MoveNext()*/;
		}

		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, new object[0]);
		}

		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((Comparison<QueuedIncident>)((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick)));
			return true;
		}

		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null)
		{
			FiringIncident firingInc = new FiringIncident(def, null, parms);
			QueuedIncident qi = new QueuedIncident(firingInc, fireTick);
			this.Add(qi);
			return true;
		}

		public void IncidentQueueTick()
		{
			for (int num = this.queuedIncidents.Count - 1; num >= 0; num--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[num];
				if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
				{
					Find.Storyteller.TryFire(queuedIncident.FiringIncident);
					this.queuedIncidents.Remove(queuedIncident);
				}
			}
		}
	}
}
