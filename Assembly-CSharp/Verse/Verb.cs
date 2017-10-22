#define ENABLE_PROFILER
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	public abstract class Verb : IExposable, ILoadReferenceable
	{
		public VerbProperties verbProps;

		public Thing caster = null;

		public ThingWithComps ownerEquipment = null;

		public HediffComp_VerbGiver ownerHediffComp = null;

		public ImplementOwnerTypeDef implementOwnerType = null;

		public Tool tool = null;

		public ManeuverDef maneuver = null;

		public string loadID;

		public VerbState state = VerbState.Idle;

		protected LocalTargetInfo currentTarget = (Thing)null;

		protected int burstShotsLeft = 0;

		protected int ticksToNextBurstShot = 0;

		protected bool surpriseAttack;

		protected bool canFreeInterceptNow = true;

		public Action castCompleteCallback = null;

		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		private static List<IntVec3> tempDestList = new List<IntVec3>();

		public Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		public bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		public virtual Texture2D UIIcon
		{
			get
			{
				return (this.ownerEquipment == null) ? BaseContent.BadTex : this.ownerEquipment.def.uiIcon;
			}
		}

		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		public BodyPartGroupDef LinkedBodyPartsGroup
		{
			get
			{
				return (this.tool == null) ? ((this.verbProps != null) ? this.verbProps.linkedBodyPartsGroup : null) : this.tool.linkedBodyPartsGroup;
			}
		}

		public float GetDamageFactorFor(Pawn pawn)
		{
			float result;
			if (pawn != null)
			{
				if (this.ownerHediffComp != null)
				{
					result = PawnCapacityUtility.CalculatePartEfficiency(this.ownerHediffComp.Pawn.health.hediffSet, this.ownerHediffComp.parent.Part, true, null);
					goto IL_0078;
				}
				if (this.LinkedBodyPartsGroup != null)
				{
					result = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, this.LinkedBodyPartsGroup);
					goto IL_0078;
				}
			}
			result = 1f;
			goto IL_0078;
			IL_0078:
			return result;
		}

		public bool IsStillUsableBy(Pawn pawn)
		{
			Profiler.BeginSample("IsStillUsableBy()");
			bool result;
			if (this.ownerEquipment != null && !pawn.equipment.AllEquipmentListForReading.Contains(this.ownerEquipment))
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
					int num = 0;
					while (num < hediffs.Count)
					{
						if (hediffs[num] != this.ownerHediffComp.parent)
						{
							num++;
							continue;
						}
						flag = true;
						break;
					}
					if (!flag)
					{
						Profiler.EndSample();
						result = false;
						goto IL_00d3;
					}
				}
				if (this.GetDamageFactorFor(pawn) == 0.0)
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
			goto IL_00d3;
			IL_00d3:
			return result;
		}

		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", (string)null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
		}

		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool == null) ? "NT" : tool.Id, (maneuver == null) ? "NM" : maneuver.defName);
		}

		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canFreeIntercept = true)
		{
			bool result;
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).");
				result = false;
			}
			else if (!this.caster.Spawned)
			{
				result = false;
			}
			else
			{
				if (this.state != VerbState.Bursting && this.CanHitTarget(castTarg))
				{
					if (this.verbProps.CausesTimeSlowdown && castTarg.HasThing && (castTarg.Thing.def.category == ThingCategory.Pawn || (castTarg.Thing.def.building != null && castTarg.Thing.def.building.IsTurret)) && castTarg.Thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer))
					{
						Find.TickManager.slower.SignalForceNormalSpeed();
					}
					this.surpriseAttack = surpriseAttack;
					this.canFreeInterceptNow = canFreeIntercept;
					this.currentTarget = castTarg;
					if (this.CasterIsPawn && this.verbProps.warmupTime > 0.0)
					{
						ShootLine newShootLine = default(ShootLine);
						if (this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine))
						{
							this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
							float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
							int ticks = (this.verbProps.warmupTime * statValue).SecondsToTicks();
							this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
							goto IL_01c8;
						}
						result = false;
						goto IL_01cf;
					}
					this.WarmupComplete();
					goto IL_01c8;
				}
				result = false;
			}
			goto IL_01cf;
			IL_01c8:
			result = true;
			goto IL_01cf;
			IL_01cf:
			return result;
		}

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

		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.0099999997764825821)
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
						return;
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
				if ((object)this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
			}
		}

		protected abstract bool TryCastShot();

		public void Notify_PickedUp()
		{
			this.Reset();
		}

		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = (Thing)null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
		}

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

		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		public bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && this.CanHitTargetFrom(this.caster.Position, targ);
		}

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
						if (!wornApparel[i].AllowVerbCast(root, this.caster.Map, targ))
							goto IL_0088;
					}
				}
				ShootLine shootLine = default(ShootLine);
				result = this.TryFindShootLineFromTo(root, targ, out shootLine);
			}
			goto IL_00b1;
			IL_00b1:
			return result;
			IL_0088:
			result = false;
			goto IL_00b1;
		}

		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine)
		{
			bool result;
			IntVec3 dest = default(IntVec3);
			IntVec3 intVec;
			IntVec3 current;
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				result = false;
			}
			else if (this.verbProps.MeleeRange)
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
						if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
						{
							resultingLine = new ShootLine(root, dest);
							result = true;
							goto IL_022c;
						}
						ShootLeanUtility.LeanShootingSourcesFromTo(root, cellRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
						for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
						{
							intVec = Verb.tempLeanShootSources[i];
							if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
								goto IL_018c;
						}
					}
					else
					{
						CellRect.CellRectIterator iterator = this.caster.OccupiedRect().GetIterator();
						while (!iterator.Done())
						{
							current = iterator.Current;
							if (this.CanHitFromCellIgnoringRange(current, targ, out dest))
								goto IL_01f1;
							iterator.MoveNext();
						}
					}
					resultingLine = new ShootLine(root, targ.Cell);
					result = false;
				}
			}
			goto IL_022c;
			IL_022c:
			return result;
			IL_018c:
			resultingLine = new ShootLine(intVec, dest);
			result = true;
			goto IL_022c;
			IL_01f1:
			resultingLine = new ShootLine(current, dest);
			result = true;
			goto IL_022c;
		}

		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			bool result;
			int i;
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					result = false;
					goto IL_00f2;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing);
				for (i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
						goto IL_0081;
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				result = true;
				goto IL_00f2;
			}
			goodDest = IntVec3.Invalid;
			result = false;
			goto IL_00f2;
			IL_00f2:
			return result;
			IL_0081:
			goodDest = Verb.tempDestList[i];
			result = true;
			goto IL_00f2;
		}

		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			bool result;
			if (this.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
			{
				result = false;
			}
			else
			{
				if (this.verbProps.requireLineOfSight)
				{
					if (!includeCorners)
					{
						if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
						{
							result = false;
							goto IL_00b6;
						}
					}
					else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
					{
						result = false;
						goto IL_00b6;
					}
				}
				result = true;
			}
			goto IL_00b6;
			IL_00b6:
			return result;
		}

		public override string ToString()
		{
			string text = (this.verbProps != null) ? (this.verbProps.label.NullOrEmpty() ? ((this.ownerHediffComp == null) ? ((this.ownerEquipment == null) ? ((this.LinkedBodyPartsGroup == null) ? "unknown" : this.LinkedBodyPartsGroup.defName) : this.ownerEquipment.def.label) : this.ownerHediffComp.Def.label) : this.verbProps.label) : "null";
			if (this.tool != null)
			{
				text = text + "/" + this.tool.Id;
			}
			return base.GetType().ToString() + "(" + text + ")";
		}
	}
}
