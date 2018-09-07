using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_Sum : ThingSetMaker
	{
		public List<ThingSetMaker_Sum.Option> options;

		private List<ThingSetMaker_Sum.Option> optionsInRandomOrder = new List<ThingSetMaker_Sum.Option>();

		public ThingSetMaker_Sum()
		{
		}

		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f && this.options[i].thingSetMaker.CanGenerate(parms))
				{
					return true;
				}
			}
			return false;
		}

		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			this.optionsInRandomOrder.Clear();
			this.optionsInRandomOrder.AddRange(this.options);
			this.optionsInRandomOrder.Shuffle<ThingSetMaker_Sum.Option>();
			for (int i = 0; i < this.optionsInRandomOrder.Count; i++)
			{
				ThingSetMakerParams parms2 = parms;
				IntRange? countRange = parms2.countRange;
				if (countRange != null)
				{
					parms2.countRange = new IntRange?(new IntRange(parms2.countRange.Value.min, parms2.countRange.Value.max - num));
				}
				FloatRange? totalMarketValueRange = parms2.totalMarketValueRange;
				if (totalMarketValueRange != null)
				{
					parms2.totalMarketValueRange = new FloatRange?(new FloatRange(parms2.totalMarketValueRange.Value.min, parms2.totalMarketValueRange.Value.max - num2));
				}
				FloatRange? totalNutritionRange = parms2.totalNutritionRange;
				if (totalNutritionRange != null)
				{
					parms2.totalNutritionRange = new FloatRange?(new FloatRange(parms2.totalNutritionRange.Value.min, parms2.totalNutritionRange.Value.max - num3));
				}
				float? maxTotalMass = parms2.maxTotalMass;
				if (maxTotalMass != null)
				{
					float? maxTotalMass2 = parms2.maxTotalMass;
					parms2.maxTotalMass = ((maxTotalMass2 == null) ? null : new float?(maxTotalMass2.GetValueOrDefault() - num4));
				}
				if (Rand.Chance(this.optionsInRandomOrder[i].chance) && this.optionsInRandomOrder[i].thingSetMaker.CanGenerate(parms2))
				{
					List<Thing> list = this.optionsInRandomOrder[i].thingSetMaker.Generate(parms2);
					num += list.Count;
					for (int j = 0; j < list.Count; j++)
					{
						num2 += list[j].MarketValue * (float)list[j].stackCount;
						if (list[j].def.IsIngestible)
						{
							num3 += list[j].GetStatValue(StatDefOf.Nutrition, true) * (float)list[j].stackCount;
						}
						if (!(list[j] is Pawn))
						{
							num4 += list[j].GetStatValue(StatDefOf.Mass, true) * (float)list[j].stackCount;
						}
					}
					outThings.AddRange(list);
				}
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f)
				{
					foreach (ThingDef t in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return t;
					}
				}
			}
			yield break;
		}

		public class Option
		{
			public ThingSetMaker thingSetMaker;

			public float chance = 1f;

			public Option()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <AllGeneratableThingsDebugSub>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal int <i>__1;

			internal ThingSetMakerParams parms;

			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <t>__2;

			internal ThingSetMaker_Sum $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllGeneratableThingsDebugSub>c__Iterator0()
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
					i = 0;
					goto IL_10F;
				case 1u:
					Block_3:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							t = enumerator.Current;
							this.$current = t;
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
					break;
				default:
					return false;
				}
				IL_101:
				i++;
				IL_10F:
				if (i >= this.options.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.options[i].chance <= 0f)
					{
						goto IL_101;
					}
					enumerator = this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms).GetEnumerator();
					num = 4294967293u;
					goto Block_3;
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingSetMaker_Sum.<AllGeneratableThingsDebugSub>c__Iterator0 <AllGeneratableThingsDebugSub>c__Iterator = new ThingSetMaker_Sum.<AllGeneratableThingsDebugSub>c__Iterator0();
				<AllGeneratableThingsDebugSub>c__Iterator.$this = this;
				<AllGeneratableThingsDebugSub>c__Iterator.parms = parms;
				return <AllGeneratableThingsDebugSub>c__Iterator;
			}
		}
	}
}
