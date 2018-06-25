using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		private static List<Thing> tmpDrills = new List<Thing>();

		public StorytellerComp_DeepDrillInfestation()
		{
		}

		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				DifficultyDef difficulty = Find.Storyteller.difficulty;
				float result;
				if (difficulty.deepDrillInfestationChanceFactor <= 0f)
				{
					result = -1f;
				}
				else
				{
					result = this.Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
				}
				return result;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = (Map)target;
			StorytellerComp_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, StorytellerComp_DeepDrillInfestation.tmpDrills);
			if (!StorytellerComp_DeepDrillInfestation.tmpDrills.Any<Thing>())
			{
				yield break;
			}
			float mtb = this.DeepDrillInfestationMTBDaysPerDrill;
			for (int i = 0; i < StorytellerComp_DeepDrillInfestation.tmpDrills.Count; i++)
			{
				if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
				{
					IncidentDef def;
					if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, target).TryRandomElement(out def))
					{
						IncidentParms parms = this.GenerateParms(def.category, target);
						yield return new FiringIncident(def, this, parms);
					}
				}
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static StorytellerComp_DeepDrillInfestation()
		{
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal Map <map>__0;

			internal float <mtb>__0;

			internal int <i>__1;

			internal IncidentDef <def>__2;

			internal IncidentParms <parms>__3;

			internal StorytellerComp_DeepDrillInfestation $this;

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
					map = (Map)target;
					StorytellerComp_DeepDrillInfestation.tmpDrills.Clear();
					DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, StorytellerComp_DeepDrillInfestation.tmpDrills);
					if (!StorytellerComp_DeepDrillInfestation.tmpDrills.Any<Thing>())
					{
						return false;
					}
					mtb = base.DeepDrillInfestationMTBDaysPerDrill;
					i = 0;
					goto IL_125;
				case 1u:
					break;
				default:
					return false;
				}
				IL_115:
				IL_116:
				i++;
				IL_125:
				if (i >= StorytellerComp_DeepDrillInfestation.tmpDrills.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!Rand.MTBEventOccurs(mtb, 60000f, 1000f))
					{
						goto IL_116;
					}
					if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, target).TryRandomElement(out def))
					{
						parms = this.GenerateParms(def.category, target);
						this.$current = new FiringIncident(def, this, parms);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_115;
				}
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
				StorytellerComp_DeepDrillInfestation.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_DeepDrillInfestation.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}
		}
	}
}
