using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

		public Caravan_ForageTracker forage;

		public StoryState storyState;

		private Material cachedMat;

		private bool cachedImmobilized;

		private int cachedImmobilizedForTicks = -99999;

		private Pair<float, float> cachedDaysWorthOfFood;

		private int cachedDaysWorthOfFoodForTicks = -99999;

		public bool notifiedOutOfFood;

		private const int ImmobilizedCacheDuration = 60;

		private const int DaysWorthOfFoodCacheDuration = 3000;

		private const int TendIntervalTicks = 2000;

		private const int TryTakeScheduledDrugsIntervalTicks = 120;

		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache1;

		public Caravan()
		{
			this.pawns = new ThingOwner<Pawn>(this, false, LookMode.Reference);
			this.pather = new Caravan_PathFollower(this);
			this.gotoMote = new Caravan_GotoMoteRenderer();
			this.tweener = new Caravan_Tweener(this);
			this.trader = new Caravan_TraderTracker(this);
			this.forage = new Caravan_ForageTracker(this);
			this.storyState = new StoryState(this);
		}

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
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction == null)
					{
						color = Color.white;
					}
					else if (base.Faction.IsPlayer)
					{
						color = Caravan.PlayerCaravanColor;
					}
					else
					{
						color = base.Faction.Color;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.DynamicObjectRenderQueue);
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
				return this.Resting || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass;
			}
		}

		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, null);
			}
		}

		public string MassCapacityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage<Pawn>(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		public bool AllOwnersDowned
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].Downed)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool AllOwnersHaveMentalBreak
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool Resting
		{
			get
			{
				return (!this.pather.Moving || this.pather.nextTile != this.pather.Destination || !Caravan_PathFollower.IsValidFinalPushDestination(this.pather.Destination) || Mathf.CeilToInt(this.pather.nextTileCostLeft / 1f) > 10000) && CaravanRestUtility.RestingNowAt(base.Tile);
			}
		}

		public int LeftRestTicks
		{
			get
			{
				int result;
				if (!this.Resting)
				{
					result = 0;
				}
				else
				{
					result = CaravanRestUtility.LeftRestTicksAt(base.Tile);
				}
				return result;
			}
		}

		public int LeftNonRestTicks
		{
			get
			{
				int result;
				if (this.Resting)
				{
					result = 0;
				}
				else
				{
					result = CaravanRestUtility.LeftNonRestTicksAt(base.Tile);
				}
				return result;
			}
		}

		public override string Label
		{
			get
			{
				string label;
				if (this.nameInt != null)
				{
					label = this.nameInt;
				}
				else
				{
					label = base.Label;
				}
				return label;
			}
		}

		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this, null);
			}
		}

		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		public float Visibility
		{
			get
			{
				return CaravanVisibilityCalculator.Visibility(this, null);
			}
		}

		public string VisibilityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanVisibilityCalculator.Visibility(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		public string TicksPerMoveExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
				return stringBuilder.ToString();
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
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050, false);
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
						if (this.pawns[i].RaceProps.Animal && this.pawns[i].Faction == Faction.OfPlayer)
						{
							num += this.pawns[i].MarketValue;
						}
					}
					result = num * 0.5f;
				}
				return result;
			}
		}

		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				IEnumerable<Pawn> result;
				if (!this.IsPlayerControlled)
				{
					result = Enumerable.Empty<Pawn>();
				}
				else
				{
					result = from x in this.PawnsListForReading
					where x.Faction == Faction.OfPlayer
					select x;
				}
				return result;
			}
		}

		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.CaravanPointsRandomFactorRange;
			}
		}

		public IEnumerable<Thing> AllThings
		{
			get
			{
				return CaravanInventoryUtility.AllInventoryItems(this).Concat(this.pawns);
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
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawns, "pawns", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.autoJoinable, "autoJoinable", false, false);
			Scribe_Deep.Look<Caravan_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_ForageTracker>(ref this.forage, "forage", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
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
			this.forage.ForageTrackerTick();
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
			this.tweener.ResetTweenedPosToRoot();
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
				Log.Warning("Tried to add a null pawn to " + this, false);
			}
			else if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add ",
					p,
					" to ",
					this,
					", but this pawn is dead."
				}), false);
			}
			else
			{
				Pawn pawn = p.carryTracker.CarriedThing as Pawn;
				if (p.Spawned)
				{
					p.DeSpawn(DestroyMode.Vanish);
				}
				if (this.pawns.TryAdd(p, true))
				{
					if (this.ShouldAutoCapture(p))
					{
						p.guest.CapturedBy(base.Faction, null);
					}
					if (pawn != null)
					{
						p.carryTracker.innerContainer.Remove(pawn);
						if (this.ShouldAutoCapture(pawn))
						{
							pawn.guest.CapturedBy(base.Faction, p);
						}
						this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
						if (addCarriedPawnToWorldPawnsIfAny)
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						}
					}
				}
				else
				{
					Log.Error("Couldn't add pawn " + p + " to caravan.", false);
				}
			}
		}

		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + this, false);
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
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].IsColonist)
				{
					num++;
				}
				else if (this.pawns[i].RaceProps.Animal)
				{
					num2++;
				}
				else if (this.pawns[i].IsPrisoner)
				{
					num3++;
				}
				if (this.pawns[i].Downed)
				{
					num4++;
				}
				if (this.pawns[i].InMentalState)
				{
					num5++;
				}
			}
			stringBuilder.Append("CaravanColonistsCount".Translate(new object[]
			{
				num,
				(num != 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular
			}));
			if (num2 == 1)
			{
				stringBuilder.Append(", " + "CaravanAnimal".Translate());
			}
			else if (num2 > 1)
			{
				stringBuilder.Append(", " + "CaravanAnimalsCount".Translate(new object[]
				{
					num2
				}));
			}
			if (num3 == 1)
			{
				stringBuilder.Append(", " + "CaravanPrisoner".Translate());
			}
			else if (num3 > 1)
			{
				stringBuilder.Append(", " + "CaravanPrisonersCount".Translate(new object[]
				{
					num3
				}));
			}
			stringBuilder.AppendLine();
			if (num5 > 0)
			{
				stringBuilder.Append("CaravanPawnsInMentalState".Translate(new object[]
				{
					num5
				}));
			}
			if (num4 > 0)
			{
				if (num5 > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("CaravanPawnsDowned".Translate(new object[]
				{
					num4
				}));
			}
			if (num5 > 0 || num4 > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.pather.Moving)
			{
				if (this.pather.ArrivalAction != null)
				{
					stringBuilder.Append(this.pather.ArrivalAction.ReportString);
				}
				else
				{
					stringBuilder.Append("CaravanTraveling".Translate());
				}
			}
			else
			{
				SettlementBase settlementBase = CaravanVisitUtility.SettlementVisitedNow(this);
				if (settlementBase != null)
				{
					stringBuilder.Append("CaravanVisiting".Translate(new object[]
					{
						settlementBase.Label
					}));
				}
				else
				{
					stringBuilder.Append("CaravanWaiting".Translate());
				}
			}
			if (this.pather.Moving)
			{
				float num6 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, true) / 60000f;
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(new object[]
				{
					num6.ToString("0.#")
				}));
			}
			if (this.AllOwnersDowned)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersDowned".Translate());
			}
			else if (this.AllOwnersHaveMentalBreak)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
			}
			else if (this.ImmobilizedByMass)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanImmobilizedByMass".Translate());
			}
			string text;
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
			if (this.Resting)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanResting".Translate());
			}
			else if (this.pather.Paused)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanPaused".Translate());
			}
			return stringBuilder.ToString();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return new Gizmo_CaravanInfo(this);
			}
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.IsPlayerControlled)
			{
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					yield return SettleInEmptyTileUtility.SettleCommand(this);
				}
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					if (this.PawnsListForReading.Count((Pawn x) => x.IsColonist) >= 2)
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandSplitCaravan".Translate(),
							defaultDesc = "CommandSplitCaravanDesc".Translate(),
							icon = Caravan.SplitCommand,
							action = delegate()
							{
								Find.WindowStack.Add(new Dialog_SplitCaravan(this));
							}
						};
					}
				}
				if (this.pather.Moving)
				{
					yield return new Command_Toggle
					{
						hotKey = KeyBindingDefOf.Misc1,
						isActive = (() => this.pather.Paused),
						toggleAction = delegate()
						{
							if (this.pather.Moving)
							{
								this.pather.Paused = !this.pather.Paused;
							}
						},
						defaultDesc = "CommandToggleCaravanPauseDesc".Translate(new object[]
						{
							2f.ToString("0.#"),
							0.3f.ToStringPercent()
						}),
						icon = TexCommand.PauseCaravan,
						defaultLabel = "CommandPauseCaravan".Translate()
					};
				}
				if (CaravanMergeUtility.ShouldShowMergeCommand)
				{
					yield return CaravanMergeUtility.MergeCommand(this);
				}
				foreach (Gizmo g2 in this.forage.GetGizmos())
				{
					yield return g2;
				}
				foreach (WorldObject wo in Find.WorldObjects.ObjectsAt(base.Tile))
				{
					foreach (Gizmo gizmo in wo.GetCaravanGizmos(this))
					{
						yield return gizmo;
					}
				}
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Mental break",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where x.RaceProps.Humanlike && !x.InMentalState
						select x).TryRandomElement(out pawn))
						{
							pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Make random pawn hungry",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where x.needs.food != null
						select x).TryRandomElement(out pawn))
						{
							pawn.needs.food.CurLevelPercentage = 0f;
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Kill random pawn",
					action = delegate()
					{
						Pawn pawn;
						if (this.PawnsListForReading.TryRandomElement(out pawn))
						{
							pawn.Kill(null, null);
							Messages.Message("Dev: Killed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Harm random pawn",
					action = delegate()
					{
						Pawn pawn;
						if (this.PawnsListForReading.TryRandomElement(out pawn))
						{
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
							pawn.TakeDamage(dinfo);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Down random pawn",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn))
						{
							HealthUtility.DamageUntilDowned(pawn);
							Messages.Message("Dev: Downed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Teleport to destination",
					action = delegate()
					{
						base.Tile = this.pather.Destination;
						this.pather.StopDead();
					}
				};
			}
			yield break;
		}

		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy1(pods, representative))
			{
				yield return o;
			}
			foreach (FloatMenuOption f in TransportPodsArrivalAction_GiveToCaravan.GetFloatMenuOptions(representative, pods, this))
			{
				yield return f;
			}
			yield break;
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
			if (!this.PawnsListForReading.Any((Pawn x) => x != member && this.IsOwner(x)))
			{
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(new object[]
					{
						this.Name
					}).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null);
				}
				Find.WorldObjects.Remove(this);
			}
			else
			{
				member.Strip();
				this.RemovePawn(member);
			}
		}

		public virtual void Notify_Merged(List<Caravan> group)
		{
			this.notifiedOutOfFood = false;
		}

		public virtual void Notify_StartedTrading()
		{
			this.notifiedOutOfFood = false;
		}

		private void CheckAnyNonWorldPawns()
		{
			for (int i = this.pawns.Count - 1; i >= 0; i--)
			{
				if (!this.pawns[i].IsWorldPawn())
				{
					Log.Error("Caravan member " + this.pawns[i] + " is not a world pawn. Removing...", false);
					this.pawns.Remove(this.pawns[i]);
				}
			}
		}

		private bool ShouldAutoCapture(Pawn p)
		{
			return CaravanUtility.ShouldAutoCapture(p, base.Faction);
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

		public void Notify_DestinationOrPauseStatusChanged()
		{
			this.RecacheDaysWorthOfFood();
		}

		public void Notify_Teleported()
		{
			this.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Caravan()
		{
		}

		[CompilerGenerated]
		private static bool <get_PlayerPawnsForStoryteller>m__0(Pawn x)
		{
			return x.Faction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(Pawn x)
		{
			return x.Destroyed;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetTransportPodsFloatMenuOptions>__BaseCallProxy1(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			return base.GetTransportPodsFloatMenuOptions(pods, representative);
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <split>__2;

			internal Command_Toggle <pause>__3;

			internal IEnumerator<Gizmo> $locvar1;

			internal Gizmo <g>__4;

			internal IEnumerator<WorldObject> $locvar2;

			internal WorldObject <wo>__5;

			internal IEnumerator<Gizmo> $locvar3;

			internal Gizmo <gizmo>__6;

			internal Command_Action <mentalBreak>__7;

			internal Command_Action <makeHungry>__8;

			internal Command_Action <kill>__9;

			internal Command_Action <harm>__10;

			internal Command_Action <down>__11;

			internal Command_Action <action>__12;

			internal Caravan $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<Pawn, bool> <>f__am$cache0;

			private static Func<Pawn, bool> <>f__am$cache1;

			private static Func<Pawn, bool> <>f__am$cache2;

			private static Func<Pawn, bool> <>f__am$cache3;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
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
					if (Find.WorldSelector.SingleSelectedObject == this)
					{
						this.$current = new Gizmo_CaravanInfo(this);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_AC;
				case 3u:
					goto IL_16D;
				case 4u:
					goto IL_233;
				case 5u:
					goto IL_315;
				case 6u:
					IL_344:
					enumerator2 = this.forage.GetGizmos().GetEnumerator();
					num = 4294967293u;
					goto Block_16;
				case 7u:
					goto IL_363;
				case 8u:
					goto IL_3FD;
				case 9u:
				{
					Command_Action makeHungry = new Command_Action();
					makeHungry.defaultLabel = "Dev: Make random pawn hungry";
					makeHungry.action = delegate()
					{
						Pawn pawn;
						if ((from x in base.PawnsListForReading
						where x.needs.food != null
						select x).TryRandomElement(out pawn))
						{
							pawn.needs.food.CurLevelPercentage = 0f;
						}
					};
					this.$current = makeHungry;
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				}
				case 10u:
				{
					Command_Action kill = new Command_Action();
					kill.defaultLabel = "Dev: Kill random pawn";
					kill.action = delegate()
					{
						Pawn pawn;
						if (base.PawnsListForReading.TryRandomElement(out pawn))
						{
							pawn.Kill(null, null);
							Messages.Message("Dev: Killed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					};
					this.$current = kill;
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				}
				case 11u:
				{
					Command_Action harm = new Command_Action();
					harm.defaultLabel = "Dev: Harm random pawn";
					harm.action = delegate()
					{
						Pawn pawn;
						if (base.PawnsListForReading.TryRandomElement(out pawn))
						{
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
							pawn.TakeDamage(dinfo);
						}
					};
					this.$current = harm;
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				}
				case 12u:
				{
					Command_Action down = new Command_Action();
					down.defaultLabel = "Dev: Down random pawn";
					down.action = delegate()
					{
						Pawn pawn;
						if ((from x in base.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn))
						{
							HealthUtility.DamageUntilDowned(pawn);
							Messages.Message("Dev: Downed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					};
					this.$current = down;
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				}
				case 13u:
				{
					Command_Action action = new Command_Action();
					action.defaultLabel = "Dev: Teleport to destination";
					action.action = delegate()
					{
						base.Tile = this.pather.Destination;
						this.pather.StopDead();
					};
					this.$current = action;
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				}
				case 14u:
					goto IL_6F2;
				default:
					return false;
				}
				enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_AC:
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (!base.IsPlayerControlled)
				{
					goto IL_4E8;
				}
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					this.$current = SettleInEmptyTileUtility.SettleCommand(this);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_16D:
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					if (base.PawnsListForReading.Count((Pawn x) => x.IsColonist) >= 2)
					{
						Command_Action split = new Command_Action();
						split.defaultLabel = "CommandSplitCaravan".Translate();
						split.defaultDesc = "CommandSplitCaravanDesc".Translate();
						split.icon = Caravan.SplitCommand;
						split.action = delegate()
						{
							Find.WindowStack.Add(new Dialog_SplitCaravan(this));
						};
						this.$current = split;
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
				}
				IL_233:
				if (this.pather.Moving)
				{
					Command_Toggle pause = new Command_Toggle();
					pause.hotKey = KeyBindingDefOf.Misc1;
					pause.isActive = (() => this.pather.Paused);
					pause.toggleAction = delegate()
					{
						if (this.pather.Moving)
						{
							this.pather.Paused = !this.pather.Paused;
						}
					};
					pause.defaultDesc = "CommandToggleCaravanPauseDesc".Translate(new object[]
					{
						2f.ToString("0.#"),
						0.3f.ToStringPercent()
					});
					pause.icon = TexCommand.PauseCaravan;
					pause.defaultLabel = "CommandPauseCaravan".Translate();
					this.$current = pause;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_315:
				if (CaravanMergeUtility.ShouldShowMergeCommand)
				{
					this.$current = CaravanMergeUtility.MergeCommand(this);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				goto IL_344;
				Block_16:
				try
				{
					IL_363:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						g2 = enumerator2.Current;
						this.$current = g2;
						if (!this.$disposing)
						{
							this.$PC = 7;
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
				enumerator3 = Find.WorldObjects.ObjectsAt(base.Tile).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_3FD:
					switch (num)
					{
					case 8u:
						Block_38:
						try
						{
							switch (num)
							{
							}
							if (enumerator4.MoveNext())
							{
								gizmo = enumerator4.Current;
								this.$current = gizmo;
								if (!this.$disposing)
								{
									this.$PC = 8;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator4 != null)
								{
									enumerator4.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator3.MoveNext())
					{
						wo = enumerator3.Current;
						enumerator4 = wo.GetCaravanGizmos(this).GetEnumerator();
						num = 4294967293u;
						goto Block_38;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
				}
				IL_4E8:
				if (Prefs.DevMode)
				{
					Command_Action mentalBreak = new Command_Action();
					mentalBreak.defaultLabel = "Dev: Mental break";
					mentalBreak.action = delegate()
					{
						Pawn pawn;
						if ((from x in base.PawnsListForReading
						where x.RaceProps.Humanlike && !x.InMentalState
						select x).TryRandomElement(out pawn))
						{
							pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, false);
						}
					};
					this.$current = mentalBreak;
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				}
				IL_6F2:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				case 2u:
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
				case 7u:
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
				case 8u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator4 != null)
							{
								enumerator4.Dispose();
							}
						}
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Caravan.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Caravan.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			private static bool <>m__0(Pawn x)
			{
				return x.IsColonist;
			}

			internal void <>m__1()
			{
				Find.WindowStack.Add(new Dialog_SplitCaravan(this));
			}

			internal bool <>m__2()
			{
				return this.pather.Paused;
			}

			internal void <>m__3()
			{
				if (this.pather.Moving)
				{
					this.pather.Paused = !this.pather.Paused;
				}
			}

			internal void <>m__4()
			{
				Pawn pawn;
				if ((from x in base.PawnsListForReading
				where x.RaceProps.Humanlike && !x.InMentalState
				select x).TryRandomElement(out pawn))
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, false);
				}
			}

			internal void <>m__5()
			{
				Pawn pawn;
				if ((from x in base.PawnsListForReading
				where x.needs.food != null
				select x).TryRandomElement(out pawn))
				{
					pawn.needs.food.CurLevelPercentage = 0f;
				}
			}

			internal void <>m__6()
			{
				Pawn pawn;
				if (base.PawnsListForReading.TryRandomElement(out pawn))
				{
					pawn.Kill(null, null);
					Messages.Message("Dev: Killed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
				}
			}

			internal void <>m__7()
			{
				Pawn pawn;
				if (base.PawnsListForReading.TryRandomElement(out pawn))
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
					pawn.TakeDamage(dinfo);
				}
			}

			internal void <>m__8()
			{
				Pawn pawn;
				if ((from x in base.PawnsListForReading
				where !x.Downed
				select x).TryRandomElement(out pawn))
				{
					HealthUtility.DamageUntilDowned(pawn);
					Messages.Message("Dev: Downed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
				}
			}

			internal void <>m__9()
			{
				base.Tile = this.pather.Destination;
				this.pather.StopDead();
			}

			private static bool <>m__A(Pawn x)
			{
				return x.RaceProps.Humanlike && !x.InMentalState;
			}

			private static bool <>m__B(Pawn x)
			{
				return x.needs.food != null;
			}

			private static bool <>m__C(Pawn x)
			{
				return !x.Downed;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptions>c__Iterator1 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal IEnumerable<IThingHolder> pods;

			internal CompLaunchable representative;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal Caravan $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptions>c__Iterator1()
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
					enumerator = base.<GetTransportPodsFloatMenuOptions>__BaseCallProxy1(pods, representative).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_EA;
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
						o = enumerator.Current;
						this.$current = o;
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
				enumerator2 = TransportPodsArrivalAction_GiveToCaravan.GetFloatMenuOptions(representative, pods, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_EA:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f = enumerator2.Current;
						this.$current = f;
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
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Caravan.<GetTransportPodsFloatMenuOptions>c__Iterator1 <GetTransportPodsFloatMenuOptions>c__Iterator = new Caravan.<GetTransportPodsFloatMenuOptions>c__Iterator1();
				<GetTransportPodsFloatMenuOptions>c__Iterator.$this = this;
				<GetTransportPodsFloatMenuOptions>c__Iterator.pods = pods;
				<GetTransportPodsFloatMenuOptions>c__Iterator.representative = representative;
				return <GetTransportPodsFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <Notify_MemberDied>c__AnonStorey2
		{
			internal Pawn member;

			internal Caravan $this;

			public <Notify_MemberDied>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return x != this.member && this.$this.IsOwner(x);
			}
		}
	}
}
