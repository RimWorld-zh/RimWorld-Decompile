using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class TraderKindDef : Def
	{
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		public float commonality = 1f;

		public bool orbital;

		public bool requestable = true;

		public SimpleCurve commonalityMultFromPopulationIntent;

		public TraderKindDef()
		{
		}

		public float CalculatedCommonality
		{
			get
			{
				float num = this.commonality;
				if (this.commonalityMultFromPopulationIntent != null)
				{
					num *= this.commonalityMultFromPopulationIntent.Evaluate(Find.Storyteller.intenderPopulation.PopulationIntent);
				}
				return num;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			foreach (StockGenerator stock in this.stockGenerators)
			{
				foreach (string err2 in stock.ConfigErrors(this))
				{
					yield return err2;
				}
			}
			yield break;
		}

		public bool WillTrade(ThingDef td)
		{
			for (int i = 0; i < this.stockGenerators.Count; i++)
			{
				if (this.stockGenerators[i].HandlesThingDef(td))
				{
					return true;
				}
			}
			return false;
		}

		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			PriceType result;
			if (thingDef == ThingDefOf.Silver)
			{
				result = PriceType.Undefined;
			}
			else
			{
				if (action == TradeAction.PlayerBuys)
				{
					for (int i = 0; i < this.stockGenerators.Count; i++)
					{
						PriceType result2;
						if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result2))
						{
							return result2;
						}
					}
				}
				result = PriceType.Normal;
			}
			return result;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal List<StockGenerator>.Enumerator $locvar1;

			internal StockGenerator <stock>__2;

			internal IEnumerator<string> $locvar2;

			internal string <err>__3;

			internal TraderKindDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_D2;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						err = enumerator.Current;
						this.$current = err;
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
				enumerator2 = this.stockGenerators.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_D2:
					switch (num)
					{
					case 2u:
						Block_11:
						try
						{
							switch (num)
							{
							}
							if (enumerator3.MoveNext())
							{
								err2 = enumerator3.Current;
								this.$current = err2;
								if (!this.$disposing)
								{
									this.$PC = 2;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator3 != null)
								{
									enumerator3.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator2.MoveNext())
					{
						stock = enumerator2.Current;
						enumerator3 = stock.ConfigErrors(this).GetEnumerator();
						num = 4294967293u;
						goto Block_11;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
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
				case 2u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TraderKindDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new TraderKindDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
