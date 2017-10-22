using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	public class Pawn : ThingWithComps, IBillGiver, ITrader, IAttackTargetSearcher, IAttackTarget, IStrippable, ILoadReferenceable, IThingHolder, IVerbOwner
	{
		private const float HumanSizedHeatOutput = 0.3f;

		public const int MaxMoveTicks = 450;

		private const int SleepDisturbanceMinInterval = 300;

		public PawnKindDef kindDef;

		private Name nameInt;

		public Gender gender;

		public Pawn_AgeTracker ageTracker;

		public Pawn_HealthTracker health;

		public Pawn_RecordsTracker records;

		public Pawn_InventoryTracker inventory;

		public Pawn_MeleeVerbs meleeVerbs;

		public VerbTracker verbTracker;

		public Pawn_CarryTracker carryTracker;

		public Pawn_NeedsTracker needs;

		public Pawn_MindState mindState;

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

		private int lastSleepDisturbedTick;

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
				if (this.Name != null)
				{
					return this.Name.ToStringShort;
				}
				return this.KindLabel;
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
				return GenLabel.BestKindLabel(this, false, false, false);
			}
		}

		public string KindLabelPlural
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, true);
			}
		}

		public bool InMentalState
		{
			get
			{
				if (this.Dead)
				{
					return false;
				}
				return this.mindState.mentalStateHandler.InMentalState;
			}
		}

		public MentalState MentalState
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurState;
			}
		}

		public MentalStateDef MentalStateDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurStateDef;
			}
		}

		public bool InAggroMentalState
		{
			get
			{
				if (this.Dead)
				{
					return false;
				}
				return this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
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
				if (this.guest == null)
				{
					return null;
				}
				return this.guest.HostFaction;
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
				if (base.ParentHolder == null)
				{
					return null;
				}
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					return pawn_CarryTracker.pawn;
				}
				return null;
			}
		}

		public override string LabelNoCount
		{
			get
			{
				if (this.Name != null)
				{
					if (this.story != null && (this.story.adulthood != null || this.story.childhood != null))
					{
						return this.Name.ToStringShort + ", " + this.story.TitleShort;
					}
					return this.Name.ToStringShort;
				}
				return this.KindLabel;
			}
		}

		public override string LabelShort
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort;
				}
				return this.LabelNoCount;
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
					if (position.Standable(base.Map) && position.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position.GetDoor(base.Map) == null)
					{
						return position;
					}
					if (position2.Standable(base.Map) && position2.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position2.GetDoor(base.Map) == null)
					{
						return position2;
					}
					if (position3.Standable(base.Map) && position3.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position3.GetDoor(base.Map) == null)
					{
						return position3;
					}
					if (position4.Standable(base.Map) && position4.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null && position4.GetDoor(base.Map) == null)
					{
						return position4;
					}
					if (position.Standable(base.Map) && position.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						return position;
					}
					if (position2.Standable(base.Map) && position2.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						return position2;
					}
					if (position3.Standable(base.Map) && position3.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						return position3;
					}
					if (position4.Standable(base.Map) && position4.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def.IsBed)) == null)
					{
						return position4;
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

		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				if (!base.Spawned)
				{
					return LocalTargetInfo.Invalid;
				}
				Stance curStance = this.stances.curStance;
				if (!(curStance is Stance_Warmup) && !(curStance is Stance_Cooldown))
				{
					return LocalTargetInfo.Invalid;
				}
				return ((Stance_Busy)curStance).focusTarg;
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
				if (building_Turret != null)
				{
					return building_Turret.AttackVerb;
				}
				return this.TryGetAttackVerb(!this.IsColonist);
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
				foreach (StatDrawEntry specialDisplayStat in base.SpecialDisplayStats)
				{
					yield return specialDisplayStat;
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), 0);
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
		}

		public override string ToString()
		{
			if (this.story != null)
			{
				return this.NameStringShort;
			}
			if (base.thingIDNumber > 0)
			{
				return base.ThingID;
			}
			if (this.kindDef != null)
			{
				return this.KindLabel + "_" + base.ThingID;
			}
			if (base.def != null)
			{
				return base.ThingID;
			}
			return base.GetType().ToString();
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
			if (this.IsColonistPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.mindState.priorityWork.DrawExtraSelectionOverlays();
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
						this.jobs.JobTrackerTick();
					}
					if (base.Spawned)
					{
						this.Drawer.DrawTrackerTick();
					}
					this.health.HealthTick();
					if (!this.Dead)
					{
						this.mindState.MindStateTick();
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
						this.equipment.EquipmentTrackerTick();
					}
					if (this.apparel != null)
					{
						this.apparel.ApparelTrackerTick();
					}
					if (this.interactions != null)
					{
						this.interactions.InteractionsTrackerTick();
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

		public void Notify_Teleported(bool endCurrentJob = true)
		{
			this.Drawer.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob && this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
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
			if (dinfo.Def.externalViolence)
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
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

		public override void Kill(DamageInfo? dinfo)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map map = base.Map;
			Map mapHeld = base.MapHeld;
			bool flag = base.Spawned;
			bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
			bool wasWorldPawn = this.IsWorldPawn();
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
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
			}
			float num = 0f;
			Thing attachment = this.GetAttachment(ThingDefOf.Fire);
			if (attachment != null)
			{
				num = ((Fire)attachment).CurrentSize();
			}
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (Current.ProgramState == ProgramState.Playing && base.Faction != null && base.Faction == Faction.OfPlayer)
			{
				Find.StoryWatcher.watcherRampUp.Notify_PlayerPawnIncappedOrKilled(this);
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
				}
			}
			if (Current.ProgramState == ProgramState.Playing && dinfo.HasValue)
			{
				Pawn pawn2 = dinfo.Value.Instigator as Pawn;
				if (pawn2 == null || pawn2.CurJob == null || !(pawn2.jobs.curDriver is JobDriver_Execute))
				{
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
						else if (base.Faction == Faction.OfPlayer && this.RaceProps.Animal)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledColonyAnimal, pawn2, this);
						}
					}
					if (base.Faction == Faction.OfPlayer && (this.RaceProps.Humanlike || dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction != Faction.OfPlayer))
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledBy, this, dinfo.Value);
					}
				}
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
			Caravan caravan = this.GetCaravan();
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
							((Pawn)dinfo.Value.Instigator).Reserve((Thing)corpse, 1, -1, null);
						}
						else if (!flag3)
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
				base.Faction.Notify_MemberDied(this, dinfo, wasWorldPawn);
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
				this.ClearReservations(true);
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
			this.ClearReservations(true);
			map.mapPawns.DeRegisterPawn(this);
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
		}

		public override void Discard()
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".");
			}
			else
			{
				base.Discard();
				if (this.relations != null)
				{
					this.relations.ClearAllRelations();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.PlayLog.Notify_PawnDiscarded(this);
					Find.TaleManager.Notify_PawnDiscarded(this);
				}
				Corpse.PostCorpseDestroy(this);
			}
		}

		private Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (base.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder);
				return null;
			}
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
			return corpse;
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
				if (flag && this.HostFaction != null && this.guest != null && (this.guest.released || !this.IsPrisoner) && !this.InMentalState && this.health.hediffSet.BleedRateTotal < 0.0010000000474974513 && base.Faction.def.appreciative && !base.Faction.def.hidden)
				{
					float num = 15f;
					if (PawnUtility.IsFactionLeader(this))
					{
						num = (float)(num + 50.0);
					}
					Messages.Message("MessagePawnExitMapRelationsGain".Translate(this.LabelShort, base.Faction.Name, num.ToString("F0")), MessageSound.Benefit);
					base.Faction.AffectGoodwillWith(this.HostFaction, num);
				}
				if (this.ownership != null)
				{
					this.ownership.UnclaimAll();
				}
				if (this.guest != null && flag)
				{
					this.guest.SetGuestStatus(null, false);
				}
				if (base.Spawned)
				{
					this.DeSpawn();
				}
				this.inventory.UnloadEverything = false;
				this.ClearMind(false);
				this.ClearReservations(true);
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
			this.ClearReservations(true);
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
			this.ClearReservations(true);
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
					base.Map.pawnDestinationManager.UnreserveAllFor(this);
					base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				}
				if (newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer)
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
					this.kindDef = newFaction.def.basicMemberKind;
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
		}

		public void ClearReservations(bool unreserveDestinations = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (unreserveDestinations)
				{
					maps[i].pawnDestinationManager.UnreserveAllFor(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
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
			if (this.equipment != null && this.equipment.Primary != null && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.WaitCombat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			return this.meleeVerbs.TryGetMeleeVerb();
		}

		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			foreach (Thing item in base.ButcherProducts(butcher, efficiency))
			{
				yield return item;
			}
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
					if (this.gender != 0 && (this.gender != Gender.Male || !lifeStage.butcherBodyPart.allowMale))
					{
						if (this.gender != Gender.Female)
							yield break;
						if (!lifeStage.butcherBodyPart.allowFemale)
							yield break;
					}
					while (true)
					{
						BodyPartRecord record = (from x in this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined)
						where x.IsInGroup(((_003CButcherProducts_003Ec__Iterator21A)/*Error near IL_027f: stateMachine*/)._003ClifeStage_003E__6.butcherBodyPart.bodyPartGroup)
						select x).FirstOrDefault();
						if (record != null)
						{
							this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, record), null, default(DamageInfo?));
							Thing thing = (lifeStage.butcherBodyPart.thing == null) ? ThingMaker.MakeThing(record.def.spawnThingOnRemoved, null) : ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing, null);
							yield return thing;
							continue;
						}
						break;
					}
				}
			}
		}

		public string MainDesc(bool writeAge)
		{
			string text = GenLabel.BestKindLabel(this, true, true, false);
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
			if (this.MentalState != null)
			{
				stringBuilder.AppendLine(this.MentalState.InspectLine);
			}
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine("Equipped".Translate() + ": " + ((this.equipment.Primary == null) ? "EquippedNothing".Translate() : this.equipment.Primary.Label));
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
				foreach (Gizmo gizmo in base.GetGizmos())
				{
					yield return gizmo;
				}
				if (this.drafter != null)
				{
					foreach (Gizmo gizmo2 in this.drafter.GetGizmos())
					{
						yield return gizmo2;
					}
				}
				if (this.equipment != null)
				{
					foreach (Gizmo gizmo3 in this.equipment.GetGizmos())
					{
						yield return gizmo3;
					}
				}
				if (this.apparel != null)
				{
					foreach (Gizmo gizmo4 in this.apparel.GetGizmos())
					{
						yield return gizmo4;
					}
				}
				if (this.playerSettings != null)
				{
					foreach (Gizmo gizmo5 in this.playerSettings.GetGizmos())
					{
						yield return gizmo5;
					}
				}
				foreach (Gizmo gizmo6 in this.mindState.GetGizmos())
				{
					yield return gizmo6;
				}
			}
		}

		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			yield break;
		}

		public override TipSignal GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.LabelCap);
			string text = string.Empty;
			if (this.gender != 0)
			{
				text = this.gender.GetLabel();
			}
			if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				if (text != string.Empty)
				{
					text += " ";
				}
				text += this.KindLabel;
			}
			if (text != string.Empty)
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

		public bool CurrentlyUsable()
		{
			return (this.InBed() || (!this.RaceProps.IsFlesh && this.Downed)) && this.InteractionCell.IsValid;
		}

		public bool AnythingToStrip()
		{
			if (this.equipment != null && this.equipment.HasAnything())
			{
				goto IL_005a;
			}
			if (this.apparel != null && this.apparel.WornApparelCount > 0)
			{
				goto IL_005a;
			}
			int result = (this.inventory != null && this.inventory.innerContainer.Count > 0) ? 1 : 0;
			goto IL_005b;
			IL_005a:
			result = 1;
			goto IL_005b;
			IL_005b:
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
			if (this.health.Downed)
			{
				return true;
			}
			if (this.story != null && this.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				return true;
			}
			if (base.Faction != null && base.Faction != arrester.factionInt)
			{
				base.Faction.Notify_MemberCaptured(this, arrester.Faction);
			}
			if (Rand.Value < 0.5)
			{
				return true;
			}
			Messages.Message("MessageRefusedArrest".Translate(this.LabelShort), (Thing)this, MessageSound.SeriousAlert);
			if (base.Faction == null || !arrester.HostileTo(this))
			{
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, (string)null, false, false, null);
			}
			return false;
		}

		public bool ThreatDisabled()
		{
			if (!base.Spawned)
			{
				return true;
			}
			if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				return true;
			}
			if (this.Downed)
			{
				return true;
			}
			return false;
		}

		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (!this.InAggroMentalState && (!base.Faction.HostileTo(Faction.OfPlayer) || this.HostFaction != null || this.Downed || this.InMentalState))
			{
				reason = (string)null;
				return false;
			}
			reason = "Enemies".Translate();
			return true;
		}

		public void ChangeKind(PawnKindDef newKindDef)
		{
			this.kindDef = newKindDef;
		}

		virtual Map get_Map()
		{
			return base.Map;
		}

		Map IBillGiver.get_Map()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_Map
			return this.get_Map();
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

		virtual IThingHolder get_ParentHolder()
		{
			return base.ParentHolder;
		}

		IThingHolder IThingHolder.get_ParentHolder()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_ParentHolder
			return this.get_ParentHolder();
		}
	}
}
