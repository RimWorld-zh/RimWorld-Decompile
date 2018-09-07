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
	public class StockGenerator_Tag : StockGenerator
	{
		[NoTranslate]
		private string tradeTag;

		private IntRange thingDefCountRange = IntRange.one;

		private List<ThingDef> excludedThingDefs;

		public StockGenerator_Tag()
		{
		}

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			for (int i = 0; i < numThingDefsToUse; i++)
			{
				ThingDef chosenThingDef;
				if (!(from d in DefDatabase<ThingDef>.AllDefs
				where this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(d)) && !generatedDefs.Contains(d)
				select d).TryRandomElement(out chosenThingDef))
				{
					yield break;
				}
				foreach (Thing th in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return th;
				}
				generatedDefs.Add(chosenThingDef);
			}
			yield break;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		[CompilerGenerated]
		private sealed class <GenerateThings>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal int <numThingDefsToUse>__0;

			internal int <i>__1;

			internal ThingDef <chosenThingDef>__2;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <th>__3;

			internal StockGenerator_Tag $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private StockGenerator_Tag.<GenerateThings>c__Iterator0.<GenerateThings>c__AnonStorey1 $locvar1;

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
				if (i >= numThingDefsToUse)
				{
					this.$PC = -1;
				}
				else if ((from d in DefDatabase<ThingDef>.AllDefs
				where <GenerateThings>c__AnonStorey.<>f__ref$0.$this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && (<GenerateThings>c__AnonStorey.<>f__ref$0.$this.excludedThingDefs == null || !<GenerateThings>c__AnonStorey.<>f__ref$0.$this.excludedThingDefs.Contains(d)) && !<GenerateThings>c__AnonStorey.generatedDefs.Contains(d)
				select d).TryRandomElement(out chosenThingDef))
				{
					enumerator = StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)).GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
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
				StockGenerator_Tag.<GenerateThings>c__Iterator0 <GenerateThings>c__Iterator = new StockGenerator_Tag.<GenerateThings>c__Iterator0();
				<GenerateThings>c__Iterator.$this = this;
				return <GenerateThings>c__Iterator;
			}

			private sealed class <GenerateThings>c__AnonStorey1
			{
				internal List<ThingDef> generatedDefs;

				internal StockGenerator_Tag.<GenerateThings>c__Iterator0 <>f__ref$0;

				public <GenerateThings>c__AnonStorey1()
				{
				}

				internal bool <>m__0(ThingDef d)
				{
					return this.<>f__ref$0.$this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && (this.<>f__ref$0.$this.excludedThingDefs == null || !this.<>f__ref$0.$this.excludedThingDefs.Contains(d)) && !this.generatedDefs.Contains(d);
				}
			}
		}
	}
}
