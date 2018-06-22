using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005EA RID: 1514
	public class Caravan_TraderTracker : IExposable
	{
		// Token: 0x06001E13 RID: 7699 RVA: 0x00102F0F File Offset: 0x0010130F
		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x00102F2C File Offset: 0x0010132C
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
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x00102F98 File Offset: 0x00101398
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
		// (get) Token: 0x06001E16 RID: 7702 RVA: 0x00102FC4 File Offset: 0x001013C4
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x00102FF0 File Offset: 0x001013F0
		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x00103010 File Offset: 0x00101410
		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x00103070 File Offset: 0x00101470
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x001030CC File Offset: 0x001014CC
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

		// Token: 0x06001E1B RID: 7707 RVA: 0x001030F8 File Offset: 0x001014F8
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

		// Token: 0x06001E1C RID: 7708 RVA: 0x00103228 File Offset: 0x00101628
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

		// Token: 0x040011CC RID: 4556
		private Caravan caravan;

		// Token: 0x040011CD RID: 4557
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
