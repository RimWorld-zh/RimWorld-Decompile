using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_ClassicIntro : StorytellerComp
	{
		public StorytellerComp_ClassicIntro()
		{
		}

		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target != Find.Maps.Find((Map x) => x.IsPlayerHome))
			{
				yield break;
			}
			if (this.IntervalsPassed == 150)
			{
				IncidentDef inc = IncidentDefOf.VisitorGroup;
				if (inc.TargetAllowed(target))
				{
					yield return new FiringIncident(inc, this, null)
					{
						parms = 
						{
							target = target,
							points = (float)Rand.Range(40, 100)
						}
					};
				}
			}
			if (this.IntervalsPassed == 204)
			{
				IncidentCategoryDef threatCategory = (!Find.Storyteller.difficulty.allowIntroThreats) ? IncidentCategoryDefOf.Misc : IncidentCategoryDefOf.ThreatSmall;
				IncidentDef incDef;
				if ((from def in DefDatabase<IncidentDef>.AllDefs
				where def.TargetAllowed(target) && def.category == threatCategory
				select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
				{
					yield return new FiringIncident(incDef, this, null)
					{
						parms = StorytellerUtility.DefaultParmsNow(incDef.category, target)
					};
				}
			}
			if (this.IntervalsPassed == 264)
			{
				IncidentDef incDef2;
				if ((from def in DefDatabase<IncidentDef>.AllDefs
				where def.TargetAllowed(target) && def.category == IncidentCategoryDefOf.Misc
				select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef2))
				{
					yield return new FiringIncident(incDef2, this, null)
					{
						parms = StorytellerUtility.DefaultParmsNow(incDef2.category, target)
					};
				}
			}
			if (this.IntervalsPassed == 324)
			{
				IncidentDef inc2 = IncidentDefOf.RaidEnemy;
				if (!Find.Storyteller.difficulty.allowIntroThreats)
				{
					inc2 = (from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(target) && def.category == IncidentCategoryDefOf.Misc
					select def).RandomElementByWeightWithFallback(new Func<IncidentDef, float>(base.IncidentChanceFinal), null);
				}
				if (inc2 != null && inc2.TargetAllowed(target))
				{
					yield return new FiringIncident(inc2, this, null)
					{
						parms = this.GenerateParms(inc2.category, target)
					};
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal IncidentDef <inc>__1;

			internal FiringIncident <qi>__2;

			internal IncidentDef <incDef>__3;

			internal FiringIncident <qi>__4;

			internal IncidentDef <incDef>__5;

			internal FiringIncident <qi>__6;

			internal IncidentDef <inc>__7;

			internal FiringIncident <qi>__8;

			internal StorytellerComp_ClassicIntro $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			private StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0.<MakeIntervalIncidents>c__AnonStorey1 $locvar0;

			private static Predicate<Map> <>f__am$cache0;

			private StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0.<MakeIntervalIncidents>c__AnonStorey2 $locvar1;

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
					if (target != Find.Maps.Find((Map x) => x.IsPlayerHome))
					{
						return false;
					}
					if (base.IntervalsPassed == 150)
					{
						inc = IncidentDefOf.VisitorGroup;
						if (inc.TargetAllowed(target))
						{
							FiringIncident qi = new FiringIncident(inc, this, null);
							qi.parms.target = target;
							qi.parms.points = (float)Rand.Range(40, 100);
							this.$current = qi;
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
				case 2u:
					goto IL_247;
				case 3u:
					goto IL_2FA;
				case 4u:
					goto IL_3F4;
				default:
					return false;
				}
				if (base.IntervalsPassed == 204)
				{
					IncidentCategoryDef threatCategory = (!Find.Storyteller.difficulty.allowIntroThreats) ? IncidentCategoryDefOf.Misc : IncidentCategoryDefOf.ThreatSmall;
					if ((from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(<MakeIntervalIncidents>c__AnonStorey.target) && def.category == threatCategory
					select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
					{
						FiringIncident qi2 = new FiringIncident(incDef, this, null);
						qi2.parms = StorytellerUtility.DefaultParmsNow(incDef.category, <MakeIntervalIncidents>c__AnonStorey.target);
						this.$current = qi2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
				}
				IL_247:
				if (base.IntervalsPassed == 264)
				{
					if ((from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(<MakeIntervalIncidents>c__AnonStorey.target) && def.category == IncidentCategoryDefOf.Misc
					select def).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef2))
					{
						FiringIncident qi3 = new FiringIncident(incDef2, this, null);
						qi3.parms = StorytellerUtility.DefaultParmsNow(incDef2.category, <MakeIntervalIncidents>c__AnonStorey.target);
						this.$current = qi3;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
				}
				IL_2FA:
				if (base.IntervalsPassed == 324)
				{
					inc2 = IncidentDefOf.RaidEnemy;
					if (!Find.Storyteller.difficulty.allowIntroThreats)
					{
						inc2 = (from def in DefDatabase<IncidentDef>.AllDefs
						where def.TargetAllowed(<MakeIntervalIncidents>c__AnonStorey.target) && def.category == IncidentCategoryDefOf.Misc
						select def).RandomElementByWeightWithFallback(new Func<IncidentDef, float>(base.IncidentChanceFinal), null);
					}
					if (inc2 != null && inc2.TargetAllowed(<MakeIntervalIncidents>c__AnonStorey.target))
					{
						FiringIncident qi4 = new FiringIncident(inc2, this, null);
						qi4.parms = this.GenerateParms(inc2.category, <MakeIntervalIncidents>c__AnonStorey.target);
						this.$current = qi4;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
				}
				IL_3F4:
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
				StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}

			private static bool <>m__0(Map x)
			{
				return x.IsPlayerHome;
			}

			private sealed class <MakeIntervalIncidents>c__AnonStorey1
			{
				internal IIncidentTarget target;

				internal StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0 <>f__ref$0;

				public <MakeIntervalIncidents>c__AnonStorey1()
				{
				}

				internal bool <>m__0(IncidentDef def)
				{
					return def.TargetAllowed(this.target) && def.category == IncidentCategoryDefOf.Misc;
				}

				internal bool <>m__1(IncidentDef def)
				{
					return def.TargetAllowed(this.target) && def.category == IncidentCategoryDefOf.Misc;
				}
			}

			private sealed class <MakeIntervalIncidents>c__AnonStorey2
			{
				internal IncidentCategoryDef threatCategory;

				internal StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0 <>f__ref$0;

				internal StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__Iterator0.<MakeIntervalIncidents>c__AnonStorey1 <>f__ref$1;

				public <MakeIntervalIncidents>c__AnonStorey2()
				{
				}

				internal bool <>m__0(IncidentDef def)
				{
					return def.TargetAllowed(this.<>f__ref$1.target) && def.category == this.threatCategory;
				}
			}
		}
	}
}
