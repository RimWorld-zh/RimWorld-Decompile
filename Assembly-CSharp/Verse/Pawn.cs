#define ENABLE_PROFILER
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		private const float KilledTaleLongRangeThreshold = 35f;

		private const float KilledTaleMeleeRangeThreshold = 2f;

		private const float MajorEnemyThreshold = 250f;

		private static string NotSurgeryReadyTrans;

		private static string CannotReachTrans;

		public const int MaxMoveTicks = 450;

		private int lastSleepDisturbedTick = 0;

		private const int SleepDisturbanceMinInterval = 300;

		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
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

		public string NameStringShort
		{
			get
			{
				return (this.Name == null) ? this.KindLabel : this.Name.ToStringShort;
			}
		}

		public RaceProperties RaceProps
		{
			get
			{
				return base.def.race;
			}
		}

		public Job CurJob
		{
			get
			{
				return (this.jobs == null) ? null : this.jobs.curJob;
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
				return (!this.Dead) ? this.mindState.mentalStateHandler.CurState : null;
			}
		}

		public MentalStateDef MentalStateDef
		{
			get
			{
				return (!this.Dead) ? this.mindState.mentalStateHandler.CurStateDef : null;
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
				return (!this.Dead) ? this.mindState.inspirationHandler.CurState : null;
			}
		}

		public InspirationDef InspirationDef
		{
			get
			{
				return (!this.Dead) ? this.mindState.inspirationHandler.CurStateDef : null;
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
				return base.def.Verbs;
			}
		}

		public List<Tool> Tools
		{
			get
			{
				return null;
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
				return (this.guest != null) ? this.guest.HostFaction : null;
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
				/*Error: Unable to find new state assignment for yield return*/;
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
					result = ((pawn_CarryTracker == null) ? null : pawn_CarryTracker.pawn);
				}
				return result;
			}
		}

		public override string LabelNoCount
		{
			get
			{
				return (this.Name == null) ? this.KindLabel : ((this.story != null && (this.story.adulthood != null || this.story.childhood != null)) ? (this.Name.ToStringShort + ", " + this.story.TitleShort) : this.Name.ToStringShort);
			}
		}

		public override string LabelShort
		{
			get
			{
				return (this.Name == null) ? this.LabelNoCount : this.Name.ToStringShort;
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
				IntVec3 result;
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
					if (position.Standable(base.Map) && position.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position.GetDoor(base.Map) == null)
					{
						result = position;
						goto IL_03e0;
					}
					if (position2.Standable(base.Map) && position2.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position2.GetDoor(base.Map) == null)
					{
						result = position2;
						goto IL_03e0;
					}
					if (position3.Standable(base.Map) && position3.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position3.GetDoor(base.Map) == null)
					{
						result = position3;
						goto IL_03e0;
					}
					if (position4.Standable(base.Map) && position4.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position4.GetDoor(base.Map) == null)
					{
						result = position4;
						goto IL_03e0;
					}
					if (position.Standable(base.Map) && position.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						result = position;
						goto IL_03e0;
					}
					if (position2.Standable(base.Map) && position2.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						result = position2;
						goto IL_03e0;
					}
					if (position3.Standable(base.Map) && position3.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						result = position3;
						goto IL_03e0;
					}
					if (position4.Standable(base.Map) && position4.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						result = position4;
						goto IL_03e0;
					}
					if (position.Standable(base.Map))
					{
						result = position;
						goto IL_03e0;
					}
					if (position2.Standable(base.Map))
					{
						result = position2;
						goto IL_03e0;
					}
					if (position3.Standable(base.Map))
					{
						result = position3;
						goto IL_03e0;
					}
					if (position4.Standable(base.Map))
					{
						result = position4;
						goto IL_03e0;
					}
				}
				result = base.InteractionCell;
				goto IL_03e0;
				IL_03e0:
				return result;
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
					result = ((!(curStance is Stance_Warmup) && !(curStance is Stance_Cooldown)) ? LocalTargetInfo.Invalid : ((Stance_Busy)curStance).focusTarg);
				}
				return result;
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
				return (building_Turret == null) ? this.TryGetAttackVerb(!this.IsColonist) : building_Turret.AttackVerb;
			}
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

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				using (IEnumerator<StatDrawEntry> enumerator = this._003Cget_SpecialDisplayStats_003E__BaseCallProxy2().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						StatDrawEntry s = enumerator.Current;
						yield return s;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
				IL_010d:
				/*Error near IL_010e: Unexpected return in MoveNext()*/;
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

		public static void Reset()
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
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[1]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[0]);
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[1]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[1]
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
			return (this.story == null) ? ((base.thingIDNumber <= 0) ? ((this.kindDef == null) ? ((base.def == null) ? base.GetType().ToString() : base.ThingID) : (this.KindLabel + "_" + base.ThingID)) : base.ThingID) : this.NameStringShort;
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this + ". Replacing with corpse.");
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map);
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
					Log.Error("Pawn " + this.ToString() + " spawned in invalid state. Destroying...");
					this.Destroy(DestroyMode.Vanish);
				}
				else
				{
					this.Drawer.Notify_Spawned();
					this.rotationTracker.Notify_Spawned();
					this.pather.ResetToCurrentPosition();
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
			if (!ThingOwnerUtility.ContentsFrozen(base.ParentHolder))
			{
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTickRare();
				}
				this.inventory.InventoryTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh)
			{
				GenTemperature.PushHeat(this, (float)(0.30000001192092896 * this.BodySize * 4.1666665077209473));
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
				if (!ThingOwnerUtility.ContentsFrozen(base.ParentHolder))
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
				if (!ThingOwnerUtility.ContentsFrozen(base.ParentHolder))
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
						this.relations.SocialTrackerTick();
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
			if (!ThingOwnerUtility.ContentsFrozen(base.ParentHolder))
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
			if (base.Faction == null && this.RaceProps.Humanlike)
			{
				goto IL_0037;
			}
			if (base.Faction != null && base.Faction.IsPlayer)
				goto IL_0037;
			goto IL_00a7;
			IL_0037:
			if (!this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && (int)base.Faction.def.techLevel >= 3;
				Faction newFaction = default(Faction);
				if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out newFaction, tryMedievalOrBetter, false, TechLevel.Undefined))
				{
					this.SetFaction(newFaction, null);
				}
				else
				{
					this.SetFaction(Faction.OfSpacer, null);
				}
			}
			goto IL_00a7;
			IL_00a7:
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind(false);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PassedToWorld();
			}
		}

		public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(dinfo, out absorbed);
			if (!absorbed)
			{
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
			this.records.AccumulateStoryEvent(StoryEventDefOf.DamageTaken);
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
		}

		public override Thing SplitOff(int count)
		{
			if (count > 0 && count < base.stackCount)
			{
				throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
			}
			return base.SplitOff(count);
		}

		private int TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true);
			if (RestraintsUtility.InRestraints(this))
			{
				num = (float)(num * 0.34999999403953552);
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num = (float)(num * 0.60000002384185791);
			}
			float num2 = (float)(num / 60.0);
			float num3;
			if (num2 == 0.0)
			{
				num3 = 450f;
			}
			else
			{
				num3 = (float)(1.0 / num2);
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 = (float)(num3 * 1.4142099618911743);
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
				thingOwner = base.holdingOwner;
				thingOwner.Remove(this);
			}
			bool flag3 = false;
			bool flag4 = false;
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
				flag4 = (map.designationManager.DesignationOn(this, DesignationDefOf.Slaughter) != null);
			}
			bool flag5 = PawnUtility.ShouldSendNotificationAbout(this) && (!flag4 || !dinfo.HasValue || dinfo.Value.Def != DamageDefOf.ExecutionCut);
			float num = 0f;
			Thing attachment = this.GetAttachment(ThingDefOf.Fire);
			if (attachment != null)
			{
				num = ((Fire)attachment).CurrentSize();
			}
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (Current.ProgramState == ProgramState.Playing && this.IsColonist)
			{
				Find.StoryWatcher.watcherRampUp.Notify_ColonistViolentlyDownedOrKilled(this);
			}
			if (this.IsColonist)
			{
				Find.StoryWatcher.statsRecord.colonistsKilled++;
			}
			if (flag && dinfo.HasValue && dinfo.Value.Def.externalViolence)
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (Func<LifeStageAge, SoundDef>)((LifeStageAge ls) => ls.soundDeath), 1f);
			}
			if (dinfo.HasValue && dinfo.Value.Instigator != null)
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
			if (Current.ProgramState == ProgramState.Playing && dinfo.HasValue)
			{
				Pawn pawn2 = dinfo.Value.Instigator as Pawn;
				if (pawn2 == null || pawn2.CurJob == null || !(pawn2.jobs.curDriver is JobDriver_Execute))
				{
					bool flag6 = !this.RaceProps.Humanlike && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Spawned && dinfo.Value.Instigator is Pawn && ((Pawn)dinfo.Value.Instigator).jobs.curDriver is JobDriver_Slaughter;
					if (pawn2 != null)
					{
						if (base.Faction != Faction.OfPlayer && this.kindDef.combatPower >= 250.0 && pawn2.Faction == Faction.OfPlayer)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMajorColonyEnemy, pawn2, this);
						}
						else if (this.IsColonist)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledColonist, pawn2, this);
						}
						else if (base.Faction == Faction.OfPlayer && this.RaceProps.Animal && !flag6)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledColonyAnimal, pawn2, this);
						}
					}
					if ((base.Faction == Faction.OfPlayer || (pawn2 != null && pawn2.Faction == Faction.OfPlayer)) && !flag6)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledBy, this, dinfo.Value);
					}
					if (pawn2 != null)
					{
						if (dinfo.Value.Weapon != null && dinfo.Value.Weapon.building != null && dinfo.Value.Weapon.building.IsMortar)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMortar, pawn2, this, dinfo.Value.Weapon);
						}
						else if (pawn2 != null && pawn2.Position.DistanceTo(base.Position) >= 35.0)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledLongRange, pawn2, this, dinfo.Value.Weapon);
						}
						else if (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMelee, pawn2, this, dinfo.Value.Weapon);
						}
						if (this.kindDef.combatPower >= 250.0)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMajorThreat, pawn2, this, dinfo.Value.Weapon);
						}
						PawnCapacityDef pawnCapacityDef = this.health.ShouldBeDeadFromRequiredCapacity();
						if (pawnCapacityDef != null)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledCapacity, pawn2, this, pawnCapacityDef);
						}
					}
				}
			}
			if (flag)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, RulePackDefOf.Transition_Died));
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
			Thing thing = default(Thing);
			if (pawn_CarryTracker != null && base.holdingOwner.TryDrop((Thing)this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, (Action<Thing, int>)null))
			{
				map = pawn_CarryTracker.pawn.Map;
				flag = true;
			}
			this.health.SetDead();
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
				this.DeSpawn();
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
					if (base.holdingOwner != null)
					{
						base.holdingOwner.Remove(this);
					}
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null))
					{
						corpse.Rotation = base.Rotation;
						if (HuntJobUtility.WasKilledByHunter(this, dinfo))
						{
							((Pawn)dinfo.Value.Instigator).Reserve((Thing)corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null);
						}
						else if (!flag3 && !flag4)
						{
							corpse.SetForbiddenIfOutsideHomeArea();
						}
						if (num > 0.0)
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
				else if (base.holdingOwner != null || this.IsWorldPawn())
				{
					Corpse.PostCorpseDestroy(this);
				}
				else
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
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
			if (mode != 0 && mode != DestroyMode.KillFinalize)
			{
				Log.Error("Destroyed pawn " + this + " with unsupported mode " + mode + ".");
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
					PawnLostCondition cond = (PawnLostCondition)((mode != DestroyMode.KillFinalize) ? 1 : 2);
					lord.Notify_PawnLost(this, cond);
				}
				Find.GameEnder.CheckGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn item in from p in PawnsFinder.AllMapsAndWorld_Alive
			where p.playerSettings != null && p.playerSettings.master == this
			select p)
			{
				item.playerSettings.master = null;
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

		public override void DeSpawn()
		{
			Map map = base.Map;
			if (this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
			}
			base.DeSpawn();
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.needs != null && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
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
				Log.Warning("Tried to discard a world pawn " + this + ".");
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
				foreach (Pawn item in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (item.needs.mood != null)
					{
						item.needs.mood.thoughts.memories.Notify_PawnDiscarded(this);
					}
				}
				Corpse.PostCorpseDestroy(this);
			}
		}

		private Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			Corpse result;
			if (base.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder);
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
					corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation((float)(bedRotation + 180.0));
				}
				result = corpse;
			}
			return result;
		}

		public void ExitMap(bool allowedToJoinOrCreateCaravan)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this);
			}
			else if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this);
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
							pawn.ExitMap(false);
						}
					}
					else
					{
						this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
					}
					this.carryTracker.innerContainer.Clear();
				}
				bool flag = !this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this);
				if (flag && this.HostFaction != null && this.guest != null && (this.guest.Released || !this.IsPrisoner) && !this.InMentalState && this.health.hediffSet.BleedRateTotal < 0.0010000000474974513 && base.Faction.def.appreciative && !base.Faction.def.hidden)
				{
					float num = 15f;
					if (PawnUtility.IsFactionLeader(this))
					{
						num = (float)(num + 50.0);
					}
					Messages.Message("MessagePawnExitMapRelationsGain".Translate(this.LabelShort, base.Faction.Name, num.ToString("F0")), MessageTypeDefOf.PositiveEvent);
					base.Faction.AffectGoodwillWith(this.HostFaction, num);
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
					this.DeSpawn();
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
			switch (action)
			{
			case TradeAction.PlayerBuys:
			{
				this.SetFaction(Faction.OfPlayer, null);
				break;
			}
			case TradeAction.PlayerSells:
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, playerNegotiator, this, trader);
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
					foreach (Pawn item in from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods
					where x.IsColonist || x.IsPrisonerOfColony
					select x)
					{
						item.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, null);
					}
				}
				break;
			}
			}
			this.ClearMind(false);
		}

		public void PreKidnapped(Pawn kidnapper)
		{
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, kidnapper, this);
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
				Log.Warning("Used SetFaction to change " + this + " to same faction " + newFaction);
			}
			else
			{
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
				Find.GameEnder.CheckGameOver();
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
			if (this.jobs != null && this.CurJob == null && this.jobs.jobQueue.Count <= 0 && !this.jobs.startingNewJob)
			{
				bool flag = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
					if (obj.IsValid)
					{
						Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe(), obj.ToStringSafe()), 97771429 ^ base.thingIDNumber);
						flag = true;
					}
					LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
					if (obj2.IsValid)
					{
						Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe(), obj2.ToStringSafe()), 19586765 ^ base.thingIDNumber);
						flag = true;
					}
					IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
					if (attackTarget != null)
					{
						Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe(), attackTarget.ToStringSafe()), 100495878 ^ base.thingIDNumber);
						flag = true;
					}
					IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
					if (obj3.IsValid)
					{
						Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
						Log.ErrorOnce(string.Format("Pawn destination reservation manager failed to clean up properly; {0}/{1}/{2} still reserving {3}", this.ToStringSafe(), job.ToStringSafe(), job.def.ToStringSafe(), obj3.ToStringSafe()), 1958674 ^ base.thingIDNumber);
						flag = true;
					}
				}
				if (flag)
				{
					this.ClearAllReservations(true);
				}
			}
		}

		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false)
		{
			if (this.InContainerEnclosed)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, base.holdingOwner, true);
				}
				if (this.equipment != null && this.equipment.Primary != null)
				{
					this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, base.holdingOwner);
				}
				if (this.inventory != null)
				{
					this.inventory.innerContainer.TryTransferAllToContainer(base.holdingOwner, true);
				}
			}
			else if (base.SpawnedOrAnyParentSpawned)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Thing thing = default(Thing);
					this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, (Action<Thing, int>)null);
				}
				if (keepInventoryAndEquipmentIfInBed && this.InBed())
					return;
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

		public void GenerateNecessaryName()
		{
			if (base.Faction == Faction.OfPlayer && this.RaceProps.Animal && (this.Name == null || this.Name.Numerical))
			{
				if (Rand.Value < this.RaceProps.nameOnTameChance)
				{
					this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Full, (string)null);
				}
				else
				{
					this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, (string)null);
				}
			}
		}

		public Verb TryGetAttackVerb(bool allowManualCastWeapons = false)
		{
			return (this.equipment == null || this.equipment.Primary == null || (this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast && (this.CurJob == null || this.CurJob.def == JobDefOf.WaitCombat) && !allowManualCastWeapons)) ? this.meleeVerbs.TryGetMeleeVerb() : this.equipment.PrimaryEq.PrimaryVerb;
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
				Verb verb = this.TryGetAttackVerb(allowManualCastWeapons);
				result = (verb != null && verb.TryStartCastOn(targ, false, true));
			}
			return result;
		}

		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			using (IEnumerator<Thing> enumerator = this._003CButcherProducts_003E__BaseCallProxy0(butcher, efficiency).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing t = enumerator.Current;
					yield return t;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.RaceProps.meatDef != null)
			{
				int meatCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true) * efficiency);
				if (meatCount > 0)
				{
					Thing meat = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					meat.stackCount = meatCount;
					yield return meat;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.RaceProps.leatherDef != null)
			{
				int leatherCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true) * efficiency);
				if (leatherCount > 0)
				{
					Thing leather = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					leather.stackCount = leatherCount;
					yield return leather;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.RaceProps.Humanlike)
			{
				_003CButcherProducts_003Ec__Iterator1 _003CButcherProducts_003Ec__Iterator = (_003CButcherProducts_003Ec__Iterator1)/*Error near IL_021c: stateMachine*/;
				PawnKindLifeStage lifeStage = this.ageTracker.CurKindLifeStage;
				if (lifeStage.butcherBodyPart != null)
				{
					if (this.gender != 0 && (this.gender != Gender.Male || !lifeStage.butcherBodyPart.allowMale))
					{
						if (this.gender != Gender.Female)
							yield break;
						if (!lifeStage.butcherBodyPart.allowFemale)
							yield break;
					}
					BodyPartRecord record = (from x in this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
					where x.IsInGroup(lifeStage.butcherBodyPart.bodyPartGroup)
					select x).FirstOrDefault();
					if (record != null)
					{
						this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, record), null, default(DamageInfo?));
						Thing thing = (lifeStage.butcherBodyPart.thing == null) ? ThingMaker.MakeThing(record.def.spawnThingOnRemoved, null) : ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing, null);
						yield return thing;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_03c3:
			/*Error near IL_03c4: Unexpected return in MoveNext()*/;
		}

		public string MainDesc(bool writeAge)
		{
			string text = GenLabel.BestKindLabel(this, true, true, false, -1);
			if (base.Faction != null && !base.Faction.def.hidden)
			{
				text = "PawnMainDescFactionedWrap".Translate(text, base.Faction.Name);
			}
			if (writeAge && this.ageTracker != null)
			{
				text = text + ", " + "AgeIndicator".Translate(this.ageTracker.AgeNumberString);
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
			string text = (string)null;
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
					text = (text.NullOrEmpty() ? text2 : (text + ": " + text2));
				}
				catch (Exception arg)
				{
					stringBuilder.AppendLine("JobDriver.GetReport() exception: " + arg);
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
						text3 = text4 + " (+" + (this.jobs.jobQueue.Count - 1) + ")";
					}
					stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
				}
				catch (Exception arg2)
				{
					stringBuilder.AppendLine("JobDriver.GetReport() exception: " + arg2);
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
				using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy1().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Gizmo c2 = enumerator.Current;
						yield return c2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				if (this.drafter != null)
				{
					using (IEnumerator<Gizmo> enumerator2 = this.drafter.GetGizmos().GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							Gizmo c = enumerator2.Current;
							yield return c;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				using (IEnumerator<Gizmo> enumerator3 = PawnAttackGizmoUtility.GetAttackGizmos(this).GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						Gizmo attack = enumerator3.Current;
						yield return attack;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				if (this.equipment != null)
				{
					using (IEnumerator<Gizmo> enumerator4 = this.equipment.GetGizmos().GetEnumerator())
					{
						if (enumerator4.MoveNext())
						{
							Gizmo g4 = enumerator4.Current;
							yield return g4;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				if (this.apparel != null)
				{
					using (IEnumerator<Gizmo> enumerator5 = this.apparel.GetGizmos().GetEnumerator())
					{
						if (enumerator5.MoveNext())
						{
							Gizmo g3 = enumerator5.Current;
							yield return g3;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				if (this.playerSettings != null)
				{
					using (IEnumerator<Gizmo> enumerator6 = this.playerSettings.GetGizmos().GetEnumerator())
					{
						if (enumerator6.MoveNext())
						{
							Gizmo g2 = enumerator6.Current;
							yield return g2;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				using (IEnumerator<Gizmo> enumerator7 = this.mindState.GetGizmos().GetEnumerator())
				{
					if (enumerator7.MoveNext())
					{
						Gizmo g = enumerator7.Current;
						yield return g;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_04ab:
			/*Error near IL_04ac: Unexpected return in MoveNext()*/;
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
			if (this.gender != 0)
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
			return new TipSignal(stringBuilder.ToString().TrimEndNewlines(), base.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		public bool CurrentlyUsableForBills()
		{
			bool result;
			if (!this.InBed() && (this.RaceProps.FleshType.requiresBedForSurgery || !this.Downed))
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans);
				result = false;
			}
			else if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool AnythingToStrip()
		{
			if (this.equipment != null && this.equipment.HasAnything())
			{
				goto IL_005b;
			}
			if (this.apparel != null && this.apparel.WornApparelCount > 0)
			{
				goto IL_005b;
			}
			int result = (this.inventory != null && this.inventory.innerContainer.Count > 0) ? 1 : 0;
			goto IL_005c;
			IL_005b:
			result = 1;
			goto IL_005c;
			IL_005c:
			return (byte)result != 0;
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

		public void HearClamor(Pawn source, ClamorType type)
		{
			if (!this.Dead)
			{
				if (type == ClamorType.Movement && this.needs.mood != null && !this.Awake() && base.Faction == Faction.OfPlayer && Find.TickManager.TicksGame > this.lastSleepDisturbedTick + 300 && !LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
				{
					this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
					this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null);
				}
				if (type == ClamorType.Harm && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
				}
				if (type == ClamorType.Construction && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
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
				if (Rand.Value < 0.5)
				{
					result = true;
				}
				else
				{
					Messages.Message("MessageRefusedArrest".Translate(this.LabelShort), (Thing)this, MessageTypeDefOf.ThreatSmall);
					if (base.Faction == null || !arrester.HostileTo(this))
					{
						this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, (string)null, false, false, null);
					}
					result = false;
				}
			}
			return result;
		}

		public bool ThreatDisabled()
		{
			return (byte)((!base.Spawned) ? 1 : ((!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee)) ? 1 : (this.Downed ? 1 : 0))) != 0;
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
				reason = (string)null;
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
	}
}
