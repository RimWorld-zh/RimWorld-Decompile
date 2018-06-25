using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000FD6 RID: 4054
	public abstract class Verb : IExposable, ILoadReferenceable
	{
		// Token: 0x04004018 RID: 16408
		public VerbProperties verbProps;

		// Token: 0x04004019 RID: 16409
		public Thing caster = null;

		// Token: 0x0400401A RID: 16410
		public ThingWithComps ownerEquipment = null;

		// Token: 0x0400401B RID: 16411
		public HediffComp_VerbGiver ownerHediffComp = null;

		// Token: 0x0400401C RID: 16412
		public ImplementOwnerTypeDef implementOwnerType = null;

		// Token: 0x0400401D RID: 16413
		public Tool tool = null;

		// Token: 0x0400401E RID: 16414
		public ManeuverDef maneuver = null;

		// Token: 0x0400401F RID: 16415
		public TerrainDef terrainDef = null;

		// Token: 0x04004020 RID: 16416
		public string loadID;

		// Token: 0x04004021 RID: 16417
		public VerbState state = VerbState.Idle;

		// Token: 0x04004022 RID: 16418
		protected LocalTargetInfo currentTarget;

		// Token: 0x04004023 RID: 16419
		protected int burstShotsLeft;

		// Token: 0x04004024 RID: 16420
		protected int ticksToNextBurstShot;

		// Token: 0x04004025 RID: 16421
		protected bool surpriseAttack;

		// Token: 0x04004026 RID: 16422
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x04004027 RID: 16423
		public Action castCompleteCallback;

		// Token: 0x04004028 RID: 16424
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;

		// Token: 0x04004029 RID: 16425
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x0400402A RID: 16426
		private static List<IntVec3> tempDestList = new List<IntVec3>();

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06006201 RID: 25089 RVA: 0x001E0D74 File Offset: 0x001DF174
		public Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06006202 RID: 25090 RVA: 0x001E0D94 File Offset: 0x001DF194
		public bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06006203 RID: 25091 RVA: 0x001E0DB8 File Offset: 0x001DF1B8
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06006204 RID: 25092 RVA: 0x001E0DD0 File Offset: 0x001DF1D0
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

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06006205 RID: 25093 RVA: 0x001E0E0C File Offset: 0x001DF20C
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06006206 RID: 25094 RVA: 0x001E0E2C File Offset: 0x001DF22C
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

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06006207 RID: 25095 RVA: 0x001E0E7C File Offset: 0x001DF27C
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

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06006208 RID: 25096 RVA: 0x001E0ECC File Offset: 0x001DF2CC
		public bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x001E0EEC File Offset: 0x001DF2EC
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

		// Token: 0x0600620A RID: 25098 RVA: 0x001E0FB0 File Offset: 0x001DF3B0
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

		// Token: 0x0600620B RID: 25099 RVA: 0x001E10E4 File Offset: 0x001DF4E4
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x001E10FC File Offset: 0x001DF4FC
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

		// Token: 0x0600620D RID: 25101 RVA: 0x001E1188 File Offset: 0x001DF588
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x001E11B0 File Offset: 0x001DF5B0
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool == null) ? "NT" : tool.Id, (maneuver == null) ? "NM" : maneuver.defName);
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x001E1204 File Offset: 0x001DF604
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x06006210 RID: 25104 RVA: 0x001E1230 File Offset: 0x001DF630
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

		// Token: 0x06006211 RID: 25105 RVA: 0x001E1410 File Offset: 0x001DF810
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

		// Token: 0x06006212 RID: 25106 RVA: 0x001E148C File Offset: 0x001DF88C
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

		// Token: 0x06006213 RID: 25107 RVA: 0x001E14E8 File Offset: 0x001DF8E8
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

		// Token: 0x06006214 RID: 25108 RVA: 0x001E1544 File Offset: 0x001DF944
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

		// Token: 0x06006215 RID: 25109
		protected abstract bool TryCastShot();

		// Token: 0x06006216 RID: 25110 RVA: 0x001E17AF File Offset: 0x001DFBAF
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x06006217 RID: 25111 RVA: 0x001E17B8 File Offset: 0x001DFBB8
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
		}

		// Token: 0x06006218 RID: 25112 RVA: 0x001E17EC File Offset: 0x001DFBEC
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

		// Token: 0x06006219 RID: 25113 RVA: 0x001E1878 File Offset: 0x001DFC78
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x001E1898 File Offset: 0x001DFC98
		public bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && this.CanHitTargetFrom(this.caster.Position, targ);
		}

		// Token: 0x0600621B RID: 25115 RVA: 0x001E18E4 File Offset: 0x001DFCE4
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

		// Token: 0x0600621C RID: 25116 RVA: 0x001E19A4 File Offset: 0x001DFDA4
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

		// Token: 0x0600621D RID: 25117 RVA: 0x001E1BF4 File Offset: 0x001DFFF4
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

		// Token: 0x0600621E RID: 25118 RVA: 0x001E1CF4 File Offset: 0x001E00F4
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

		// Token: 0x0600621F RID: 25119 RVA: 0x001E1DB8 File Offset: 0x001E01B8
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
	}
}
