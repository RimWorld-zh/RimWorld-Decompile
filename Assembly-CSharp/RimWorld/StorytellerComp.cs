using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class StorytellerComp
	{
		public StorytellerCompProperties props;

		[CompilerGenerated]
		private static Func<IncidentTargetTagDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache5;

		protected StorytellerComp()
		{
		}

		public virtual IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			yield break;
		}

		public virtual void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
		}

		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IIncidentTarget target)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => this.GenerateParms(cat, target));
		}

		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parms);
		}

		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(parmsGetter(x), false)
			select x;
		}

		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		protected float IncidentChanceFactor_PopulationIntent(IncidentDef def)
		{
			if (def.populationEffect == IncidentPopulationEffect.None)
			{
				return 1f;
			}
			float num;
			switch (def.populationEffect)
			{
			case IncidentPopulationEffect.IncreaseHard:
				num = 0.4f;
				break;
			case IncidentPopulationEffect.IncreaseMedium:
				num = 0f;
				break;
			case IncidentPopulationEffect.IncreaseEasy:
				num = -0.4f;
				break;
			default:
				throw new Exception();
			}
			float a = StorytellerUtilityPopulation.PopulationIntent + num;
			return Mathf.Max(a, this.props.minIncChancePopulationIntentFactor);
		}

		protected float IncidentChanceFinal(IncidentDef def)
		{
			float num = def.Worker.AdjustedChance;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, num);
		}

		public override string ToString()
		{
			string text = base.GetType().Name;
			string text2 = typeof(StorytellerComp).Name + "_";
			if (text.StartsWith(text2))
			{
				text = text.Substring(text2.Length);
			}
			if (!this.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>())
			{
				text = text + " (" + (from x in this.props.allowedTargetTags
				select x.ToString()).ToCommaList(false) + ")";
			}
			return text;
		}

		public virtual void DebugTablesIncidentChances(IncidentCategoryDef cat)
		{
			IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			where d.category == cat
			orderby this.IncidentChanceFinal(d) descending
			select d;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[10];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("baseChance", (IncidentDef d) => d.baseChance.ToString());
			array[2] = new TableDataGetter<IncidentDef>("AdjustedChance", (IncidentDef d) => d.Worker.AdjustedChance.ToString());
			array[3] = new TableDataGetter<IncidentDef>("Factor-PopCurrent", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			array[4] = new TableDataGetter<IncidentDef>("Factor-PopIntent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			array[5] = new TableDataGetter<IncidentDef>("final chance", (IncidentDef d) => this.IncidentChanceFinal(d).ToString());
			array[6] = new TableDataGetter<IncidentDef>("vismap-usable", (IncidentDef d) => (Find.CurrentMap != null) ? ((!this.UsableIncidentsInCategory(cat, Find.CurrentMap).Contains(d)) ? string.Empty : "V") : "-");
			array[7] = new TableDataGetter<IncidentDef>("world-usable", (IncidentDef d) => (!this.UsableIncidentsInCategory(cat, Find.World).Contains(d)) ? string.Empty : "W");
			array[8] = new TableDataGetter<IncidentDef>("pop-current", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString());
			array[9] = new TableDataGetter<IncidentDef>("pop-intent", (IncidentDef d) => StorytellerUtilityPopulation.PopulationIntent.ToString("F3"));
			DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}

		[CompilerGenerated]
		private static string <ToString>m__0(IncidentTargetTagDef x)
		{
			return x.ToString();
		}

		[CompilerGenerated]
		private static string <DebugTablesIncidentChances>m__1(IncidentDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <DebugTablesIncidentChances>m__2(IncidentDef d)
		{
			return d.baseChance.ToString();
		}

		[CompilerGenerated]
		private static string <DebugTablesIncidentChances>m__3(IncidentDef d)
		{
			return d.Worker.AdjustedChance.ToString();
		}

		[CompilerGenerated]
		private static string <DebugTablesIncidentChances>m__4(IncidentDef d)
		{
			return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString();
		}

		[CompilerGenerated]
		private static string <DebugTablesIncidentChances>m__5(IncidentDef d)
		{
			return StorytellerUtilityPopulation.PopulationIntent.ToString("F3");
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeIntervalIncidents>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
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
				return new StorytellerComp.<MakeIntervalIncidents>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <UsableIncidentsInCategory>c__AnonStorey1
		{
			internal IncidentCategoryDef cat;

			internal IIncidentTarget target;

			internal StorytellerComp $this;

			public <UsableIncidentsInCategory>c__AnonStorey1()
			{
			}

			internal IncidentParms <>m__0(IncidentDef x)
			{
				return this.$this.GenerateParms(this.cat, this.target);
			}
		}

		[CompilerGenerated]
		private sealed class <UsableIncidentsInCategory>c__AnonStorey2
		{
			internal IncidentParms parms;

			public <UsableIncidentsInCategory>c__AnonStorey2()
			{
			}

			internal IncidentParms <>m__0(IncidentDef x)
			{
				return this.parms;
			}
		}

		[CompilerGenerated]
		private sealed class <UsableIncidentsInCategory>c__AnonStorey3
		{
			internal IncidentCategoryDef cat;

			internal Func<IncidentDef, IncidentParms> parmsGetter;

			public <UsableIncidentsInCategory>c__AnonStorey3()
			{
			}

			internal bool <>m__0(IncidentDef x)
			{
				return x.category == this.cat && x.Worker.CanFireNow(this.parmsGetter(x), false);
			}
		}

		[CompilerGenerated]
		private sealed class <DebugTablesIncidentChances>c__AnonStorey4
		{
			internal IncidentCategoryDef cat;

			internal StorytellerComp $this;

			public <DebugTablesIncidentChances>c__AnonStorey4()
			{
			}

			internal bool <>m__0(IncidentDef d)
			{
				return d.category == this.cat;
			}

			internal float <>m__1(IncidentDef d)
			{
				return this.$this.IncidentChanceFinal(d);
			}

			internal string <>m__2(IncidentDef d)
			{
				return this.$this.IncidentChanceFactor_CurrentPopulation(d).ToString();
			}

			internal string <>m__3(IncidentDef d)
			{
				return this.$this.IncidentChanceFactor_PopulationIntent(d).ToString();
			}

			internal string <>m__4(IncidentDef d)
			{
				return this.$this.IncidentChanceFinal(d).ToString();
			}

			internal string <>m__5(IncidentDef d)
			{
				return (Find.CurrentMap != null) ? ((!this.$this.UsableIncidentsInCategory(this.cat, Find.CurrentMap).Contains(d)) ? string.Empty : "V") : "-";
			}

			internal string <>m__6(IncidentDef d)
			{
				return (!this.$this.UsableIncidentsInCategory(this.cat, Find.World).Contains(d)) ? string.Empty : "W";
			}
		}
	}
}
