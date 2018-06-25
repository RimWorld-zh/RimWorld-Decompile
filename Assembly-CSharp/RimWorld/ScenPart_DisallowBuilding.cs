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
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		private ThingDef building;

		private const string DisallowBuildingTag = "DisallowBuilding";

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		public ScenPart_DisallowBuilding()
		{
		}

		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "DisallowBuilding")
			{
				yield return this.building.LabelCap;
			}
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement<ThingDef>();
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.building.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleBuildingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.building = localTd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building;
		}

		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.BuildableByPlayer
			select d;
		}

		private IEnumerable<ThingDef> RandomizableBuildingDefs()
		{
			yield return ThingDefOf.Wall;
			yield return ThingDefOf.Turret_MiniTurret;
			yield return ThingDefOf.OrbitalTradeBeacon;
			yield return ThingDefOf.Battery;
			yield return ThingDefOf.TrapDeadfall;
			yield return ThingDefOf.Cooler;
			yield return ThingDefOf.Heater;
			yield break;
		}

		[CompilerGenerated]
		private static string <DoEditInterface>m__0(ThingDef t)
		{
			return t.label;
		}

		[CompilerGenerated]
		private static bool <PossibleBuildingDefs>m__1(ThingDef d)
		{
			return d.category == ThingCategory.Building && d.BuildableByPlayer;
		}

		[CompilerGenerated]
		private sealed class <GetSummaryListEntries>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string tag;

			internal ScenPart_DisallowBuilding $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetSummaryListEntries>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (tag == "DisallowBuilding")
					{
						this.$current = this.building.LabelCap;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
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

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenPart_DisallowBuilding.<GetSummaryListEntries>c__Iterator0 <GetSummaryListEntries>c__Iterator = new ScenPart_DisallowBuilding.<GetSummaryListEntries>c__Iterator0();
				<GetSummaryListEntries>c__Iterator.$this = this;
				<GetSummaryListEntries>c__Iterator.tag = tag;
				return <GetSummaryListEntries>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey2
		{
			internal ThingDef localTd;

			internal ScenPart_DisallowBuilding $this;

			public <DoEditInterface>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.$this.building = this.localTd;
			}
		}

		[CompilerGenerated]
		private sealed class <RandomizableBuildingDefs>c__Iterator1 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RandomizableBuildingDefs>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = ThingDefOf.Wall;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = ThingDefOf.Turret_MiniTurret;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = ThingDefOf.OrbitalTradeBeacon;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = ThingDefOf.Battery;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = ThingDefOf.TrapDeadfall;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = ThingDefOf.Cooler;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = ThingDefOf.Heater;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ScenPart_DisallowBuilding.<RandomizableBuildingDefs>c__Iterator1();
			}
		}
	}
}
