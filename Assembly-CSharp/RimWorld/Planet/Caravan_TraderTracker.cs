using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld.Planet
{
	public class Caravan_TraderTracker : IExposable
	{
		private Caravan caravan;

		private List<Pawn> soldPrisoners = new List<Pawn>();

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		public TraderKindDef TraderKind
		{
			get
			{
				List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (this.caravan.IsOwner(pawn) && pawn.TraderKind != null)
					{
						return pawn.TraderKind;
					}
				}
				return null;
			}
		}

		public IEnumerable<Thing> Goods
		{
			get
			{
				List<Thing> inv = CaravanInventoryUtility.AllInventoryItems(this.caravan);
				for (int i = 0; i < inv.Count; i++)
				{
					yield return inv[i];
				}
				List<Pawn> pawns = this.caravan.PawnsListForReading;
				for (int j = 0; j < pawns.Count; j++)
				{
					Pawn p = pawns[j];
					if (!this.caravan.IsOwner(p) && (!p.RaceProps.packAnimal || p.inventory == null || p.inventory.innerContainer.Count <= 0) && !this.soldPrisoners.Contains(p))
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan playerCaravan = playerNegotiator.GetCaravan();
			foreach (Thing item in CaravanInventoryUtility.AllInventoryItems(playerCaravan))
			{
				yield return item;
			}
			List<Pawn> pawns = playerCaravan.PawnsListForReading;
			for (int i = 0; i < pawns.Count; i++)
			{
				if (!playerCaravan.IsOwner(pawns[i]))
				{
					yield return pawns[i];
				}
			}
			yield break;
		}

		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.", false);
			}
			else
			{
				Caravan caravan = playerNegotiator.GetCaravan();
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.caravan);
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
					this.caravan.AddPawn(pawn, false);
					if (pawn.IsWorldPawn() && !this.caravan.Spawned)
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
					if (pawn.RaceProps.Humanlike)
					{
						this.soldPrisoners.Add(pawn);
					}
				}
				else
				{
					Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, this.caravan.PawnsListForReading, null, null);
					if (pawn2 == null)
					{
						Log.Error("Could not find pawn to move sold thing to (sold by player). thing=" + thing, false);
						thing.Destroy(DestroyMode.Vanish);
					}
					else if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
					{
						Log.Error("Could not add item to inventory.", false);
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, this.caravan.PawnsListForReading, null);
				caravan.AddPawn(pawn, true);
				if (!pawn.IsWorldPawn() && caravan.Spawned)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.soldPrisoners.Remove(pawn);
			}
			else
			{
				Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
				if (pawn2 == null)
				{
					Log.Error("Could not find pawn to move bought thing to (bought by player). thing=" + thing, false);
					thing.Destroy(DestroyMode.Vanish);
				}
				else if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error("Could not add item to inventory.", false);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		[CompilerGenerated]
		private bool <get_CanTradeNow>m__0(Thing x)
		{
			return this.TraderKind.WillTrade(x.def);
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(Pawn x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal List<Thing> <inv>__0;

			internal int <i>__1;

			internal List<Pawn> <pawns>__0;

			internal int <i>__2;

			internal Pawn <p>__3;

			internal Caravan_TraderTracker $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					inv = CaravanInventoryUtility.AllInventoryItems(this.caravan);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					goto IL_16B;
				default:
					return false;
				}
				if (i >= inv.Count)
				{
					pawns = this.caravan.PawnsListForReading;
					j = 0;
					goto IL_17A;
				}
				this.$current = inv[i];
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				IL_16B:
				j++;
				IL_17A:
				if (j >= pawns.Count)
				{
					this.$PC = -1;
				}
				else
				{
					p = pawns[j];
					if (!this.caravan.IsOwner(p) && (!p.RaceProps.packAnimal || p.inventory == null || p.inventory.innerContainer.Count <= 0) && !this.soldPrisoners.Contains(p))
					{
						this.$current = p;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_16B;
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
				Caravan_TraderTracker.<>c__Iterator0 <>c__Iterator = new Caravan_TraderTracker.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ColonyThingsWillingToBuy>c__Iterator1 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn playerNegotiator;

			internal Caravan <playerCaravan>__0;

			internal List<Thing>.Enumerator $locvar0;

			internal Thing <item>__1;

			internal List<Pawn> <pawns>__0;

			internal int <i>__2;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ColonyThingsWillingToBuy>c__Iterator1()
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
					playerCaravan = playerNegotiator.GetCaravan();
					enumerator = CaravanInventoryUtility.AllInventoryItems(playerCaravan).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					IL_12E:
					i++;
					goto IL_13D;
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
						item = enumerator.Current;
						this.$current = item;
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
						((IDisposable)enumerator).Dispose();
					}
				}
				pawns = playerCaravan.PawnsListForReading;
				i = 0;
				IL_13D:
				if (i >= pawns.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!playerCaravan.IsOwner(pawns[i]))
					{
						this.$current = pawns[i];
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_12E;
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
						((IDisposable)enumerator).Dispose();
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
				Caravan_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator1 <ColonyThingsWillingToBuy>c__Iterator = new Caravan_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator1();
				<ColonyThingsWillingToBuy>c__Iterator.playerNegotiator = playerNegotiator;
				return <ColonyThingsWillingToBuy>c__Iterator;
			}
		}
	}
}
