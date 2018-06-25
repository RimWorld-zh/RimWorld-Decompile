using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000604 RID: 1540
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader
	{
		// Token: 0x04001223 RID: 4643
		public Settlement_TraderTracker trader;

		// Token: 0x04001224 RID: 4644
		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		// Token: 0x04001225 RID: 4645
		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x04001226 RID: 4646
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x04001227 RID: 4647
		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x0010928C File Offset: 0x0010768C
		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x001092AC File Offset: 0x001076AC
		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x001092F8 File Offset: 0x001076F8
		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x00109320 File Offset: 0x00107720
		public TraderKindDef TraderKind
		{
			get
			{
				TraderKindDef result;
				if (this.trader == null)
				{
					result = null;
				}
				else
				{
					result = this.trader.TraderKind;
				}
				return result;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x00109354 File Offset: 0x00107754
		public IEnumerable<Thing> Goods
		{
			get
			{
				IEnumerable<Thing> result;
				if (this.trader == null)
				{
					result = null;
				}
				else
				{
					result = this.trader.StockListForReading;
				}
				return result;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x00109388 File Offset: 0x00107788
		public int RandomPriceFactorSeed
		{
			get
			{
				int result;
				if (this.trader == null)
				{
					result = 0;
				}
				else
				{
					result = this.trader.RandomPriceFactorSeed;
				}
				return result;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x001093BC File Offset: 0x001077BC
		public string TraderName
		{
			get
			{
				string result;
				if (this.trader == null)
				{
					result = null;
				}
				else
				{
					result = this.trader.TraderName;
				}
				return result;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x001093F0 File Offset: 0x001077F0
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x00109424 File Offset: 0x00107824
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				float result;
				if (this.trader == null)
				{
					result = 0f;
				}
				else
				{
					result = this.trader.TradePriceImprovementOffsetForPlayer;
				}
				return result;
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0010945C File Offset: 0x0010785C
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> result;
			if (this.trader == null)
			{
				result = null;
			}
			else
			{
				result = this.trader.ColonyThingsWillingToBuy(playerNegotiator);
			}
			return result;
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0010948F File Offset: 0x0010788F
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x001094A0 File Offset: 0x001078A0
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x001094B4 File Offset: 0x001078B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0010952F File Offset: 0x0010792F
		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x00109550 File Offset: 0x00107950
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x001095B4 File Offset: 0x001079B4
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x001095F2 File Offset: 0x001079F2
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00109614 File Offset: 0x00107A14
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabel();
				if (!base.Faction.def.hidden)
				{
					text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
				}
			}
			return text;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x001096A8 File Offset: 0x00107AA8
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.TraderKind != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowSellableItems".Translate(),
					defaultDesc = "CommandShowSellableItemsDesc".Translate(),
					icon = Settlement.ShowSellableItemsCommand,
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_SellableItems(this.TraderKind));
					}
				};
			}
			if (base.Faction != Faction.OfPlayer)
			{
				if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
				{
					Command_Action formCaravan = new Command_Action();
					formCaravan.defaultLabel = "CommandFormCaravan".Translate();
					formCaravan.defaultDesc = "CommandFormCaravanDesc".Translate();
					formCaravan.icon = Settlement.FormCaravanCommand;
					formCaravan.action = delegate()
					{
						Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
						Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
					};
					yield return formCaravan;
				}
			}
			yield break;
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x001096D4 File Offset: 0x00107AD4
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
			{
				yield return CaravanVisitUtility.TradeCommand(caravan);
			}
			if (CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this))
			{
				yield return FactionGiftUtility.OfferGiftsCommand(caravan, this);
			}
			foreach (Gizmo g in this.<GetCaravanGizmos>__BaseCallProxy1(caravan))
			{
				yield return g;
			}
			if (this.Attackable)
			{
				yield return new Command_Action
				{
					icon = Settlement.AttackCommand,
					defaultLabel = "CommandAttackSettlement".Translate(),
					defaultDesc = "CommandAttackSettlementDesc".Translate(),
					action = delegate()
					{
						SettlementUtility.Attack(caravan, this.$this);
					}
				};
			}
			yield break;
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00109708 File Offset: 0x00107B08
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy2(caravan))
			{
				yield return o;
			}
			if (CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				foreach (FloatMenuOption f in CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, this))
				{
					yield return f;
				}
			}
			foreach (FloatMenuOption f2 in CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, this))
			{
				yield return f2;
			}
			foreach (FloatMenuOption f3 in CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, this))
			{
				yield return f3;
			}
			yield break;
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0010973C File Offset: 0x00107B3C
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative))
			{
				yield return o;
			}
			foreach (FloatMenuOption f in TransportPodsArrivalAction_VisitSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in TransportPodsArrivalAction_GiveGift.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f2;
			}
			foreach (FloatMenuOption f3 in TransportPodsArrivalAction_AttackSettlement.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f3;
			}
			yield break;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00109774 File Offset: 0x00107B74
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}
	}
}
