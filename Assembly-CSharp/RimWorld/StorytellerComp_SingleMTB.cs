using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		public StorytellerComp_SingleMTB()
		{
		}

		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal IncidentParms <parms>__1;

			internal StorytellerComp_SingleMTB $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeIntervalIncidents>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (!base.Props.incident.TargetAllowed(target))
					{
						return false;
					}
					if (Rand.MTBEventOccurs(base.Props.mtbDays, 60000f, 1000f))
					{
						parms = this.GenerateParms(base.Props.incident.category, target);
						if (base.Props.incident.Worker.CanFireNow(parms))
						{
							this.$current = new FiringIncident(base.Props.incident, this, parms);
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			FiringIncident IEnumerator<FiringIncident>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.FiringIncident>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}
		}
	}
}
