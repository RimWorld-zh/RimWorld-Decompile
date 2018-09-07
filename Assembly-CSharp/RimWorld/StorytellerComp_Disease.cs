using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_Disease : StorytellerComp
	{
		public StorytellerComp_Disease()
		{
		}

		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!DebugSettings.enableRandomDiseases)
			{
				yield break;
			}
			if (target.Tile == -1)
			{
				yield break;
			}
			BiomeDef biome = Find.WorldGrid[target.Tile].biome;
			float mtb = biome.diseaseMtbDays;
			mtb *= Find.Storyteller.difficulty.diseaseIntervalFactor;
			IncidentDef inc;
			if (Rand.MTBEventOccurs(mtb, 60000f, 1000f) && base.UsableIncidentsInCategory(this.Props.category, target).TryRandomElementByWeight((IncidentDef d) => biome.CommonalityOfDisease(d), out inc))
			{
				yield return new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
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
			internal IIncidentTarget target;

			internal float <mtb>__0;

			internal IncidentDef <inc>__1;

			internal StorytellerComp_Disease $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			private StorytellerComp_Disease.<MakeIntervalIncidents>c__Iterator0.<MakeIntervalIncidents>c__AnonStorey1 $locvar0;

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
				{
					if (!DebugSettings.enableRandomDiseases)
					{
						return false;
					}
					if (target.Tile == -1)
					{
						return false;
					}
					BiomeDef biome = Find.WorldGrid[target.Tile].biome;
					mtb = biome.diseaseMtbDays;
					mtb *= Find.Storyteller.difficulty.diseaseIntervalFactor;
					if (Rand.MTBEventOccurs(mtb, 60000f, 1000f) && base.UsableIncidentsInCategory(base.Props.category, target).TryRandomElementByWeight((IncidentDef d) => biome.CommonalityOfDisease(d), out inc))
					{
						this.$current = new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				}
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
				StorytellerComp_Disease.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_Disease.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}

			private sealed class <MakeIntervalIncidents>c__AnonStorey1
			{
				internal BiomeDef biome;

				internal StorytellerComp_Disease.<MakeIntervalIncidents>c__Iterator0 <>f__ref$0;

				public <MakeIntervalIncidents>c__AnonStorey1()
				{
				}

				internal float <>m__0(IncidentDef d)
				{
					return this.biome.CommonalityOfDisease(d);
				}
			}
		}
	}
}
