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
	public class StockGenerator_Slaves : StockGenerator
	{
		private bool respectPopulationIntent = false;

		public StockGenerator_Slaves()
		{
		}

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			if (this.respectPopulationIntent && Rand.Value > Find.Storyteller.intenderPopulation.PopulationIntent)
			{
				yield break;
			}
			int count = this.countRange.RandomInRange;
			for (int i = 0; i < count; i++)
			{
				Faction slaveFaction;
				if (!(from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction
				select fac).TryRandomElement(out slaveFaction))
				{
					yield break;
				}
				PawnKindDef slave = PawnKindDefOf.Slave;
				Faction faction = slaveFaction;
				PawnGenerationRequest request = new PawnGenerationRequest(slave, faction, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, !this.trader.orbital, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				yield return PawnGenerator.GeneratePawn(request);
			}
			yield break;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability != Tradeability.None;
		}

		[CompilerGenerated]
		private sealed class <GenerateThings>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal int <count>__0;

			internal int <i>__1;

			internal Faction <slaveFaction>__2;

			internal int forTile;

			internal PawnGenerationRequest <request>__2;

			internal StockGenerator_Slaves $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<Faction, bool> <>f__am$cache0;

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
					if (this.respectPopulationIntent && Rand.Value > Find.Storyteller.intenderPopulation.PopulationIntent)
					{
						return false;
					}
					count = this.countRange.RandomInRange;
					i = 0;
					break;
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
				else if ((from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction
				select fac).TryRandomElement(out slaveFaction))
				{
					PawnKindDef slave = PawnKindDefOf.Slave;
					Faction faction = slaveFaction;
					int tile = forTile;
					request = new PawnGenerationRequest(slave, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, !this.trader.orbital, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
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
				StockGenerator_Slaves.<GenerateThings>c__Iterator0 <GenerateThings>c__Iterator = new StockGenerator_Slaves.<GenerateThings>c__Iterator0();
				<GenerateThings>c__Iterator.$this = this;
				<GenerateThings>c__Iterator.forTile = forTile;
				return <GenerateThings>c__Iterator;
			}

			private static bool <>m__0(Faction fac)
			{
				return fac != Faction.OfPlayer && fac.def.humanlikeFaction;
			}
		}
	}
}
