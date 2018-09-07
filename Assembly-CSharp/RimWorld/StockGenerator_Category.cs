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
	public class StockGenerator_Category : StockGenerator
	{
		private ThingCategoryDef categoryDef;

		private IntRange thingDefCountRange = IntRange.one;

		private List<ThingDef> excludedThingDefs;

		private List<ThingCategoryDef> excludedCategories;

		public StockGenerator_Category()
		{
		}

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			for (int i = 0; i < numThingDefsToUse; i++)
			{
				ThingDef chosenThingDef;
				if (!(from t in this.categoryDef.DescendantThingDefs
				where t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate && !generatedDefs.Contains(t) && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)))
				select t).TryRandomElement(out chosenThingDef))
				{
					break;
				}
				foreach (Thing th in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return th;
				}
				generatedDefs.Add(chosenThingDef);
			}
			yield break;
		}

		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}

		[CompilerGenerated]
		private sealed class <GenerateThings>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal int <numThingDefsToUse>__0;

			internal int <i>__1;

			internal ThingDef <chosenThingDef>__2;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <th>__3;

			internal StockGenerator_Category $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private StockGenerator_Category.<GenerateThings>c__Iterator0.<GenerateThings>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <GenerateThings>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
				{
					List<ThingDef> generatedDefs = new List<ThingDef>();
					numThingDefsToUse = this.thingDefCountRange.RandomInRange;
					i = 0;
					break;
				}
				case 1u:
					Block_3:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							th = enumerator.Current;
							this.$current = th;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					<GenerateThings>c__AnonStorey.generatedDefs.Add(chosenThingDef);
					i++;
					break;
				default:
					return false;
				}
				if (i < numThingDefsToUse)
				{
					if ((from t in this.categoryDef.DescendantThingDefs
					where t.tradeability.TraderCanSell() && t.techLevel <= <GenerateThings>c__AnonStorey.<>f__ref$0.$this.maxTechLevelGenerate && !<GenerateThings>c__AnonStorey.generatedDefs.Contains(t) && (<GenerateThings>c__AnonStorey.<>f__ref$0.$this.excludedThingDefs == null || !<GenerateThings>c__AnonStorey.<>f__ref$0.$this.excludedThingDefs.Contains(t)) && (<GenerateThings>c__AnonStorey.<>f__ref$0.$this.excludedCategories == null || !<GenerateThings>c__AnonStorey.<>f__ref$0.$this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)))
					select t).TryRandomElement(out chosenThingDef))
					{
						enumerator = StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)).GetEnumerator();
						num = 4294967293u;
						goto Block_3;
					}
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StockGenerator_Category.<GenerateThings>c__Iterator0 <GenerateThings>c__Iterator = new StockGenerator_Category.<GenerateThings>c__Iterator0();
				<GenerateThings>c__Iterator.$this = this;
				return <GenerateThings>c__Iterator;
			}

			private sealed class <GenerateThings>c__AnonStorey1
			{
				internal List<ThingDef> generatedDefs;

				internal StockGenerator_Category.<GenerateThings>c__Iterator0 <>f__ref$0;

				public <GenerateThings>c__AnonStorey1()
				{
				}

				internal bool <>m__0(ThingDef t)
				{
					StockGenerator_Category.<GenerateThings>c__Iterator0 <>f__ref$0 = this.<>f__ref$0;
					StockGenerator_Category.<GenerateThings>c__Iterator0.<GenerateThings>c__AnonStorey1 <>f__ref$1 = this;
					ThingDef t = t2;
					return t.tradeability.TraderCanSell() && t.techLevel <= this.<>f__ref$0.$this.maxTechLevelGenerate && !this.generatedDefs.Contains(t) && (this.<>f__ref$0.$this.excludedThingDefs == null || !this.<>f__ref$0.$this.excludedThingDefs.Contains(t)) && (this.<>f__ref$0.$this.excludedCategories == null || !this.<>f__ref$0.$this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
				}

				private sealed class <GenerateThings>c__AnonStorey2
				{
					internal ThingDef t;

					internal StockGenerator_Category.<GenerateThings>c__Iterator0 <>f__ref$0;

					internal StockGenerator_Category.<GenerateThings>c__Iterator0.<GenerateThings>c__AnonStorey1 <>f__ref$1;

					public <GenerateThings>c__AnonStorey2()
					{
					}

					internal bool <>m__0(ThingCategoryDef c)
					{
						return c.DescendantThingDefs.Contains(this.t);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <HandlesThingDef>c__AnonStorey3
		{
			internal ThingDef t;

			public <HandlesThingDef>c__AnonStorey3()
			{
			}

			internal bool <>m__0(ThingCategoryDef c)
			{
				return c.DescendantThingDefs.Contains(this.t);
			}
		}
	}
}
