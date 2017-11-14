using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader
	{
		public Settlement_TraderTracker trader;

		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
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
					return null;
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
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
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
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (base.Faction == Faction.OfPlayer)
				yield break;
			if (Current.ProgramState != ProgramState.Playing)
				yield break;
			if (PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan))
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "CommandFormCaravan".Translate(),
				defaultDesc = "CommandFormCaravanDesc".Translate(),
				icon = Settlement.FormCaravanCommand,
				action = delegate
				{
					Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
					Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput);
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0179:
			/*Error near IL_017a: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.CanTradeNow)
			{
				yield return (Gizmo)CaravanVisitUtility.TradeCommand(caravan);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.GetComponent<CaravanRequestComp>() == null)
				yield break;
			if (!base.GetComponent<CaravanRequestComp>().ActiveRequest)
				yield break;
			yield return (Gizmo)CaravanVisitUtility.FulfillRequestCommand(caravan);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			_003CGetFloatMenuOptions_003Ec__Iterator2 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator2)/*Error near IL_0044: stateMachine*/;
			using (IEnumerator<FloatMenuOption> enumerator = this._003CGetFloatMenuOptions_003E__BaseCallProxy1(caravan).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption o = enumerator.Current;
					yield return o;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.Visitable && CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				yield return new FloatMenuOption("VisitSettlement".Translate(this.Label), delegate
				{
					caravan.pather.StartPath(_003CGetFloatMenuOptions_003Ec__Iterator._0024this.Tile, new CaravanArrivalAction_VisitSettlement(_003CGetFloatMenuOptions_003Ec__Iterator._0024this), true);
				}, MenuOptionPriority.Default, null, null, 0f, null, this);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.Attackable)
				yield break;
			yield return new FloatMenuOption("AttackSettlement".Translate(this.Label), delegate
			{
				caravan.pather.StartPath(_003CGetFloatMenuOptions_003Ec__Iterator._0024this.Tile, new CaravanArrivalAction_AttackSettlement(_003CGetFloatMenuOptions_003Ec__Iterator._0024this), true);
			}, MenuOptionPriority.Default, null, null, 0f, null, this);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_02d3:
			/*Error near IL_02d4: Unexpected return in MoveNext()*/;
		}

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
