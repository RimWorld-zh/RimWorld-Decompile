using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000FD1 RID: 4049
	public abstract class Verb : IExposable, ILoadReferenceable
	{
		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060061CA RID: 25034 RVA: 0x001E06B0 File Offset: 0x001DEAB0
		public Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060061CB RID: 25035 RVA: 0x001E06D0 File Offset: 0x001DEAD0
		public bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060061CC RID: 25036 RVA: 0x001E06F4 File Offset: 0x001DEAF4
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060061CD RID: 25037 RVA: 0x001E070C File Offset: 0x001DEB0C
		public virtual Texture2D UIIcon
		{
			get
			{
				Texture2D result;
				if (this.ownerEquipment != null)
				{
					result = this.ownerEquipment.def.uiIcon;
				}
				else
				{
					result = BaseContent.BadTex;
				}
				return result;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060061CE RID: 25038 RVA: 0x001E0748 File Offset: 0x001DEB48
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060061CF RID: 25039 RVA: 0x001E0768 File Offset: 0x001DEB68
		public BodyPartGroupDef LinkedBodyPartsGroup
		{
			get
			{
				BodyPartGroupDef result;
				if (this.tool != null)
				{
					result = this.tool.linkedBodyPartsGroup;
				}
				else if (this.verbProps == null)
				{
					result = null;
				}
				else
				{
					result = this.verbProps.linkedBodyPartsGroup;
				}
				return result;
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x060061D0 RID: 25040 RVA: 0x001E07B8 File Offset: 0x001DEBB8
		public bool EnsureLinkedBodyPartsGroupAlwaysUsable
		{
			get
			{
				bool result;
				if (this.tool != null)
				{
					result = this.tool.ensureLinkedBodyPartsGroupAlwaysUsable;
				}
				else
				{
					result = (this.verbProps != null && this.verbProps.ensureLinkedBodyPartsGroupAlwaysUsable);
				}
				return result;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x060061D1 RID: 25041 RVA: 0x001E0808 File Offset: 0x001DEC08
		public bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060061D2 RID: 25042 RVA: 0x001E0828 File Offset: 0x001DEC28
		public bool CanBeUsedInMelee
		{
			get
			{
				return this.verbProps.CanBeUsedInMelee;
			}
		}

		// Token: 0x060061D3 RID: 25043 RVA: 0x001E0848 File Offset: 0x001DEC48
		public float GetDamageFactorFor(Pawn pawn)
		{
			float num = 1f;
			if (pawn != null)
			{
				if (this.ownerHediffComp != null)
				{
					num *= PawnCapacityUtility.CalculatePartEfficiency(this.ownerHediffComp.Pawn.health.hediffSet, this.ownerHediffComp.parent.Part, true, null);
				}
				else if (this.LinkedBodyPartsGroup != null)
				{
					float num2 = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, this.LinkedBodyPartsGroup);
					if (this.EnsureLinkedBodyPartsGroupAlwaysUsable)
					{
						num2 = Mathf.Max(num2, 0.4f);
					}
					num *= num2;
				}
				if (this.IsMeleeAttack)
				{
					num *= pawn.ageTracker.CurLifeStage.meleeDamageFactor;
				}
			}
			return num;
		}

		// Token: 0x060061D4 RID: 25044 RVA: 0x001E090C File Offset: 0x001DED0C
		public bool IsStillUsableBy(Pawn pawn)
		{
			Profiler.BeginSample("IsStillUsableBy()");
			bool result;
			if (!this.Available())
			{
				Profiler.EndSample();
				result = false;
			}
			else if (this.ownerEquipment != null && !pawn.equipment.AllEquipmentListForReading.Contains(this.ownerEquipment))
			{
				Profiler.EndSample();
				result = false;
			}
			else
			{
				if (this.ownerHediffComp != null)
				{
					bool flag = false;
					List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (hediffs[i] == this.ownerHediffComp.parent)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Profiler.EndSample();
						return false;
					}
				}
				if (this.terrainDef != null && (!pawn.Spawned || this.terrainDef != pawn.Position.GetTerrain(pawn.Map)))
				{
					result = false;
				}
				else if (this.GetDamageFactorFor(pawn) == 0f)
				{
					Profiler.EndSample();
					result = false;
				}
				else
				{
					Profiler.EndSample();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060061D5 RID: 25045 RVA: 0x001E0A40 File Offset: 0x001DEE40
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x001E0A58 File Offset: 0x001DEE58
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
		}

		// Token: 0x060061D7 RID: 25047 RVA: 0x001E0AE4 File Offset: 0x001DEEE4
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x001E0B0C File Offset: 0x001DEF0C
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool == null) ? "NT" : tool.Id, (maneuver == null) ? "NM" : maneuver.defName);
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x001E0B60 File Offset: 0x001DEF60
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x001E0B8C File Offset: 0x001DEF8C
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			bool result;
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).", false);
				result = false;
			}
			else if (!this.caster.Spawned)
			{
				result = false;
			}
			else if (this.state == VerbState.Bursting || !this.CanHitTarget(castTarg))
			{
				result = false;
			}
			else
			{
				if (this.verbProps.CausesTimeSlowdown && castTarg.HasThing && (castTarg.Thing.def.category == ThingCategory.Pawn || (castTarg.Thing.def.building != null && castTarg.Thing.def.building.IsTurret)) && castTarg.Thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer))
				{
					Find.TickManager.slower.SignalForceNormalSpeed();
				}
				this.surpriseAttack = surpriseAttack;
				this.canHitNonTargetPawnsNow = canHitNonTargetPawns;
				this.currentTarget = castTarg;
				if (this.CasterIsPawn && this.verbProps.warmupTime > 0f)
				{
					ShootLine newShootLine;
					if (!this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine))
					{
						return false;
					}
					this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
					float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
					int ticks = (this.verbProps.warmupTime * statValue).SecondsToTicks();
					this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
				}
				else
				{
					this.WarmupComplete();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x001E0D6C File Offset: 0x001DF16C
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
			if (this.CasterIsPawn && this.currentTarget.HasThing)
			{
				Pawn pawn = this.currentTarget.Thing as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled)
				{
					this.CasterPawn.records.AccumulateStoryEvent(StoryEventDefOf.AttackedPlayer);
				}
			}
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x001E0DE8 File Offset: 0x001DF1E8
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				if (!this.caster.Spawned)
				{
					this.Reset();
				}
				else
				{
					this.ticksToNextBurstShot--;
					if (this.ticksToNextBurstShot <= 0)
					{
						this.TryCastNextBurstShot();
					}
				}
			}
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x001E0E44 File Offset: 0x001DF244
		public virtual bool Available()
		{
			if (this.verbProps.consumeFuelPerShot > 0f)
			{
				CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && compRefuelable.Fuel < this.verbProps.consumeFuelPerShot)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x001E0EA0 File Offset: 0x001DF2A0
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					MoteMaker.MakeStaticMote(this.caster.Position, this.caster.Map, ThingDefOf.Mote_ShotFlash, this.verbProps.muzzleFlashScale);
				}
				if (this.verbProps.soundCast != null)
				{
					this.verbProps.soundCast.PlayOneShot(new TargetInfo(this.caster.Position, this.caster.Map, false));
				}
				if (this.verbProps.soundCastTail != null)
				{
					this.verbProps.soundCastTail.PlayOneShotOnCamera(this.caster.Map);
				}
				if (this.CasterIsPawn)
				{
					if (this.CasterPawn.thinker != null)
					{
						this.CasterPawn.mindState.Notify_EngagedTarget();
					}
					if (this.CasterPawn.mindState != null)
					{
						this.CasterPawn.mindState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.CasterPawn.MentalState != null)
					{
						this.CasterPawn.MentalState.Notify_AttackedTarget(localTargetInfo);
					}
					if (!this.CasterPawn.Spawned)
					{
						return;
					}
				}
				if (this.verbProps.consumeFuelPerShot > 0f)
				{
					CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(this.verbProps.consumeFuelPerShot);
					}
				}
				this.burstShotsLeft--;
			}
			else
			{
				this.burstShotsLeft = 0;
			}
			if (this.burstShotsLeft > 0)
			{
				this.ticksToNextBurstShot = this.verbProps.ticksBetweenBurstShots;
				if (this.CasterIsPawn)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.ticksBetweenBurstShots + 1, this.currentTarget, this));
				}
			}
			else
			{
				this.state = VerbState.Idle;
				if (this.CasterIsPawn)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.AdjustedCooldownTicks(this, this.CasterPawn, this.ownerEquipment), this.currentTarget, this));
				}
				if (this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
			}
		}

		// Token: 0x060061DF RID: 25055
		protected abstract bool TryCastShot();

		// Token: 0x060061E0 RID: 25056 RVA: 0x001E110B File Offset: 0x001DF50B
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x001E1114 File Offset: 0x001DF514
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x001E1148 File Offset: 0x001DF548
		public virtual void Notify_EquipmentLost()
		{
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Spawned)
				{
					Stance_Warmup stance_Warmup = casterPawn.stances.curStance as Stance_Warmup;
					if (stance_Warmup != null && stance_Warmup.verb == this)
					{
						casterPawn.stances.CancelBusyStanceSoft();
					}
					if (casterPawn.CurJob != null && casterPawn.CurJob.def == JobDefOf.AttackStatic)
					{
						casterPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
				}
			}
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x001E11D4 File Offset: 0x001DF5D4
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x001E11F4 File Offset: 0x001DF5F4
		public bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && this.CanHitTargetFrom(this.caster.Position, targ);
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x001E1240 File Offset: 0x001DF640
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			bool result;
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				result = this.verbProps.targetParams.canTargetSelf;
			}
			else
			{
				if (this.CasterIsPawn && this.CasterPawn.apparel != null)
				{
					List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
					for (int i = 0; i < wornApparel.Count; i++)
					{
						if (!wornApparel[i].AllowVerbCast(root, this.caster.Map, targ, this))
						{
							return false;
						}
					}
				}
				ShootLine shootLine;
				result = this.TryFindShootLineFromTo(root, targ, out shootLine);
			}
			return result;
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x001E1300 File Offset: 0x001DF700
		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine)
		{
			bool result;
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				result = false;
			}
			else if (this.verbProps.IsMeleeAttack || this.verbProps.range <= 1.42f)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				result = ReachabilityImmediate.CanReachImmediate(root, targ, this.caster.Map, PathEndMode.Touch, null);
			}
			else
			{
				CellRect cellRect = (!targ.HasThing) ? CellRect.SingleCell(targ.Cell) : targ.Thing.OccupiedRect();
				float num = cellRect.ClosestDistSquaredTo(root);
				if (num > this.verbProps.range * this.verbProps.range || num < this.verbProps.minRange * this.verbProps.minRange)
				{
					resultingLine = new ShootLine(root, targ.Cell);
					result = false;
				}
				else if (!this.verbProps.requireLineOfSight)
				{
					resultingLine = new ShootLine(root, targ.Cell);
					result = true;
				}
				else
				{
					if (this.CasterIsPawn)
					{
						IntVec3 dest;
						if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
						{
							resultingLine = new ShootLine(root, dest);
							return true;
						}
						ShootLeanUtility.LeanShootingSourcesFromTo(root, cellRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
						for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
						{
							IntVec3 intVec = Verb.tempLeanShootSources[i];
							if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
							{
								resultingLine = new ShootLine(intVec, dest);
								return true;
							}
						}
					}
					else
					{
						CellRect.CellRectIterator iterator = this.caster.OccupiedRect().GetIterator();
						while (!iterator.Done())
						{
							IntVec3 intVec2 = iterator.Current;
							IntVec3 dest;
							if (this.CanHitFromCellIgnoringRange(intVec2, targ, out dest))
							{
								resultingLine = new ShootLine(intVec2, dest);
								return true;
							}
							iterator.MoveNext();
						}
					}
					resultingLine = new ShootLine(root, targ.Cell);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x001E1550 File Offset: 0x001DF950
		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					return false;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing);
				for (int i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
					{
						goodDest = Verb.tempDestList[i];
						return true;
					}
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				return true;
			}
			goodDest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x001E1650 File Offset: 0x001DFA50
		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			if (this.verbProps.mustCastOnOpenGround)
			{
				if (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn))
				{
					return false;
				}
			}
			if (this.verbProps.requireLineOfSight)
			{
				if (!includeCorners)
				{
					if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
					{
						return false;
					}
				}
				else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x001E1714 File Offset: 0x001DFB14
		public override string ToString()
		{
			string text;
			if (this.verbProps == null)
			{
				text = "null";
			}
			else if (!this.verbProps.label.NullOrEmpty())
			{
				text = this.verbProps.label;
			}
			else if (this.ownerHediffComp != null)
			{
				text = this.ownerHediffComp.Def.label;
			}
			else if (this.ownerEquipment != null)
			{
				text = this.ownerEquipment.def.label;
			}
			else if (this.LinkedBodyPartsGroup != null)
			{
				text = this.LinkedBodyPartsGroup.defName;
			}
			else
			{
				text = "unknown";
			}
			if (this.tool != null)
			{
				text = text + "/" + this.tool.Id;
			}
			return base.GetType().ToString() + "(" + text + ")";
		}

		// Token: 0x04003FEC RID: 16364
		public VerbProperties verbProps;

		// Token: 0x04003FED RID: 16365
		public Thing caster = null;

		// Token: 0x04003FEE RID: 16366
		public ThingWithComps ownerEquipment = null;

		// Token: 0x04003FEF RID: 16367
		public HediffComp_VerbGiver ownerHediffComp = null;

		// Token: 0x04003FF0 RID: 16368
		public ImplementOwnerTypeDef implementOwnerType = null;

		// Token: 0x04003FF1 RID: 16369
		public Tool tool = null;

		// Token: 0x04003FF2 RID: 16370
		public ManeuverDef maneuver = null;

		// Token: 0x04003FF3 RID: 16371
		public TerrainDef terrainDef = null;

		// Token: 0x04003FF4 RID: 16372
		public string loadID;

		// Token: 0x04003FF5 RID: 16373
		public VerbState state = VerbState.Idle;

		// Token: 0x04003FF6 RID: 16374
		protected LocalTargetInfo currentTarget;

		// Token: 0x04003FF7 RID: 16375
		protected int burstShotsLeft;

		// Token: 0x04003FF8 RID: 16376
		protected int ticksToNextBurstShot;

		// Token: 0x04003FF9 RID: 16377
		protected bool surpriseAttack;

		// Token: 0x04003FFA RID: 16378
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x04003FFB RID: 16379
		public Action castCompleteCallback;

		// Token: 0x04003FFC RID: 16380
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;

		// Token: 0x04003FFD RID: 16381
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x04003FFE RID: 16382
		private static List<IntVec3> tempDestList = new List<IntVec3>();
	}
}
