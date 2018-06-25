using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class StockGenerator_Animals : StockGenerator
	{
		[NoTranslate]
		private List<string> tradeTagsSell = null;

		[NoTranslate]
		private List<string> tradeTagsBuy = null;

		private IntRange kindCountRange = new IntRange(1, 1);

		private float minWildness = 0f;

		private float maxWildness = 1f;

		private bool checkTemperature = false;

		private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 100f),
				true
			},
			{
				new CurvePoint(0.25f, 60f),
				true
			},
			{
				new CurvePoint(0.5f, 30f),
				true
			},
			{
				new CurvePoint(0.75f, 12f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		public StockGenerator_Animals()
		{
		}

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			int numKinds = this.kindCountRange.RandomInRange;
			int count = this.countRange.RandomInRange;
			List<PawnKindDef> kinds = new List<PawnKindDef>();
			for (int j = 0; j < numKinds; j++)
			{
				PawnKindDef item;
				if (!(from k in DefDatabase<PawnKindDef>.AllDefs
				where !kinds.Contains(k) && this.PawnKindAllowed(k, forTile)
				select k).TryRandomElementByWeight((PawnKindDef k) => this.SelectionChance(k), out item))
				{
					break;
				}
				kinds.Add(item);
			}
			for (int i = 0; i < count; i++)
			{
				PawnKindDef kind;
				if (!kinds.TryRandomElement(out kind))
				{
					yield break;
				}
				PawnKindDef kind2 = kind;
				int forTile2 = forTile;
				PawnGenerationRequest request = new PawnGenerationRequest(kind2, null, PawnGenerationContext.NonPlayer, forTile2, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				yield return PawnGenerator.GeneratePawn(request);
			}
			yield break;
		}

		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags.Contains(tag)));
		}

		private bool PawnKindAllowed(PawnKindDef kind, int forTile)
		{
			bool result;
			if (!kind.RaceProps.Animal || kind.RaceProps.wildness < this.minWildness || kind.RaceProps.wildness > this.maxWildness || kind.RaceProps.wildness > 1f)
			{
				result = false;
			}
			else
			{
				if (this.checkTemperature)
				{
					int num = forTile;
					if (num == -1 && Find.AnyPlayerHomeMap != null)
					{
						num = Find.AnyPlayerHomeMap.Tile;
					}
					if (num != -1 && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(num, kind.race))
					{
						return false;
					}
				}
				result = (kind.race.tradeTags != null && this.tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)) && kind.race.tradeability.TraderCanSell());
			}
			return result;
		}

		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		private static void StockGenerationAnimals()
		{
			new StockGenerator_Animals
			{
				tradeTagsSell = new List<string>(),
				tradeTagsSell = 
				{
					"AnimalCommon",
					"AnimalUncommon"
				}
			}.LogAnimalChances();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static StockGenerator_Animals()
		{
		}

		[CompilerGenerated]
		private sealed class <GenerateThings>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal int <numKinds>__0;

			internal int <count>__0;

			internal int forTile;

			internal int <i>__1;

			internal PawnKindDef <kind>__2;

			internal PawnGenerationRequest <request>__2;

			internal StockGenerator_Animals $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private StockGenerator_Animals.<GenerateThings>c__Iterator0.<GenerateThings>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <GenerateThings>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
				{
					numKinds = this.kindCountRange.RandomInRange;
					count = this.countRange.RandomInRange;
					List<PawnKindDef> kinds = new List<PawnKindDef>();
					for (int j = 0; j < numKinds; j++)
					{
						PawnKindDef item;
						if (!(from k in DefDatabase<PawnKindDef>.AllDefs
						where !kinds.Contains(k) && this.PawnKindAllowed(k, forTile)
						select k).TryRandomElementByWeight((PawnKindDef k) => this.SelectionChance(k), out item))
						{
							break;
						}
						kinds.Add(item);
					}
					i = 0;
					break;
				}
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i >= count)
				{
					this.$PC = -1;
				}
				else if (<GenerateThings>c__AnonStorey.kinds.TryRandomElement(out kind))
				{
					PawnKindDef kind2 = kind;
					int tile = <GenerateThings>c__AnonStorey.forTile;
					request = new PawnGenerationRequest(kind2, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
					this.$current = PawnGenerator.GeneratePawn(request);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StockGenerator_Animals.<GenerateThings>c__Iterator0 <GenerateThings>c__Iterator = new StockGenerator_Animals.<GenerateThings>c__Iterator0();
				<GenerateThings>c__Iterator.$this = this;
				<GenerateThings>c__Iterator.forTile = forTile;
				return <GenerateThings>c__Iterator;
			}

			private sealed class <GenerateThings>c__AnonStorey1
			{
				internal List<PawnKindDef> kinds;

				internal int forTile;

				internal StockGenerator_Animals.<GenerateThings>c__Iterator0 <>f__ref$0;

				public <GenerateThings>c__AnonStorey1()
				{
				}

				internal bool <>m__0(PawnKindDef k)
				{
					return !this.kinds.Contains(k) && this.<>f__ref$0.$this.PawnKindAllowed(k, this.forTile);
				}

				internal float <>m__1(PawnKindDef k)
				{
					return this.<>f__ref$0.$this.SelectionChance(k);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <HandlesThingDef>c__AnonStorey2
		{
			internal ThingDef thingDef;

			public <HandlesThingDef>c__AnonStorey2()
			{
			}

			internal bool <>m__0(string tag)
			{
				return this.thingDef.tradeTags.Contains(tag);
			}

			internal bool <>m__1(string tag)
			{
				return this.thingDef.tradeTags.Contains(tag);
			}
		}

		[CompilerGenerated]
		private sealed class <PawnKindAllowed>c__AnonStorey3
		{
			internal PawnKindDef kind;

			public <PawnKindAllowed>c__AnonStorey3()
			{
			}

			internal bool <>m__0(string x)
			{
				return this.kind.race.tradeTags.Contains(x);
			}
		}
	}
}
