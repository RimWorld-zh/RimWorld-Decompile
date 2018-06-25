using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_AllyAssistance : StorytellerComp
	{
		public StorytellerComp_AllyAssistance()
		{
		}

		private StorytellerCompProperties_AllyAssistance Props
		{
			get
			{
				return (StorytellerCompProperties_AllyAssistance)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map == null || map.dangerWatcher.DangerRating < StoryDanger.High)
			{
				yield break;
			}
			float mtb = this.Props.baseMtbDays * StorytellerUtility.AllyIncidentMTBMultiplier(false);
			if (mtb <= 0f || !Rand.MTBEventOccurs(mtb, 60000f, 1000f))
			{
				yield break;
			}
			IncidentDef incident;
			if (!base.UsableIncidentsInCategory(IncidentCategoryDefOf.AllyAssistance, target).TryRandomElementByWeight((IncidentDef d) => d.baseChance, out incident))
			{
				yield break;
			}
			yield return new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal Map <map>__0;

			internal float <mtb>__0;

			internal IncidentDef <incident>__0;

			internal StorytellerComp_AllyAssistance $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<IncidentDef, float> <>f__am$cache0;

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
					map = (target as Map);
					if (map != null && map.dangerWatcher.DangerRating >= StoryDanger.High)
					{
						mtb = base.Props.baseMtbDays * StorytellerUtility.AllyIncidentMTBMultiplier(false);
						if (mtb > 0f && Rand.MTBEventOccurs(mtb, 60000f, 1000f))
						{
							if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.AllyAssistance, target).TryRandomElementByWeight((IncidentDef d) => d.baseChance, out incident))
							{
								this.$current = new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								return true;
							}
						}
					}
					break;
				case 1u:
					this.$PC = -1;
					break;
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
				StorytellerComp_AllyAssistance.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_AllyAssistance.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}

			private static float <>m__0(IncidentDef d)
			{
				return d.baseChance;
			}
		}
	}
}
