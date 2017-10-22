using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class Settlement : MapParent, ITrader
	{
		public Settlement_TraderTracker trader;

		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		public virtual bool Abandonable
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer));
			}
		}

		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		public override bool TransportPodsCanLandAndGenerateMap
		{
			get
			{
				return this.Attackable;
			}
		}

		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.TraderKind;
			}
		}

		public IEnumerable<Thing> Goods
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.StockListForReading;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				if (this.trader == null)
				{
					return 0;
				}
				return this.trader.RandomPriceFactorSeed;
			}
		}

		public string TraderName
		{
			get
			{
				if (this.trader == null)
				{
					return (string)null;
				}
				return this.trader.TraderName;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				if (this.trader == null)
				{
					return false;
				}
				return this.trader.CanTradeNow;
			}
		}

		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				if (this.trader == null)
				{
					return 0f;
				}
				return this.trader.TradePriceImprovementOffsetForPlayer;
			}
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			if (this.trader == null)
			{
				return null;
			}
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[1]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.SettlementPostLoadInit(this);
				this.previouslyGeneratedInhabitants.RemoveAll((Predicate<Pawn>)((Pawn x) => x == null));
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
		}

		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int num = this.previouslyGeneratedInhabitants.Count - 1; num >= 0; num--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[num];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(num);
				}
			}
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (this.Abandonable)
			{
				yield return (Gizmo)SettlementAbandonUtility.AbandonCommand(this);
			}
			if (base.Faction != Faction.OfPlayer && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
			{
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "CommandFormCaravan".Translate(),
					defaultDesc = "CommandFormCaravanDesc".Translate(),
					icon = MapParent.FormCaravanCommand,
					action = (Action)delegate
					{
						Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
						Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageSound.RejectInput);
					}
				};
			}
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			if (this.Visitable && CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				yield return new FloatMenuOption("VisitSettlement".Translate(this.Label), (Action)delegate
				{
					((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0108: stateMachine*/).caravan.pather.StartPath(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this.Tile, new CaravanArrivalAction_VisitSettlement(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this), true);
				}, MenuOptionPriority.Default, null, null, 0f, null, this);
				if (Prefs.DevMode)
				{
					yield return new FloatMenuOption("VisitSettlement".Translate(this.Label) + " (Dev: instantly)", (Action)delegate
					{
						((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_016e: stateMachine*/).caravan.Tile = ((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_016e: stateMachine*/)._003C_003Ef__this.Tile;
						((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_016e: stateMachine*/).caravan.pather.StopDead();
						new CaravanArrivalAction_VisitSettlement(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_016e: stateMachine*/)._003C_003Ef__this).Arrived(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_016e: stateMachine*/).caravan);
					}, MenuOptionPriority.Default, null, null, 0f, null, this);
				}
			}
			if (this.Attackable)
			{
				yield return new FloatMenuOption("AttackSettlement".Translate(this.Label), (Action)delegate
				{
					((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_01d0: stateMachine*/).caravan.pather.StartPath(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_01d0: stateMachine*/)._003C_003Ef__this.Tile, new CaravanArrivalAction_AttackSettlement(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_01d0: stateMachine*/)._003C_003Ef__this), true);
				}, MenuOptionPriority.Default, null, null, 0f, null, this);
				if (Prefs.DevMode)
				{
					yield return new FloatMenuOption("AttackSettlement".Translate(this.Label) + " (Dev: instantly)", (Action)delegate
					{
						((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0236: stateMachine*/).caravan.Tile = ((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0236: stateMachine*/)._003C_003Ef__this.Tile;
						new CaravanArrivalAction_AttackSettlement(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0236: stateMachine*/)._003C_003Ef__this).Arrived(((_003CGetFloatMenuOptions_003Ec__Iterator107)/*Error near IL_0236: stateMachine*/).caravan);
					}, MenuOptionPriority.Default, null, null, 0f, null, this);
				}
			}
		}

		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		virtual Faction get_Faction()
		{
			return base.Faction;
		}

		Faction ITrader.get_Faction()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_Faction
			return this.get_Faction();
		}
	}
}
