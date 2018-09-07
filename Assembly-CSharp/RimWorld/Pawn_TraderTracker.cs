using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class Pawn_TraderTracker : IExposable
	{
		private Pawn pawn;

		public TraderKindDef traderKind;

		private List<Pawn> soldPrisoners = new List<Pawn>();

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public IEnumerable<Thing> Goods
		{
			get
			{
				Lord lord = this.pawn.GetLord();
				if (lord == null || !(lord.LordJob is LordJob_TradeWithColony))
				{
					for (int i = 0; i < this.pawn.inventory.innerContainer.Count; i++)
					{
						Thing t = this.pawn.inventory.innerContainer[i];
						if (!this.pawn.inventory.NotForSale(t))
						{
							yield return t;
						}
					}
				}
				if (lord != null)
				{
					for (int j = 0; j < lord.ownedPawns.Count; j++)
					{
						Pawn p = lord.ownedPawns[j];
						TraderCaravanRole role = p.GetTraderCaravanRole();
						if (role == TraderCaravanRole.Carrier)
						{
							for (int k = 0; k < p.inventory.innerContainer.Count; k++)
							{
								yield return p.inventory.innerContainer[k];
							}
						}
						else if (role == TraderCaravanRole.Chattel && !this.soldPrisoners.Contains(p))
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && this.Goods.Any((Thing x) => this.traderKind.WillTrade(x.def));
			}
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> items = from x in this.pawn.Map.listerThings.AllThings
			where x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && this.ReachableForTrade(x)
			select x;
			foreach (Thing t in items)
			{
				yield return t;
			}
			bool hasLord = this.pawn.GetLord() != null;
			if (hasLord)
			{
				foreach (Pawn p in from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map)
				where !x.Downed && this.ReachableForTrade(x)
				select x)
				{
					yield return p;
				}
			}
			yield break;
		}

		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.", false);
				return;
			}
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
				this.AddPawnToStock(pawn);
			}
			else
			{
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
				Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
				if (thing2 != null)
				{
					if (!thing2.TryAbsorbStack(thing, false))
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				else
				{
					this.AddThingToRandomInventory(thing);
				}
			}
		}

		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Undefined, null);
				}
				if (this.soldPrisoners.Contains(pawn))
				{
					this.soldPrisoners.Remove(pawn);
				}
			}
			else
			{
				IntVec3 positionHeld = toGive.PositionHeld;
				Map mapHeld = toGive.MapHeld;
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
					}
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not place bought thing ",
						thing,
						" at ",
						positionHeld
					}), false);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map, WipeMode.Vanish);
			}
			if (newPawn.Faction != this.pawn.Faction)
			{
				newPawn.SetFaction(this.pawn.Faction, null);
			}
			if (newPawn.RaceProps.Humanlike)
			{
				newPawn.kindDef = PawnKindDefOf.Slave;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				newPawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Concat(new object[]
				{
					"Tried to sell pawn ",
					newPawn,
					" to ",
					this.pawn,
					", but ",
					this.pawn,
					" has no lord. Traders without lord can't buy pawns."
				}), false);
				return;
			}
			if (newPawn.RaceProps.Humanlike)
			{
				this.soldPrisoners.Add(newPawn);
			}
			lord.AddPawn(newPawn);
		}

		private void AddThingToRandomInventory(Thing thing)
		{
			Lord lord = this.pawn.GetLord();
			IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
			if (lord != null)
			{
				source = from x in lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
			}
			if (source.Any<Pawn>())
			{
				if (!source.RandomElement<Pawn>().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		private bool ReachableForTrade(Thing thing)
		{
			return this.pawn.Map == thing.Map && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}

		[CompilerGenerated]
		private bool <get_CanTradeNow>m__0(Thing x)
		{
			return this.traderKind.WillTrade(x.def);
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(Pawn x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private static bool <AddThingToRandomInventory>m__2(Pawn x)
		{
			return x.GetTraderCaravanRole() == TraderCaravanRole.Carrier;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Lord <lord>__0;

			internal bool <traderCaravan>__0;

			internal int <i>__1;

			internal Thing <t>__2;

			internal int <i>__3;

			internal Pawn <p>__4;

			internal TraderCaravanRole <role>__4;

			internal int <j>__5;

			internal Pawn_TraderTracker $this;

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
				{
					lord = this.pawn.GetLord();
					bool traderCaravan = lord != null && lord.LordJob is LordJob_TradeWithColony;
					if (traderCaravan)
					{
						goto IL_116;
					}
					i = 0;
					break;
				}
				case 1u:
					IL_E3:
					i++;
					break;
				case 2u:
					k++;
					goto IL_1B5;
				case 3u:
					IL_221:
					j++;
					goto IL_22F;
				default:
					return false;
				}
				if (i < this.pawn.inventory.innerContainer.Count)
				{
					t = this.pawn.inventory.innerContainer[i];
					if (!this.pawn.inventory.NotForSale(t))
					{
						this.$current = t;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_E3;
				}
				IL_116:
				if (lord != null)
				{
					j = 0;
					goto IL_22F;
				}
				goto IL_24A;
				IL_1B5:
				if (k >= p.inventory.innerContainer.Count)
				{
					goto IL_221;
				}
				this.$current = p.inventory.innerContainer[k];
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_22F:
				if (j < lord.ownedPawns.Count)
				{
					p = lord.ownedPawns[j];
					role = p.GetTraderCaravanRole();
					if (role == TraderCaravanRole.Carrier)
					{
						k = 0;
						goto IL_1B5;
					}
					if (role == TraderCaravanRole.Chattel && !this.soldPrisoners.Contains(p))
					{
						this.$current = p;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					goto IL_221;
				}
				IL_24A:
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
				Pawn_TraderTracker.<>c__Iterator0 <>c__Iterator = new Pawn_TraderTracker.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ColonyThingsWillingToBuy>c__Iterator1 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal IEnumerable<Thing> <items>__0;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <t>__1;

			internal bool <hasLord>__0;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn <p>__2;

			internal Pawn_TraderTracker $this;

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
					items = from x in this.pawn.Map.listerThings.AllThings
					where x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && base.ReachableForTrade(x)
					select x;
					enumerator = items.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_13B;
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
				hasLord = (this.pawn.GetLord() != null);
				if (!hasLord)
				{
					goto IL_1AF;
				}
				enumerator2 = (from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map)
				where !x.Downed && base.ReachableForTrade(x)
				select x).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_13B:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						p = enumerator2.Current;
						this.$current = p;
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
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				IL_1AF:
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				Pawn_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator1 <ColonyThingsWillingToBuy>c__Iterator = new Pawn_TraderTracker.<ColonyThingsWillingToBuy>c__Iterator1();
				<ColonyThingsWillingToBuy>c__Iterator.$this = this;
				return <ColonyThingsWillingToBuy>c__Iterator;
			}

			internal bool <>m__0(Thing x)
			{
				return x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && base.ReachableForTrade(x);
			}

			internal bool <>m__1(Pawn x)
			{
				return !x.Downed && base.ReachableForTrade(x);
			}
		}
	}
}
