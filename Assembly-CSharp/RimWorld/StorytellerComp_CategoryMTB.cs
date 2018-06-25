using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		public StorytellerComp_CategoryMTB()
		{
		}

		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float mtbNow = this.Props.mtbDays;
			if (this.Props.mtbDaysFactorByDaysPassedCurve != null)
			{
				mtbNow *= this.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (Rand.MTBEventOccurs(mtbNow, 60000f, 1000f))
			{
				IncidentDef selectedDef;
				if (base.UsableIncidentsInCategory(this.Props.category, target).TryRandomElementByWeight((IncidentDef incDef) => base.IncidentChanceFinal(incDef), out selectedDef))
				{
					yield return new FiringIncident(selectedDef, this, this.GenerateParms(selectedDef.category, target));
				}
			}
			yield break;
		}

		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal float <mtbNow>__0;

			internal IIncidentTarget target;

			internal IncidentDef <selectedDef>__1;

			internal StorytellerComp_CategoryMTB $this;

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
					mtbNow = base.Props.mtbDays;
					if (base.Props.mtbDaysFactorByDaysPassedCurve != null)
					{
						mtbNow *= base.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
					}
					if (Rand.MTBEventOccurs(mtbNow, 60000f, 1000f))
					{
						if (base.UsableIncidentsInCategory(base.Props.category, target).TryRandomElementByWeight((IncidentDef incDef) => base.IncidentChanceFinal(incDef), out selectedDef))
						{
							this.$current = new FiringIncident(selectedDef, this, this.GenerateParms(selectedDef.category, target));
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
				StorytellerComp_CategoryMTB.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_CategoryMTB.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}

			internal float <>m__0(IncidentDef incDef)
			{
				return base.IncidentChanceFinal(incDef);
			}
		}
	}
}
