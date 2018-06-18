using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005EE RID: 1518
	public class Caravan_TraderTracker : IExposable
	{
		// Token: 0x06001E1C RID: 7708 RVA: 0x00102EBB File Offset: 0x001012BB
		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001E1D RID: 7709 RVA: 0x00102ED8 File Offset: 0x001012D8
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

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001E1E RID: 7710 RVA: 0x00102F44 File Offset: 0x00101344
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

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x00102F70 File Offset: 0x00101370
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001E20 RID: 7712 RVA: 0x00102F9C File Offset: 0x0010139C
		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x00102FBC File Offset: 0x001013BC
		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0010301C File Offset: 0x0010141C
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x00103078 File Offset: 0x00101478
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

		// Token: 0x06001E24 RID: 7716 RVA: 0x001030A4 File Offset: 0x001014A4
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

		// Token: 0x06001E25 RID: 7717 RVA: 0x001031D4 File Offset: 0x001015D4
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

		// Token: 0x040011CF RID: 4559
		private Caravan caravan;

		// Token: 0x040011D0 RID: 4560
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
