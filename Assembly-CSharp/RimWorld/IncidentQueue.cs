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
				List<QueuedIncident>.Enumerator enumerator = this.queuedIncidents.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						QueuedIncident current = enumerator.Current;
						stringBuilder.AppendLine(current.ToString() + " (in " + (current.FireTick - Find.TickManager.TicksGame).ToString() + " ticks)");
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return stringBuilder.ToString();
			}
		}

		public IEnumerator GetEnumerator()
		{
			List<QueuedIncident>.Enumerator enumerator = this.queuedIncidents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					QueuedIncident inc = enumerator.Current;
					yield return (object)inc;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
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
