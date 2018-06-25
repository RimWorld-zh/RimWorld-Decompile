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
	public class ScenPart_PermaGameCondition : ScenPart
	{
		private GameConditionDef gameCondition;

		public const string PermaGameConditionTag = "PermaGameCondition";

		[CompilerGenerated]
		private static Func<GameConditionDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<GameConditionDef, bool> <>f__am$cache1;

		public ScenPart_PermaGameCondition()
		{
		}

		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.gameCondition.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate()
				{
					this.gameCondition = d;
				});
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		public override bool CanCoexistWith(ScenPart other)
		{
			bool result;
			if (this.gameCondition == null)
			{
				result = true;
			}
			else
			{
				ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
				if (scenPart_PermaGameCondition != null)
				{
					if (!this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		[CompilerGenerated]
		private static string <DoEditInterface>m__0(GameConditionDef d)
		{
			return d.LabelCap;
		}

		[CompilerGenerated]
		private Action <DoEditInterface>m__1(GameConditionDef d)
		{
			return delegate()
			{
				this.gameCondition = d;
			};
		}

		[CompilerGenerated]
		private static bool <AllowedGameConditions>m__2(GameConditionDef d)
		{
			return d.canBePermanent;
		}

		[CompilerGenerated]
		private sealed class <GetSummaryListEntries>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string tag;

			internal ScenPart_PermaGameCondition $this;

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
					if (tag == "PermaGameCondition")
					{
						this.$current = this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
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
				ScenPart_PermaGameCondition.<GetSummaryListEntries>c__Iterator0 <GetSummaryListEntries>c__Iterator = new ScenPart_PermaGameCondition.<GetSummaryListEntries>c__Iterator0();
				<GetSummaryListEntries>c__Iterator.$this = this;
				<GetSummaryListEntries>c__Iterator.tag = tag;
				return <GetSummaryListEntries>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey1
		{
			internal GameConditionDef d;

			internal ScenPart_PermaGameCondition $this;

			public <DoEditInterface>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.gameCondition = this.d;
			}
		}
	}
}
