using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class IncidentQueue : IExposable
	{
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();

		[CompilerGenerated]
		private static Comparison<QueuedIncident> <>f__am$cache0;

		public IncidentQueue()
		{
		}

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
			foreach (QueuedIncident inc in this.queuedIncidents)
			{
				yield return inc;
			}
			yield break;
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
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
		{
			FiringIncident firingInc = new FiringIncident(def, null, parms);
			QueuedIncident qi = new QueuedIncident(firingInc, fireTick, retryDurationTicks);
			this.Add(qi);
			return true;
		}

		public void IncidentQueueTick()
		{
			for (int i = this.queuedIncidents.Count - 1; i >= 0; i--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[i];
				if (!queuedIncident.TriedToFire)
				{
					if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
					{
						bool flag = Find.Storyteller.TryFire(queuedIncident.FiringIncident);
						queuedIncident.Notify_TriedToFire();
						if (flag || queuedIncident.RetryDurationTicks == 0)
						{
							this.queuedIncidents.Remove(queuedIncident);
						}
					}
				}
				else if (queuedIncident.FireTick + queuedIncident.RetryDurationTicks <= Find.TickManager.TicksGame)
				{
					this.queuedIncidents.Remove(queuedIncident);
				}
				else if (Find.TickManager.TicksGame % 833 == Rand.RangeSeeded(0, 833, queuedIncident.FireTick))
				{
					bool flag2 = Find.Storyteller.TryFire(queuedIncident.FiringIncident);
					queuedIncident.Notify_TriedToFire();
					if (flag2)
					{
						this.queuedIncidents.Remove(queuedIncident);
					}
				}
			}
		}

		public void Notify_MapRemoved(Map map)
		{
			this.queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
		}

		[CompilerGenerated]
		private static int <Add>m__0(QueuedIncident a, QueuedIncident b)
		{
			return a.FireTick.CompareTo(b.FireTick);
		}

		[CompilerGenerated]
		private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal List<QueuedIncident>.Enumerator $locvar0;

			internal QueuedIncident <inc>__1;

			internal IncidentQueue $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetEnumerator>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = this.queuedIncidents.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						inc = enumerator.Current;
						this.$current = inc;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		private sealed class <Notify_MapRemoved>c__AnonStorey1
		{
			internal Map map;

			public <Notify_MapRemoved>c__AnonStorey1()
			{
			}

			internal bool <>m__0(QueuedIncident x)
			{
				return x.FiringIncident.parms.target == this.map;
			}
		}
	}
}
