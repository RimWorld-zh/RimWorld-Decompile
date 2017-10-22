using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ITrader, ILoadReferenceable
	{
		private string nameInt;

		public ThingOwner<Pawn> pawns;

		public bool autoJoinable;

		public Caravan_PathFollower pather;

		public Caravan_GotoMoteRenderer gotoMote;

		public Caravan_Tweener tweener;

		public Caravan_TraderTracker trader;

		public StoryState storyState;

		private Material cachedMat;

		private bool cachedImmobilized;

		private int cachedImmobilizedForTicks = -99999;

		private Pair<float, float> cachedDaysWorthOfFood;

		private int cachedDaysWorthOfFoodForTicks = -99999;

		private const int ImmobilizedCacheDuration = 60;

		private const int DaysWorthOfFoodCacheDuration = 3000;

		private const int TendIntervalTicks = 2000;

		private const int TryTakeScheduledDrugsIntervalTicks = 120;

		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);

		public List<Pawn> PawnsListForReading
		{
			get
			{
				return this.pawns.InnerListForReading;
			}
		}

		public override Material Material
		{
			get
			{
				if ((UnityEngine.Object)this.cachedMat == (UnityEngine.Object)null)
				{
					Color color = (base.Faction != null) ? ((!base.Faction.IsPlayer) ? base.Faction.Color : Caravan.PlayerCaravanColor) : Color.white;
					this.cachedMat = MaterialPool.MatFrom(base.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.DynamicObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		public override Vector3 DrawPos
		{
			get
			{
				return this.tweener.TweenedPos;
			}
		}

		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		public bool ImmobilizedByMass
		{
			get
			{
				bool result;
				if (Find.TickManager.TicksGame - this.cachedImmobilizedForTicks < 60)
				{
					result = this.cachedImmobilized;
				}
				else
				{
					this.cachedImmobilized = (this.MassUsage > this.MassCapacity);
					this.cachedImmobilizedForTicks = Find.TickManager.TicksGame;
					result = this.cachedImmobilized;
				}
				return result;
			}
		}

		public Pair<float, float> DaysWorthOfFood
		{
			get
			{
				Pair<float, float> result;
				if (Find.TickManager.TicksGame - this.cachedDaysWorthOfFoodForTicks < 3000)
				{
					result = this.cachedDaysWorthOfFood;
				}
				else
				{
					this.cachedDaysWorthOfFood = new Pair<float, float>(DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this), DaysUntilRotCalculator.ApproxDaysUntilRot(this));
					this.cachedDaysWorthOfFoodForTicks = Find.TickManager.TicksGame;
					result = this.cachedDaysWorthOfFood;
				}
				return result;
			}
		}

		public bool CantMove
		{
			get
			{
				return this.Resting || this.AnyPawnHasExtremeMentalBreak || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass;
			}
		}

		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity(this.PawnsListForReading);
			}
		}

		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		public bool AllOwnersDowned
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.pawns.Count)
					{
						if (this.IsOwner(this.pawns[num]) && !this.pawns[num].Downed)
						{
							result = false;
							break;
						}
						num++;
						continue;
					}
					result = true;
					break;
				}
				return result;
			}
		}

		public bool AllOwnersHaveMentalBreak
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.pawns.Count)
					{
						if (this.IsOwner(this.pawns[num]) && !this.pawns[num].InMentalState)
						{
							result = false;
							break;
						}
						num++;
						continue;
					}
					result = true;
					break;
				}
				return result;
			}
		}

		public bool Resting
		{
			get
			{
				return CaravanRestUtility.RestingNowAt(base.Tile);
			}
		}

		public int LeftRestTicks
		{
			get
			{
				return CaravanRestUtility.LeftRestTicksAt(base.Tile);
			}
		}

		public int LeftNonRestTicks
		{
			get
			{
				return CaravanRestUtility.LeftNonRestTicksAt(base.Tile);
			}
		}

		public override string Label
		{
			get
			{
				return (this.nameInt == null) ? base.Label : this.nameInt;
			}
		}

		private bool AnyPawnHasExtremeMentalBreak
		{
			get
			{
				return this.FirstPawnWithExtremeMentalBreak != null;
			}
		}

		private Pawn FirstPawnWithExtremeMentalBreak
		{
			get
			{
				int num = 0;
				Pawn result;
				while (true)
				{
					if (num < this.pawns.Count)
					{
						if (this.pawns[num].InMentalState && this.pawns[num].MentalStateDef.IsExtreme)
						{
							result = this.pawns[num];
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
			}
		}

		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this);
			}
		}

		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050);
				return null;
			}
		}

		public float PlayerWealthForStoryteller
		{
			get
			{
				float result;
				if (!this.IsPlayerControlled)
				{
					result = 0f;
				}
				else
				{
					float num = 0f;
					for (int i = 0; i < this.pawns.Count; i++)
					{
						num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(this.pawns[i]);
					}
					result = (float)(num * 0.5);
				}
				return result;
			}
		}

		public IEnumerable<Pawn> FreeColonistsForStoryteller
		{
			get
			{
				return this.IsPlayerControlled ? (from x in this.PawnsListForReading
				where x.IsFreeColonist
				select x) : Enumerable.Empty<Pawn>();
			}
		}

		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return CaravanIncidentUtility.IncidentPointsRandomFactorRange;
			}
		}

		public TraderKindDef TraderKind
		{
			get
			{
				return this.trader.TraderKind;
			}
		}

		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		public bool CanTradeNow
		{
			get
			{
				return this.trader.CanTradeNow;
			}
		}

		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		public Caravan()
		{
			this.pawns = new ThingOwner<Pawn>(this, false, LookMode.Reference);
			this.pather = new Caravan_PathFollower(this);
			this.gotoMote = new Caravan_GotoMoteRenderer();
			this.tweener = new Caravan_Tweener(this);
			this.trader = new Caravan_TraderTracker(this);
			this.storyState = new StoryState(this);
		}

		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
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
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.pawns.RemoveAll((Predicate<Pawn>)((Pawn x) => x.Destroyed));
			}
			Scribe_Values.Look<string>(ref this.nameInt, "name", (string)null, false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawns, "pawns", new object[1]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.autoJoinable, "autoJoinable", false, false);
			Scribe_Deep.Look<Caravan_PathFollower>(ref this.pather, "pather", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Caravan_TraderTracker>(ref this.trader, "trader", new object[1]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[1]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.CaravanPostLoadInit(this);
			}
		}

		public override void PostAdd()
		{
			base.PostAdd();
			Find.ColonistBar.MarkColonistsDirty();
		}

		public override void PostRemove()
		{
			base.PostRemove();
			this.pather.StopDead();
			Find.ColonistBar.MarkColonistsDirty();
		}

		public override void Tick()
		{
			base.Tick();
			this.CheckAnyNonWorldPawns();
			this.pather.PatherTick();
			this.tweener.TweenerTick();
			CaravanPawnsNeedsUtility.TrySatisfyPawnsNeeds(this);
			if (this.IsHashIntervalTick(120))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(this);
			}
			if (this.IsHashIntervalTick(2000))
			{
				CaravanTendUtility.TryTendToRandomPawn(this);
			}
		}

		public override void SpawnSetup()
		{
			base.SpawnSetup();
			this.tweener.ResetToPosition();
		}

		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.gotoMote.RenderMote();
		}

		public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (p == null)
			{
				Log.Warning("Tried to add a null pawn to " + this);
			}
			else if (p.Dead)
			{
				Log.Warning("Tried to add " + p + " to " + this + ", but this pawn is dead.");
			}
			else
			{
				Pawn pawn = p.carryTracker.CarriedThing as Pawn;
				if (p.Spawned)
				{
					p.DeSpawn();
				}
				if (this.pawns.TryAdd(p, true))
				{
					if (pawn != null)
					{
						p.carryTracker.innerContainer.Remove(pawn);
						this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
						if (addCarriedPawnToWorldPawnsIfAny)
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						}
					}
				}
				else
				{
					Log.Error("Couldn't add pawn " + p + " to caravan.");
				}
			}
		}

		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + this);
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
				}
				else
				{
					CaravanInventoryUtility.GiveThing(this, thing);
				}
			}
		}

		public bool ContainsPawn(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		public void RemovePawn(Pawn p)
		{
			this.pawns.Remove(p);
		}

		public void RemoveAllPawns()
		{
			this.pawns.Clear();
		}

		public bool IsOwner(Pawn p)
		{
			return this.pawns.Contains(p) && CaravanUtility.IsOwner(p, base.Faction);
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.Resting)
			{
				stringBuilder.Append("CaravanResting".Translate());
			}
			else if (this.AnyPawnHasExtremeMentalBreak)
			{
				stringBuilder.Append("CaravanMemberMentalBreak".Translate(this.FirstPawnWithExtremeMentalBreak.LabelShort));
			}
			else if (this.AllOwnersDowned)
			{
				stringBuilder.Append("AllCaravanMembersDowned".Translate());
			}
			else if (this.AllOwnersHaveMentalBreak)
			{
				stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
			}
			else if (this.pather.Moving)
			{
				if (this.pather.arrivalAction != null)
				{
					stringBuilder.Append(this.pather.arrivalAction.ReportString);
				}
				else
				{
					stringBuilder.Append("CaravanTraveling".Translate());
				}
			}
			else
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(this);
				if (settlement != null)
				{
					stringBuilder.Append("CaravanVisiting".Translate(settlement.Label));
				}
				else
				{
					stringBuilder.Append("CaravanWaiting".Translate());
				}
			}
			if (this.pather.Moving)
			{
				float num = (float)((float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, true) / 60000.0);
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(num.ToString("0.#")));
			}
			if (this.ImmobilizedByMass)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanImmobilizedByMass".Translate());
			}
			string text = default(string);
			if (CaravanPawnsNeedsUtility.AnyPawnOutOfFood(this, out text))
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanOutOfFood".Translate());
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(text);
					stringBuilder.Append(".");
				}
			}
			else if (this.DaysWorthOfFood.First < 1000.0)
			{
				Pair<float, float> daysWorthOfFood = this.DaysWorthOfFood;
				stringBuilder.AppendLine();
				if (daysWorthOfFood.Second < 1000.0)
				{
					stringBuilder.Append("CaravanDaysOfFoodRot".Translate(daysWorthOfFood.First.ToString("0.#"), daysWorthOfFood.Second.ToString("0.#")));
				}
				else
				{
					stringBuilder.Append("CaravanDaysOfFood".Translate(daysWorthOfFood.First.ToString("0.#")));
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("CaravanBaseMovementTime".Translate() + ": " + ((float)((float)this.TicksPerMove / 2500.0)).ToString("0.##") + " " + "CaravanHoursPerTile".Translate());
			stringBuilder.AppendLine("CurrentTileMovementTime".Translate() + ": " + Caravan_PathFollower.CostToDisplay(this, base.Tile, this.pather.nextTile, -1f).ToStringTicksToPeriod(true, false, true));
			stringBuilder.Append("StealthFactor".Translate() + ": " + CaravanIncidentUtility.CalculateCaravanStealthFactor(this.PawnsListForReading.Count).ToString("F1"));
			return stringBuilder.ToString();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.IsPlayerControlled)
			{
				if (CaravanMergeUtility.ShouldShowMergeCommand)
				{
					yield return (Gizmo)CaravanMergeUtility.MergeCommand(this);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (Find.WorldSelector.SingleSelectedObject == this && this.PawnsListForReading.Count((Func<Pawn, bool>)((Pawn x) => x.IsColonist)) >= 2)
				{
					yield return (Gizmo)new Command_Action
					{
						defaultLabel = "CommandSplitCaravan".Translate(),
						defaultDesc = "CommandSplitCaravanDesc".Translate(),
						icon = Caravan.SplitCommand,
						action = (Action)delegate
						{
							Find.WindowStack.Add(new Dialog_SplitCaravan(((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_01b4: stateMachine*/)._0024this));
						}
					};
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					yield return (Gizmo)SettleInEmptyTileUtility.SettleCommand(this);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				foreach (WorldObject item in Find.WorldObjects.ObjectsAt(base.Tile))
				{
					using (IEnumerator<Gizmo> enumerator3 = item.GetCaravanGizmos(this).GetEnumerator())
					{
						if (enumerator3.MoveNext())
						{
							Gizmo gizmo = enumerator3.Current;
							yield return gizmo;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "Dev: Mental break",
				action = (Action)delegate()
				{
					Pawn pawn = default(Pawn);
					if ((from x in ((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_035c: stateMachine*/)._0024this.PawnsListForReading
					where x.RaceProps.Humanlike && !x.InMentalState
					select x).TryRandomElement<Pawn>(out pawn))
					{
						pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.WanderSad, (string)null, false, false, null);
					}
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0594:
			/*Error near IL_0595: Unexpected return in MoveNext()*/;
		}

		public void RecacheImmobilizedNow()
		{
			this.cachedImmobilizedForTicks = -99999;
		}

		public void RecacheDaysWorthOfFood()
		{
			this.cachedDaysWorthOfFoodForTicks = -99999;
		}

		public virtual void Notify_MemberDied(Pawn member)
		{
			if (!this.PawnsListForReading.Any((Predicate<Pawn>)((Pawn x) => x != member && this.IsOwner(x))))
			{
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), (string)null);
				}
				Find.WorldObjects.Remove(this);
			}
			else
			{
				member.Strip();
				this.RemovePawn(member);
			}
		}

		private void CheckAnyNonWorldPawns()
		{
			for (int num = this.pawns.Count - 1; num >= 0; num--)
			{
				if (!this.pawns[num].IsWorldPawn())
				{
					Log.Error("Caravan member " + this.pawns[num] + " is not a world pawn. Removing...");
					this.pawns.Remove(this.pawns[num]);
				}
			}
		}

		public void Notify_PawnRemoved(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
		}

		public void Notify_PawnAdded(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheImmobilizedNow();
			this.RecacheDaysWorthOfFood();
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
