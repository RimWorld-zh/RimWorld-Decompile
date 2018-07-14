using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, IAttackTargetSearcher, IThingHolder, ILoadReferenceable
	{
		public PawnKindDef kindDef;

		private Name nameInt;

		public Gender gender = Gender.None;

		public Pawn_AgeTracker ageTracker;

		public Pawn_HealthTracker health;

		public Pawn_RecordsTracker records;

		public Pawn_InventoryTracker inventory;

		public Pawn_MeleeVerbs meleeVerbs;

		public VerbTracker verbTracker;

		public Pawn_CarryTracker carryTracker;

		public Pawn_NeedsTracker needs;

		public Pawn_MindState mindState;

		public Pawn_RotationTracker rotationTracker;

		public Pawn_PathFollower pather;

		public Pawn_Thinker thinker;

		public Pawn_JobTracker jobs;

		public Pawn_StanceTracker stances;

		public Pawn_NativeVerbs natives;

		public Pawn_FilthTracker filth;

		public Pawn_EquipmentTracker equipment;

		public Pawn_ApparelTracker apparel;

		public Pawn_Ownership ownership;

		public Pawn_SkillTracker skills;

		public Pawn_StoryTracker story;

		public Pawn_GuestTracker guest;

		public Pawn_GuiltTracker guilt;

		public Pawn_WorkSettings workSettings;

		public Pawn_TraderTracker trader;

		public Pawn_TrainingTracker training;

		public Pawn_CallTracker caller;

		public Pawn_RelationsTracker relations;

		public Pawn_InteractionsTracker interactions;

		public Pawn_PlayerSettings playerSettings;

		public Pawn_OutfitTracker outfits;

		public Pawn_DrugPolicyTracker drugs;

		public Pawn_TimetableTracker timetable;

		public Pawn_DraftController drafter;

		private Pawn_DrawTracker drawer;

		private const float HumanSizedHeatOutput = 0.3f;

		private const float AnimalHeatOutputFactor = 0.6f;

		private static string NotSurgeryReadyTrans;

		private static string CannotReachTrans;

		public const int MaxMoveTicks = 450;

		private static List<string> states = new List<string>();

		private int lastSleepDisturbedTick = 0;

		private const int SleepDisturbanceMinInterval = 300;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache4;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache5;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache6;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<LifeStageAge, SoundDef> <>f__am$cache8;

		[CompilerGenerated]
		private static Predicate<DirectPawnRelation> <>f__am$cache9;

		public Pawn()
		{
		}

		public Name Name
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

		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		public Job CurJob
		{
			get
			{
				return (this.jobs == null) ? null : this.jobs.curJob;
			}
		}

		public JobDef CurJobDef
		{
			get
			{
				return (this.CurJob == null) ? null : this.CurJob.def;
			}
		}

		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		public MentalState MentalState
		{
			get
			{
				MentalState result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.mentalStateHandler.CurState;
				}
				return result;
			}
		}

		public MentalStateDef MentalStateDef
		{
			get
			{
				MentalStateDef result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.mentalStateHandler.CurStateDef;
				}
				return result;
			}
		}

		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		public bool Inspired
		{
			get
			{
				return !this.Dead && this.mindState.inspirationHandler.Inspired;
			}
		}

		public Inspiration Inspiration
		{
			get
			{
				Inspiration result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.inspirationHandler.CurState;
				}
				return result;
			}
		}

		public InspirationDef InspirationDef
		{
			get
			{
				InspirationDef result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.inspirationHandler.CurStateDef;
				}
				return result;
			}
		}

		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike;
			}
		}

		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		public Faction HostFaction
		{
			get
			{
				Faction result;
				if (this.guest == null)
				{
					result = null;
				}
				else
				{
					result = this.guest.HostFaction;
				}
				return result;
			}
		}

		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && this.HostFaction == null;
			}
		}

		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		public Pawn CarriedBy
		{
			get
			{
				Pawn result;
				if (base.ParentHolder == null)
				{
					result = null;
				}
				else
				{
					Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						result = pawn_CarryTracker.pawn;
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		public override string LabelNoCount
		{
			get
			{
				string result;
				if (this.Name != null)
				{
					if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
					{
						result = this.Name.ToStringShort;
					}
					else
					{
						result = this.Name.ToStringShort + ", " + this.story.TitleShortCap;
					}
				}
				else
				{
					result = this.KindLabel;
				}
				return result;
			}
		}

		public override string LabelShort
		{
			get
			{
				string result;
				if (this.Name != null)
				{
					result = this.Name.ToStringShort;
				}
				else
				{
					result = this.LabelNoCount;
				}
				return result;
			}
		}

		public Pawn_DrawTracker Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new Pawn_DrawTracker(this);
				}
				return this.drawer;
			}
		}

		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				if (building_Bed != null)
				{
					IntVec3 position = base.Position;
					IntVec3 position2 = base.Position;
					IntVec3 position3 = base.Position;
					IntVec3 position4 = base.Position;
					if (building_Bed.Rotation.IsHorizontal)
					{
						position.z++;
						position2.z--;
						position3.x--;
						position4.x++;
					}
					else
					{
						position.x--;
						position2.x++;
						position3.z++;
						position4.z--;
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position.GetDoor(base.Map) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position2.GetDoor(base.Map) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position3.GetDoor(base.Map) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position4.GetDoor(base.Map) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						return position;
					}
					if (position2.Standable(base.Map))
					{
						return position2;
					}
					if (position3.Standable(base.Map))
					{
						return position3;
					}
					if (position4.Standable(base.Map))
					{
						return position4;
					}
				}
				return base.InteractionCell;
			}
		}

		public TraderKindDef TraderKind
		{
			get
			{
				return (this.trader == null) ? null : this.trader.traderKind;
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
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				LocalTargetInfo result;
				if (!base.Spawned)
				{
					result = LocalTargetInfo.Invalid;
				}
				else
				{
					Stance curStance = this.stances.curStance;
					if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
					{
						result = ((Stance_Busy)curStance).focusTarg;
					}
					else
					{
						result = LocalTargetInfo.Invalid;
					}
				}
				return result;
			}
		}

		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				Verb result;
				if (building_Turret != null)
				{
					result = building_Turret.AttackVerb;
				}
				else
				{
					result = this.TryGetAttackVerb(null, !this.IsColonist);
				}
				return result;
			}
		}

		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		public int GetRootTile()
		{
			return base.Tile;
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", new object[0]);
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[0]);
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.PawnPostLoadInit(this);
			}
		}

		public override string ToString()
		{
			string result;
			if (this.story != null)
			{
				result = this.LabelShort;
			}
			else if (this.thingIDNumber > 0)
			{
				result = base.ThingID;
			}
			else if (this.kindDef != null)
			{
				result = this.KindLabel + "_" + base.ThingID;
			}
			else if (this.def != null)
			{
				result = base.ThingID;
			}
			else
			{
				result = base.GetType().ToString();
			}
			return result;
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.", false);
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
			}
			else if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".", false);
			}
			else
			{
				base.SpawnSetup(map, respawningAfterLoad);
				if (Find.WorldPawns.Contains(this))
				{
					Find.WorldPawns.RemovePawn(this);
				}
				PawnComponentsUtility.AddComponentsForSpawn(this);
				if (!PawnUtility.InValidState(this))
				{
					Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...", false);
					try
					{
						this.DeSpawn(DestroyMode.Vanish);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Tried to despawn ",
							this.ToStringSafe<Pawn>(),
							" because of the previous error but couldn't: ",
							ex
						}), false);
					}
					Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				}
				else
				{
					this.Drawer.Notify_Spawned();
					this.rotationTracker.Notify_Spawned();
					if (!respawningAfterLoad)
					{
						this.pather.ResetToCurrentPosition();
					}
					base.Map.mapPawns.RegisterPawn(this);
					if (this.RaceProps.IsFlesh)
					{
						this.relations.everSeenByPlayer = true;
					}
					AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
					if (this.needs != null && this.needs.mood != null && this.needs.mood.recentMemory != null)
					{
						this.needs.mood.recentMemory.Notify_Spawned(respawningAfterLoad);
					}
					if (!respawningAfterLoad)
					{
						this.records.AccumulateStoryEvent(StoryEventDefOf.Seen);
						Find.GameEnder.CheckOrUpdateGameOver();
						PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
					}
				}
			}
		}

		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Drawer.DrawAt(drawLoc);
		}

		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
		}

		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsColonistPlayerControlled)
			{
				if (this.pather.curPath != null)
				{
					this.pather.curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		public override void TickRare()
		{
			base.TickRare();
			if (!base.Suspended)
			{
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTickRare();
				}
				this.inventory.InventoryTrackerTickRare();
			}
			if (this.training != null)
			{
				this.training.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.16666651f * ((!this.def.race.Humanlike) ? 0.6f : 1f));
			}
		}

		public override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.RaceProps.Animal)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				base.Tick();
				if (Find.TickManager.TicksGame % 250 == 0)
				{
					this.TickRare();
				}
				bool suspended = base.Suspended;
				if (!suspended)
				{
					if (base.Spawned)
					{
						this.pather.PatherTick();
					}
					if (base.Spawned)
					{
						this.stances.StanceTrackerTick();
						this.verbTracker.VerbsTick();
						this.natives.NativeVerbsTick();
					}
					if (base.Spawned)
					{
						Profiler.BeginSample("jobs");
						this.jobs.JobTrackerTick();
						Profiler.EndSample();
					}
					if (base.Spawned)
					{
						Profiler.BeginSample("Drawer");
						this.Drawer.DrawTrackerTick();
						Profiler.EndSample();
						Profiler.BeginSample("rotationTracker");
						this.rotationTracker.RotationTrackerTick();
						Profiler.EndSample();
					}
					Profiler.BeginSample("health");
					this.health.HealthTick();
					Profiler.EndSample();
					if (!this.Dead)
					{
						Profiler.BeginSample("mindState");
						this.mindState.MindStateTick();
						Profiler.EndSample();
						this.carryTracker.CarryHandsTick();
					}
				}
				if (!this.Dead)
				{
					this.needs.NeedsTrackerTick();
				}
				if (!suspended)
				{
					if (this.equipment != null)
					{
						Profiler.BeginSample("equipment");
						this.equipment.EquipmentTrackerTick();
						Profiler.EndSample();
					}
					if (this.apparel != null)
					{
						this.apparel.ApparelTrackerTick();
					}
					if (this.interactions != null && base.Spawned)
					{
						Profiler.BeginSample("interactions");
						this.interactions.InteractionsTrackerTick();
						Profiler.EndSample();
					}
					if (this.caller != null)
					{
						this.caller.CallTrackerTick();
					}
					if (this.skills != null)
					{
						this.skills.SkillsTick();
					}
					if (this.inventory != null)
					{
						this.inventory.InventoryTrackerTick();
					}
					if (this.drafter != null)
					{
						this.drafter.DraftControllerTick();
					}
					if (this.relations != null)
					{
						this.relations.RelationsTrackerTick();
					}
					if (this.RaceProps.Humanlike)
					{
						this.guest.GuestTrackerTick();
					}
					this.ageTracker.AgeTick();
					this.records.RecordsTick();
				}
			}
		}

		public void TickMothballed(int interval)
		{
			if (!base.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		public void Notify_Teleported(bool endCurrentJob = true)
		{
			this.Drawer.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob && this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			}
		}

		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind(false);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PassedToWorld();
			}
		}

		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				if (this.story != null && this.story.traits.HasTrait(TraitDefOf.Tough) && dinfo.Def.externalViolence)
				{
					dinfo.SetAmount(dinfo.Amount * 0.6f);
				}
				this.health.PreApplyDamage(dinfo, out absorbed);
			}
		}

		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.externalViolence)
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.records.AccumulateStoryEvent(StoryEventDefOf.DamageTaken);
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
			}
		}

		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		public int TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		public int TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		private int TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true);
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			int value = Mathf.RoundToInt(num3);
			return Mathf.Clamp(value, 1, 450);
		}

		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map map = base.Map;
			Map mapHeld = base.MapHeld;
			bool flag = base.Spawned;
			bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
			bool wasWorldPawn = this.IsWorldPawn();
			Caravan caravan = this.GetCaravan();
			Building_Grave assignedGrave = null;
			if (this.ownership != null)
			{
				assignedGrave = this.ownership.AssignedGrave;
			}
			bool flag2 = this.InBed();
			float bedRotation = 0f;
			if (flag2)
			{
				bedRotation = this.CurrentBed().Rotation.AsAngle;
			}
			ThingOwner thingOwner = null;
			bool inContainerEnclosed = this.InContainerEnclosed;
			if (inContainerEnclosed)
			{
				thingOwner = this.holdingOwner;
				thingOwner.Remove(this);
			}
			bool flag3 = false;
			bool flag4 = false;
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
				flag4 = (map.designationManager.DesignationOn(this, DesignationDefOf.Slaughter) != null);
			}
			bool flag5 = PawnUtility.ShouldSendNotificationAbout(this) && (!flag4 || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut);
			float num = 0f;
			Thing attachment = this.GetAttachment(ThingDefOf.Fire);
			if (attachment != null)
			{
				num = ((Fire)attachment).CurrentSize();
			}
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(this, AdaptationEvent.Died, null);
			}
			if (this.IsColonist)
			{
				Find.StoryWatcher.statsRecord.colonistsKilled++;
			}
			if (flag && dinfo != null && dinfo.Value.Def.externalViolence)
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge ls) => ls.soundDeath, 1f);
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					if (this.IsColonist)
					{
						pawn.records.AccumulateStoryEvent(StoryEventDefOf.KilledPlayer);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (flag)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, (dinfo == null) ? null : (dinfo.Value.Instigator as Pawn), exactCulprit, (dinfo == null) ? null : dinfo.Value.HitPart));
			}
			this.health.surgeryBills.Clear();
			if (this.apparel != null)
			{
				this.apparel.Notify_PawnKilled(dinfo);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKilled(dinfo, map);
			}
			this.meleeVerbs.Notify_PawnKilled();
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Thing thing;
				if (this.holdingOwner.TryDrop(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null))
				{
					map = pawn_CarryTracker.pawn.Map;
					flag = true;
				}
			}
			this.health.SetDead();
			if (this.health.deflectionEffecter != null)
			{
				this.health.deflectionEffecter.Cleanup();
				this.health.deflectionEffecter = null;
			}
			if (caravan != null)
			{
				caravan.Notify_MemberDied(this);
			}
			if (flag)
			{
				this.DropAndForbidEverything(false);
			}
			if (flag)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			Corpse corpse = null;
			if (!PawnGenerator.IsBeingGenerated(this))
			{
				if (inContainerEnclosed)
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (!thingOwner.TryAdd(corpse, true))
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (spawnedOrAnyParentSpawned)
				{
					if (this.holdingOwner != null)
					{
						this.holdingOwner.Remove(this);
					}
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null, null))
					{
						corpse.Rotation = base.Rotation;
						if (HuntJobUtility.WasKilledByHunter(this, dinfo))
						{
							((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null);
						}
						else if (!flag3 && !flag4)
						{
							corpse.SetForbiddenIfOutsideHomeArea();
						}
						if (num > 0f)
						{
							FireUtility.TryStartFireIn(corpse.Position, corpse.Map, num);
						}
					}
					else
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (caravan != null && caravan.Spawned)
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					caravan.AddPawnOrItem(corpse, true);
				}
				else if (this.holdingOwner != null || this.IsWorldPawn())
				{
					Corpse.PostCorpseDestroy(this);
				}
				else
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
				}
			}
			if (corpse != null)
			{
				Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				CompRottable comp = corpse.GetComp<CompRottable>();
				if (firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity && comp != null)
				{
					comp.RotImmediately();
				}
			}
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.KillFinalize);
			}
			PawnComponentsUtility.RemoveComponentsOnKilled(this);
			this.health.hediffSet.DirtyCache();
			PortraitsCache.SetDirty(this);
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnDied();
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberDied(this, dinfo, wasWorldPawn, mapHeld);
			}
			if (corpse != null)
			{
				if (this.RaceProps.DeathActionWorker != null && flag)
				{
					this.RaceProps.DeathActionWorker.PawnDied(corpse);
				}
				if (Find.Scenario != null)
				{
					Find.Scenario.Notify_PawnDied(corpse);
				}
			}
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (spawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this, mapHeld);
			}
			if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (flag5)
			{
				this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
			}
		}

		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new object[]
				{
					"Destroyed pawn ",
					this,
					" with unsupported mode ",
					mode,
					"."
				}), false);
			}
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			this.ClearMind(false);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Lord lord = this.GetLord();
				if (lord != null)
				{
					PawnLostCondition cond = (mode != DestroyMode.KillFinalize) ? PawnLostCondition.Vanished : PawnLostCondition.IncappedOrKilled;
					lord.Notify_PawnLost(this, cond);
				}
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			if (mode != DestroyMode.KillFinalize)
			{
				if (this.equipment != null)
				{
					this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				}
				this.inventory.DestroyAll(DestroyMode.Vanish);
				if (this.apparel != null)
				{
					this.apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
		}

		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			if (this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.StopAll(false);
			}
			base.DeSpawn(mode);
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.needs != null && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (this.meleeVerbs != null)
			{
				this.meleeVerbs.Notify_PawnDespawned();
			}
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
		}

		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".", false);
			}
			else
			{
				base.Discard(silentlyRemoveReferences);
				if (this.relations != null)
				{
					this.relations.ClearAllRelations();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
					Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
					Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.Notify_PawnDiscarded(this);
					}
				}
				Corpse.PostCorpseDestroy(this);
			}
		}

		private Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			Corpse result;
			if (this.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder, false);
				result = null;
			}
			else
			{
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				if (assignedGrave != null)
				{
					corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
				}
				if (inBed)
				{
					corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
				}
				result = corpse;
			}
			return result;
		}

		public void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this, false);
			}
			else if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
			}
			else
			{
				Lord lord = this.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap);
				}
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Pawn pawn = this.carryTracker.CarriedThing as Pawn;
					if (pawn != null)
					{
						if (base.Faction != null && base.Faction != pawn.Faction)
						{
							base.Faction.kidnapped.KidnapPawn(pawn, this);
						}
						else
						{
							this.carryTracker.innerContainer.Remove(pawn);
							pawn.ExitMap(false, exitDir);
						}
					}
					else
					{
						this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
					}
					this.carryTracker.innerContainer.Clear();
				}
				bool flag = !this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this);
				if (base.Faction != null)
				{
					base.Faction.Notify_MemberExitedMap(this, flag);
				}
				if (this.ownership != null)
				{
					this.ownership.UnclaimAll();
				}
				if (this.guest != null)
				{
					if (flag)
					{
						this.guest.SetGuestStatus(null, false);
					}
					this.guest.Released = false;
				}
				if (base.Spawned)
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				this.inventory.UnloadEverything = false;
				if (flag)
				{
					this.ClearMind(false);
				}
				if (this.relations != null)
				{
					this.relations.Notify_ExitedMap();
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
		}

		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false);
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (action == TradeAction.PlayerBuys)
			{
				this.SetFaction(Faction.OfPlayer, null);
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
				if (this.RaceProps.Humanlike)
				{
					foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, null);
					}
				}
			}
			this.ClearMind(false);
		}

		public void PreKidnapped(Pawn kidnapper)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(this, AdaptationEvent.Kidnapped, null);
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind(false);
		}

		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>(), false);
			}
			else
			{
				Faction faction = base.Faction;
				if (this.guest != null)
				{
					this.guest.SetGuestStatus(null, false);
				}
				if (base.Spawned)
				{
					base.Map.mapPawns.DeRegisterPawn(this);
					base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
					base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				}
				if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				Lord lord = this.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction);
				}
				if (base.Faction != null && base.Faction.leader == this)
				{
					base.Faction.Notify_LeaderLost();
				}
				if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike)
				{
					this.ChangeKind(newFaction.def.basicMemberKind);
				}
				base.SetFaction(newFaction, null);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
				if (base.Faction != null && base.Faction.IsPlayer)
				{
					if (this.workSettings != null)
					{
						this.workSettings.EnableAndInitialize();
					}
					Find.Storyteller.intenderPopulation.Notify_PopulationGained();
				}
				if (this.Drafted)
				{
					this.drafter.Drafted = false;
				}
				ReachabilityUtility.ClearCache();
				this.health.surgeryBills.Clear();
				if (base.Spawned)
				{
					base.Map.mapPawns.RegisterPawn(this);
				}
				this.GenerateNecessaryName();
				if (this.playerSettings != null)
				{
					this.playerSettings.ResetMedicalCare();
				}
				this.ClearMind(true);
				if (!this.Dead && this.needs.mood != null)
				{
					this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
				if (base.Spawned)
				{
					base.Map.attackTargetsCache.UpdateTarget(this);
				}
				Find.GameEnder.CheckOrUpdateGameOver();
				AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
				if (this.needs != null)
				{
					this.needs.AddOrRemoveNeedsAsAppropriate();
				}
				if (this.playerSettings != null)
				{
					this.playerSettings.Notify_FactionChanged();
				}
				if (this.relations != null)
				{
					this.relations.Notify_ChangedFaction();
				}
				if (this.RaceProps.Animal && newFaction == Faction.OfPlayer)
				{
					this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
					this.training.Train(TrainableDefOf.Tameness, recruiter, true);
				}
				if (faction == Faction.OfPlayer)
				{
					BillUtility.Notify_ColonistUnavailable(this);
				}
			}
		}

		public void ClearMind(bool ifLayingKeepLaying = false)
		{
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.mindState != null)
			{
				this.mindState.Reset();
			}
			if (this.jobs != null)
			{
				this.jobs.StopAll(ifLayingKeepLaying);
			}
			this.VerifyReservations();
		}

		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		public void VerifyReservations()
		{
			if (this.jobs != null)
			{
				if (this.CurJob == null && this.jobs.jobQueue.Count <= 0 && !this.jobs.startingNewJob)
				{
					bool flag = false;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
						if (obj.IsValid)
						{
							Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>()), 97771429 ^ this.thingIDNumber, false);
							flag = true;
						}
						LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
						if (obj2.IsValid)
						{
							Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj2.ToStringSafe<LocalTargetInfo>()), 19586765 ^ this.thingIDNumber, false);
							flag = true;
						}
						IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
						if (attackTarget != null)
						{
							Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), attackTarget.ToStringSafe<IAttackTarget>()), 100495878 ^ this.thingIDNumber, false);
							flag = true;
						}
						IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
						if (obj3.IsValid)
						{
							Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
							Log.ErrorOnce(string.Format("Pawn destination reservation manager failed to clean up properly; {0}/{1}/{2} still reserving {3}", new object[]
							{
								this.ToStringSafe<Pawn>(),
								job.ToStringSafe<Job>(),
								job.def.ToStringSafe<JobDef>(),
								obj3.ToStringSafe<IntVec3>()
							}), 1958674 ^ this.thingIDNumber, false);
							flag = true;
						}
					}
					if (flag)
					{
						this.ClearAllReservations(true);
					}
				}
			}
		}

		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (this.InContainerEnclosed)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
				}
				if (this.equipment != null && this.equipment.Primary != null)
				{
					this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
				}
				if (this.inventory != null)
				{
					this.inventory.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
				}
			}
			else if (base.SpawnedOrAnyParentSpawned)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
				{
					if (this.equipment != null)
					{
						this.equipment.DropAllEquipment(base.PositionHeld, true);
					}
					if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
					{
						this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
					}
				}
			}
		}

		public void GenerateNecessaryName()
		{
			if (base.Faction == Faction.OfPlayer && this.RaceProps.Animal)
			{
				if (this.Name == null || this.Name.Numerical)
				{
					if (Rand.Value < this.RaceProps.nameOnTameChance)
					{
						this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Full, null);
					}
					else
					{
						this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null);
					}
				}
			}
		}

		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false)
		{
			Verb result;
			if (this.equipment != null && this.equipment.Primary != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				result = this.equipment.PrimaryEq.PrimaryVerb;
			}
			else
			{
				result = this.meleeVerbs.TryGetMeleeVerb(target);
			}
			return result;
		}

		public bool TryStartAttack(LocalTargetInfo targ)
		{
			bool result;
			if (this.stances.FullBodyBusy)
			{
				result = false;
			}
			else if (this.story != null && this.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = false;
			}
			else
			{
				bool allowManualCastWeapons = !this.IsColonist;
				Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons);
				result = (verb != null && verb.TryStartCastOn(targ, false, true));
			}
			return result;
		}

		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.RaceProps.meatDef != null)
			{
				int meatCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true) * efficiency);
				if (meatCount > 0)
				{
					Thing meat = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					meat.stackCount = meatCount;
					yield return meat;
				}
			}
			foreach (Thing t in this.<ButcherProducts>__BaseCallProxy0(butcher, efficiency))
			{
				yield return t;
			}
			if (this.RaceProps.leatherDef != null)
			{
				int leatherCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true) * efficiency);
				if (leatherCount > 0)
				{
					Thing leather = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					leather.stackCount = leatherCount;
					yield return leather;
				}
			}
			if (!this.RaceProps.Humanlike)
			{
				PawnKindLifeStage lifeStage = this.ageTracker.CurKindLifeStage;
				if (lifeStage.butcherBodyPart != null)
				{
					if (this.gender == Gender.None || (this.gender == Gender.Male && lifeStage.butcherBodyPart.allowMale) || (this.gender == Gender.Female && lifeStage.butcherBodyPart.allowFemale))
					{
						for (;;)
						{
							BodyPartRecord record = (from x in this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
							where x.IsInGroup(lifeStage.butcherBodyPart.bodyPartGroup)
							select x).FirstOrDefault<BodyPartRecord>();
							if (record == null)
							{
								break;
							}
							this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, record), null, null, null);
							Thing thing;
							if (lifeStage.butcherBodyPart.thing != null)
							{
								thing = ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing, null);
							}
							else
							{
								thing = ThingMaker.MakeThing(record.def.spawnThingOnRemoved, null);
							}
							yield return thing;
						}
					}
				}
			}
			yield break;
		}

		public string MainDesc(bool writeAge)
		{
			string text = GenLabel.BestKindLabel(this, true, true, false, -1);
			if (base.Faction != null && !base.Faction.def.hidden)
			{
				text = "PawnMainDescFactionedWrap".Translate(new object[]
				{
					text,
					base.Faction.Name
				});
			}
			if (writeAge && this.ageTracker != null)
			{
				text = text + ", " + "AgeIndicator".Translate(new object[]
				{
					this.ageTracker.AgeNumberString
				});
			}
			return text.CapitalizeFirst();
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.MainDesc(true));
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				stringBuilder.AppendLine(this.MentalState.InspectLine);
			}
			Pawn.states.Clear();
			if (this.stances != null && this.stances.stunner != null && this.stances.stunner.Stunned)
			{
				Pawn.states.AddDistinct("StunLower".Translate());
			}
			if (this.health != null && this.health.hediffSet != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddDistinct(hediff.def.battleStateLabel);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false).CapitalizeFirst()));
				Pawn.states.Clear();
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine("Equipped".Translate() + ": " + ((this.equipment.Primary == null) ? "EquippedNothing".Translate() : this.equipment.Primary.Label).CapitalizeFirst());
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			string text = null;
			Lord lord = this.GetLord();
			if (lord != null && lord.LordJob != null)
			{
				text = lord.LordJob.GetReport();
			}
			if (this.jobs.curJob != null)
			{
				try
				{
					string text2 = this.jobs.curDriver.GetReport().CapitalizeFirst();
					if (!text.NullOrEmpty())
					{
						text = text + ": " + text2;
					}
					else
					{
						text = text2;
					}
				}
				catch (Exception arg)
				{
					Log.Error("JobDriver.GetReport() exception: " + arg, false);
				}
			}
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine(text);
			}
			if (this.jobs.curJob != null && this.jobs.jobQueue.Count > 0)
			{
				try
				{
					string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
					if (this.jobs.jobQueue.Count > 1)
					{
						string text4 = text3;
						text3 = string.Concat(new object[]
						{
							text4,
							" (+",
							this.jobs.jobQueue.Count - 1,
							")"
						});
					}
					stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
				}
				catch (Exception arg2)
				{
					Log.Error("JobDriver.GetReport() exception: " + arg2, false);
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			stringBuilder.Append(base.InspectStringPartsFromComps());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.IsColonistPlayerControlled)
			{
				foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy1())
				{
					yield return c;
				}
				if (this.drafter != null)
				{
					foreach (Gizmo c2 in this.drafter.GetGizmos())
					{
						yield return c2;
					}
				}
				foreach (Gizmo attack in PawnAttackGizmoUtility.GetAttackGizmos(this))
				{
					yield return attack;
				}
			}
			if (this.equipment != null)
			{
				foreach (Gizmo g in this.equipment.GetGizmos())
				{
					yield return g;
				}
			}
			if (this.IsColonistPlayerControlled)
			{
				if (this.apparel != null)
				{
					foreach (Gizmo g2 in this.apparel.GetGizmos())
					{
						yield return g2;
					}
				}
				if (this.playerSettings != null)
				{
					foreach (Gizmo g3 in this.playerSettings.GetGizmos())
					{
						yield return g3;
					}
				}
			}
			foreach (Gizmo g4 in this.mindState.GetGizmos())
			{
				yield return g4;
			}
			yield break;
		}

		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			yield break;
		}

		public override TipSignal GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.LabelCap);
			string text = "";
			if (this.gender != Gender.None)
			{
				text = this.gender.GetLabel();
			}
			if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				if (text != "")
				{
					text += " ";
				}
				text += this.KindLabel;
			}
			if (text != "")
			{
				stringBuilder.Append(" (" + text + ")");
			}
			stringBuilder.AppendLine();
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine(this.equipment.Primary.LabelCap);
			}
			stringBuilder.AppendLine(HealthUtility.GetGeneralConditionLabel(this, false));
			return new TipSignal(stringBuilder.ToString().TrimEndNewlines(), this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry s in this.<SpecialDisplayStats>__BaseCallProxy2())
			{
				yield return s;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), 0, "");
			yield break;
		}

		public bool CurrentlyUsableForBills()
		{
			bool result;
			if (!this.InBed() && (this.RaceProps.FleshType.requiresBedForSurgery || !this.Downed))
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				result = false;
			}
			else if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		public bool AnythingToStrip()
		{
			return (this.equipment != null && this.equipment.HasAnything()) || (this.apparel != null && this.apparel.WornApparelCount > 0) || (this.inventory != null && this.inventory.innerContainer.Count > 0);
		}

		public void Strip()
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				IntVec3 pos = (this.Corpse == null) ? base.PositionHeld : this.Corpse.PositionHeld;
				if (this.equipment != null)
				{
					this.equipment.DropAllEquipment(pos, false);
				}
				if (this.apparel != null)
				{
					this.apparel.DropAll(pos, false);
				}
				if (this.inventory != null)
				{
					this.inventory.DropAllNearPawn(pos, false, false);
				}
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

		public void HearClamor(Thing source, ClamorDef type)
		{
			if (!this.Dead && !this.Downed)
			{
				if (type == ClamorDefOf.Movement)
				{
					Pawn pawn = source as Pawn;
					if (pawn != null)
					{
						this.CheckForDisturbedSleep(pawn);
					}
				}
				if (type == ClamorDefOf.Harm)
				{
					if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
					{
						this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
						if (this.CurJob != null)
						{
							this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
				if (type == ClamorDefOf.Construction)
				{
					if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
					{
						this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
						if (this.CurJob != null)
						{
							this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
				if (type == ClamorDefOf.Impact)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null && !this.Awake())
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
				}
			}
		}

		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood != null)
			{
				if (!this.Awake())
				{
					if (base.Faction == Faction.OfPlayer)
					{
						if (Find.TickManager.TicksGame >= this.lastSleepDisturbedTick + 300)
						{
							if (source != null)
							{
								if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
								{
									return;
								}
								if (source.RaceProps.petness > 0f)
								{
									return;
								}
								if (source.relations != null)
								{
									if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
									{
										return;
									}
								}
							}
							this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
							this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null);
						}
					}
				}
			}
		}

		public bool CheckAcceptArrest(Pawn arrester)
		{
			bool result;
			if (this.health.Downed)
			{
				result = true;
			}
			else if (this.story != null && this.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = true;
			}
			else
			{
				if (base.Faction != null && base.Faction != arrester.factionInt)
				{
					base.Faction.Notify_MemberCaptured(this, arrester.Faction);
				}
				float num = (!this.IsWildMan()) ? 0.5f : 0.2f;
				if (Rand.Value < num)
				{
					result = true;
				}
				else
				{
					Messages.Message("MessageRefusedArrest".Translate(new object[]
					{
						this.LabelShort
					}), this, MessageTypeDefOf.ThreatSmall, true);
					if (base.Faction == null || !arrester.HostileTo(this))
					{
						this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, null, false);
					}
					result = false;
				}
			}
			return result;
		}

		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			bool result;
			if (!base.Spawned)
			{
				result = true;
			}
			else if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				result = true;
			}
			else if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				result = true;
			}
			else if (!this.mindState.Active)
			{
				result = true;
			}
			else
			{
				if (this.Downed)
				{
					if (disabledFor == null)
					{
						return true;
					}
					Pawn pawn = disabledFor.Thing as Pawn;
					if (pawn == null || pawn.mindState == null || pawn.mindState.duty == null || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			bool result;
			if (this.InAggroMentalState || (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState))
			{
				reason = "Enemies".Translate();
				result = true;
			}
			else
			{
				reason = null;
				result = false;
			}
			return result;
		}

		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef != newKindDef)
			{
				if (this.kindDef == PawnKindDefOf.WildMan && base.Spawned)
				{
					base.Map.reachability.ClearCache();
				}
				this.kindDef = newKindDef;
				if (this.kindDef == PawnKindDefOf.WildMan)
				{
					this.mindState.wildManEverReachedOutside = false;
					if (base.Spawned)
					{
						base.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Pawn()
		{
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__0(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__1(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__2(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__3(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__4(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__5(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__6(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static bool <get_InteractionCell>m__7(Thing x)
		{
			return x.def.IsBed;
		}

		[CompilerGenerated]
		private static SoundDef <Kill>m__8(LifeStageAge ls)
		{
			return ls.soundDeath;
		}

		[CompilerGenerated]
		private bool <Destroy>m__9(Pawn p)
		{
			return p.playerSettings != null && p.playerSettings.Master == this;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Thing> <ButcherProducts>__BaseCallProxy0(Pawn butcher, float efficiency)
		{
			return base.ButcherProducts(butcher, efficiency);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy1()
		{
			return base.GetGizmos();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<StatDrawEntry> <SpecialDisplayStats>__BaseCallProxy2()
		{
			return base.SpecialDisplayStats();
		}

		[CompilerGenerated]
		private static bool <CheckForDisturbedSleep>m__A(DirectPawnRelation dr)
		{
			return dr.def == PawnRelationDefOf.Bond;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Pawn $this;

			internal IntVec3 $current;

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
					this.$current = this.InteractionCell;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn.<>c__Iterator0 <>c__Iterator = new Pawn.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ButcherProducts>c__Iterator1 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal float efficiency;

			internal int <meatCount>__1;

			internal Thing <meat>__2;

			internal Pawn butcher;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <t>__3;

			internal int <leatherCount>__4;

			internal Thing <leather>__5;

			internal BodyPartRecord <record>__7;

			internal Thing <thing>__7;

			internal Pawn $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private Pawn.<ButcherProducts>c__Iterator1.<ButcherProducts>c__AnonStorey5 $locvar1;

			[DebuggerHidden]
			public <ButcherProducts>c__Iterator1()
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
					if (base.RaceProps.meatDef != null)
					{
						meatCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true) * efficiency);
						if (meatCount > 0)
						{
							meat = ThingMaker.MakeThing(base.RaceProps.meatDef, null);
							meat.stackCount = meatCount;
							this.$current = meat;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
					}
					break;
				case 1u:
					break;
				case 2u:
					Block_5:
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
					if (base.RaceProps.leatherDef == null)
					{
						goto IL_1F6;
					}
					leatherCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true) * efficiency);
					if (leatherCount > 0)
					{
						leather = ThingMaker.MakeThing(base.RaceProps.leatherDef, null);
						leather.stackCount = leatherCount;
						this.$current = leather;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					goto IL_1F5;
				case 3u:
					goto IL_1F5;
				case 4u:
					goto IL_2BB;
				default:
					return false;
				}
				enumerator = base.<ButcherProducts>__BaseCallProxy0(butcher, efficiency).GetEnumerator();
				num = 4294967293u;
				goto Block_5;
				IL_1F5:
				IL_1F6:
				if (base.RaceProps.Humanlike)
				{
					goto IL_3BC;
				}
				PawnKindLifeStage lifeStage = this.ageTracker.CurKindLifeStage;
				if (lifeStage.butcherBodyPart == null)
				{
					goto IL_3BB;
				}
				if (this.gender != Gender.None && (this.gender != Gender.Male || !lifeStage.butcherBodyPart.allowMale) && (this.gender != Gender.Female || !lifeStage.butcherBodyPart.allowFemale))
				{
					goto IL_3BA;
				}
				IL_2BB:
				record = (from x in this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where x.IsInGroup(lifeStage.butcherBodyPart.bodyPartGroup)
				select x).FirstOrDefault<BodyPartRecord>();
				if (record != null)
				{
					this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, record), null, null, null);
					if (lifeStage.butcherBodyPart.thing != null)
					{
						thing = ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing, null);
					}
					else
					{
						thing = ThingMaker.MakeThing(record.def.spawnThingOnRemoved, null);
					}
					this.$current = thing;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_3BA:
				IL_3BB:
				IL_3BC:
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
				Pawn.<ButcherProducts>c__Iterator1 <ButcherProducts>c__Iterator = new Pawn.<ButcherProducts>c__Iterator1();
				<ButcherProducts>c__Iterator.$this = this;
				<ButcherProducts>c__Iterator.efficiency = efficiency;
				<ButcherProducts>c__Iterator.butcher = butcher;
				return <ButcherProducts>c__Iterator;
			}

			private sealed class <ButcherProducts>c__AnonStorey5
			{
				internal PawnKindLifeStage lifeStage;

				internal Pawn.<ButcherProducts>c__Iterator1 <>f__ref$1;

				public <ButcherProducts>c__AnonStorey5()
				{
				}

				internal bool <>m__0(BodyPartRecord x)
				{
					return x.IsInGroup(this.lifeStage.butcherBodyPart.bodyPartGroup);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator2 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <c>__1;

			internal IEnumerator<Gizmo> $locvar1;

			internal Gizmo <c>__2;

			internal IEnumerator<Gizmo> $locvar2;

			internal Gizmo <attack>__3;

			internal IEnumerator<Gizmo> $locvar3;

			internal Gizmo <g>__4;

			internal IEnumerator<Gizmo> $locvar4;

			internal Gizmo <g>__5;

			internal IEnumerator<Gizmo> $locvar5;

			internal Gizmo <g>__6;

			internal IEnumerator<Gizmo> $locvar6;

			internal Gizmo <g>__7;

			internal Pawn $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator2()
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
					if (!base.IsColonistPlayerControlled)
					{
						goto IL_215;
					}
					enumerator = base.<GetGizmos>__BaseCallProxy1().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_10D;
				case 3u:
					Block_6:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							attack = enumerator3.Current;
							this.$current = attack;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
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
					goto IL_215;
				case 4u:
					Block_8:
					try
					{
						switch (num)
						{
						}
						if (enumerator4.MoveNext())
						{
							g = enumerator4.Current;
							this.$current = g;
							if (!this.$disposing)
							{
								this.$PC = 4;
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
					goto IL_2BC;
				case 5u:
					Block_11:
					try
					{
						switch (num)
						{
						}
						if (enumerator5.MoveNext())
						{
							g2 = enumerator5.Current;
							this.$current = g2;
							if (!this.$disposing)
							{
								this.$PC = 5;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator5 != null)
							{
								enumerator5.Dispose();
							}
						}
					}
					goto IL_374;
				case 6u:
					Block_13:
					try
					{
						switch (num)
						{
						}
						if (enumerator6.MoveNext())
						{
							g3 = enumerator6.Current;
							this.$current = g3;
							if (!this.$disposing)
							{
								this.$PC = 6;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator6 != null)
							{
								enumerator6.Dispose();
							}
						}
					}
					goto IL_41B;
				case 7u:
					Block_14:
					try
					{
						switch (num)
						{
						}
						if (enumerator7.MoveNext())
						{
							g4 = enumerator7.Current;
							this.$current = g4;
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
							if (enumerator7 != null)
							{
								enumerator7.Dispose();
							}
						}
					}
					this.$PC = -1;
					return false;
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
						c = enumerator.Current;
						this.$current = c;
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
				if (this.drafter == null)
				{
					goto IL_184;
				}
				enumerator2 = this.drafter.GetGizmos().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_10D:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						c2 = enumerator2.Current;
						this.$current = c2;
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
				IL_184:
				enumerator3 = PawnAttackGizmoUtility.GetAttackGizmos(this).GetEnumerator();
				num = 4294967293u;
				goto Block_6;
				IL_215:
				if (this.equipment != null)
				{
					enumerator4 = this.equipment.GetGizmos().GetEnumerator();
					num = 4294967293u;
					goto Block_8;
				}
				IL_2BC:
				if (!base.IsColonistPlayerControlled)
				{
					goto IL_41C;
				}
				if (this.apparel != null)
				{
					enumerator5 = this.apparel.GetGizmos().GetEnumerator();
					num = 4294967293u;
					goto Block_11;
				}
				IL_374:
				if (this.playerSettings != null)
				{
					enumerator6 = this.playerSettings.GetGizmos().GetEnumerator();
					num = 4294967293u;
					goto Block_13;
				}
				IL_41B:
				IL_41C:
				enumerator7 = this.mindState.GetGizmos().GetEnumerator();
				num = 4294967293u;
				goto Block_14;
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
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
					break;
				case 4u:
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
					break;
				case 5u:
					try
					{
					}
					finally
					{
						if (enumerator5 != null)
						{
							enumerator5.Dispose();
						}
					}
					break;
				case 6u:
					try
					{
					}
					finally
					{
						if (enumerator6 != null)
						{
							enumerator6.Dispose();
						}
					}
					break;
				case 7u:
					try
					{
					}
					finally
					{
						if (enumerator7 != null)
						{
							enumerator7.Dispose();
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
				Pawn.<GetGizmos>c__Iterator2 <GetGizmos>c__Iterator = new Pawn.<GetGizmos>c__Iterator2();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetExtraFloatMenuOptionsFor>c__Iterator3 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetExtraFloatMenuOptionsFor>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
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
				return new Pawn.<GetExtraFloatMenuOptionsFor>c__Iterator3();
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator4 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <s>__1;

			internal Pawn $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator4()
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
					enumerator = base.<SpecialDisplayStats>__BaseCallProxy2().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					this.$PC = -1;
					return false;
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
						s = enumerator.Current;
						this.$current = s;
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
				this.$current = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), base.BodySize.ToString("F2"), 0, "");
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn.<SpecialDisplayStats>c__Iterator4 <SpecialDisplayStats>c__Iterator = new Pawn.<SpecialDisplayStats>c__Iterator4();
				<SpecialDisplayStats>c__Iterator.$this = this;
				return <SpecialDisplayStats>c__Iterator;
			}
		}
	}
}
