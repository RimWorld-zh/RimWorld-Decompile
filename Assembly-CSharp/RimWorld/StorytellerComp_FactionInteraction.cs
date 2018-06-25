using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		private const int ForceChooseTraderAfterTicks = 780000;

		[CompilerGenerated]
		private static Func<IncidentDef, float> <>f__am$cache0;

		public StorytellerComp_FactionInteraction()
		{
		}

		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float mtb = this.Props.baseMtbDays * StorytellerUtility.AllyIncidentMTBMultiplier(true);
			if (mtb < 0f)
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
			{
				IncidentDef incDef;
				if (this.TryChooseIncident(target, out incDef))
				{
					yield return new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
				}
			}
			yield break;
		}

		private bool TryChooseIncident(IIncidentTarget target, out IncidentDef result)
		{
			if (IncidentDefOf.TraderCaravanArrival.TargetAllowed(target))
			{
				int num = 0;
				if (!target.StoryState.lastFireTicks.TryGetValue(IncidentDefOf.TraderCaravanArrival, out num))
				{
					num = (int)(this.props.minDaysPassed * 60000f);
				}
				if (Find.TickManager.TicksGame > num + 780000)
				{
					result = IncidentDefOf.TraderCaravanArrival;
					return true;
				}
			}
			return base.UsableIncidentsInCategory(IncidentCategoryDefOf.FactionArrival, target).TryRandomElementByWeight((IncidentDef d) => d.baseChance, out result);
		}

		[CompilerGenerated]
		private static float <TryChooseIncident>m__0(IncidentDef d)
		{
			return d.baseChance;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal float <mtb>__0;

			internal IIncidentTarget target;

			internal IncidentDef <incDef>__1;

			internal StorytellerComp_FactionInteraction $this;

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
					mtb = base.Props.baseMtbDays * StorytellerUtility.AllyIncidentMTBMultiplier(true);
					if (mtb < 0f)
					{
						return false;
					}
					if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
					{
						if (base.TryChooseIncident(target, out incDef))
						{
							this.$current = new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
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
				StorytellerComp_FactionInteraction.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_FactionInteraction.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}
		}
	}
}
