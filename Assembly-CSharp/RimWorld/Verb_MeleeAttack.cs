using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020009D6 RID: 2518
	public abstract class Verb_MeleeAttack : Verb
	{
		// Token: 0x0400240A RID: 9226
		private const int TargetCooldown = 50;

		// Token: 0x0600386C RID: 14444 RVA: 0x001E1E0C File Offset: 0x001E020C
		protected override bool TryCastShot()
		{
			Pawn casterPawn = base.CasterPawn;
			bool result;
			if (!casterPawn.Spawned)
			{
				result = false;
			}
			else if (casterPawn.stances.FullBodyBusy)
			{
				result = false;
			}
			else
			{
				Thing thing = this.currentTarget.Thing;
				if (!base.CanHitTarget(thing))
				{
					Log.Warning(string.Concat(new object[]
					{
						casterPawn,
						" meleed ",
						thing,
						" from out of melee position."
					}), false);
				}
				casterPawn.rotationTracker.Face(thing.DrawPos);
				if (!this.IsTargetImmobile(this.currentTarget) && casterPawn.skills != null)
				{
					casterPawn.skills.Learn(SkillDefOf.Melee, 200f * this.verbProps.AdjustedFullCycleTime(this, casterPawn, this.ownerEquipment), false);
				}
				Pawn pawn = thing as Pawn;
				if (pawn != null && !pawn.Dead)
				{
					if (casterPawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn.MentalStateDef != MentalStateDefOf.SocialFighting)
					{
						pawn.mindState.meleeThreat = casterPawn;
						pawn.mindState.lastMeleeThreatHarmTick = Find.TickManager.TicksGame;
					}
				}
				Map map = thing.Map;
				Vector3 drawPos = thing.DrawPos;
				SoundDef soundDef;
				bool flag;
				if (Rand.Chance(this.GetNonMissChance(thing)))
				{
					if (!Rand.Chance(this.GetDodgeChance(thing)))
					{
						if (thing.def.category == ThingCategory.Building)
						{
							soundDef = this.SoundHitBuilding();
						}
						else
						{
							soundDef = this.SoundHitPawn();
						}
						if (this.verbProps.impactMote != null)
						{
							MoteMaker.MakeStaticMote(drawPos, map, this.verbProps.impactMote, 1f);
						}
						BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesHit, true);
						flag = true;
						DamageWorker.DamageResult damageResult = this.ApplyMeleeDamageToTarget(this.currentTarget);
						damageResult.AssociateWithLog(battleLogEntry_MeleeCombat);
						if (damageResult.deflected)
						{
							battleLogEntry_MeleeCombat.RuleDef = this.maneuver.combatLogRulesDeflect;
							battleLogEntry_MeleeCombat.alwaysShowInCompact = false;
						}
					}
					else
					{
						flag = false;
						soundDef = this.SoundMiss();
						MoteMaker.ThrowText(drawPos, map, "TextMote_Dodge".Translate(), 1.9f);
						this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesDodge, false);
					}
				}
				else
				{
					flag = false;
					soundDef = this.SoundMiss();
					this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesMiss, false);
				}
				soundDef.PlayOneShot(new TargetInfo(thing.Position, map, false));
				if (casterPawn.Spawned)
				{
					casterPawn.Drawer.Notify_MeleeAttackOn(thing);
				}
				if (pawn != null && !pawn.Dead)
				{
					if (pawn.Spawned)
					{
						pawn.stances.StaggerFor(95);
					}
				}
				if (casterPawn.Spawned)
				{
					casterPawn.rotationTracker.FaceCell(thing.Position);
				}
				if (casterPawn.caller != null)
				{
					casterPawn.caller.Notify_DidMeleeAttack();
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x001E2160 File Offset: 0x001E0560
		public BattleLogEntry_MeleeCombat CreateCombatLog(Func<ManeuverDef, RulePackDef> rulePackGetter, bool alwaysShow)
		{
			BattleLogEntry_MeleeCombat result;
			if (this.maneuver == null)
			{
				result = null;
			}
			else if (this.tool == null)
			{
				result = null;
			}
			else
			{
				BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackGetter(this.maneuver), alwaysShow, base.CasterPawn, this.currentTarget.Thing, this.implementOwnerType, (!this.tool.labelUsedInLogging) ? "" : this.tool.label, (this.ownerEquipment != null) ? this.ownerEquipment.def : null, (this.ownerHediffComp != null) ? this.ownerHediffComp.Def : null, this.maneuver.logEntryDef);
				Find.BattleLog.Add(battleLogEntry_MeleeCombat);
				result = battleLogEntry_MeleeCombat;
			}
			return result;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x001E2238 File Offset: 0x001E0638
		private float GetNonMissChance(LocalTargetInfo target)
		{
			float result;
			if (this.surpriseAttack)
			{
				result = 1f;
			}
			else if (this.IsTargetImmobile(target))
			{
				result = 1f;
			}
			else
			{
				result = base.CasterPawn.GetStatValue(StatDefOf.MeleeHitChance, true);
			}
			return result;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x001E228C File Offset: 0x001E068C
		private float GetDodgeChance(LocalTargetInfo target)
		{
			float result;
			if (this.surpriseAttack)
			{
				result = 0f;
			}
			else if (this.IsTargetImmobile(target))
			{
				result = 0f;
			}
			else
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn == null)
				{
					result = 0f;
				}
				else
				{
					Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
					if (stance_Busy != null && stance_Busy.verb != null && !stance_Busy.verb.verbProps.IsMeleeAttack)
					{
						result = 0f;
					}
					else
					{
						result = pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true);
					}
				}
			}
			return result;
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x001E233C File Offset: 0x001E073C
		private bool IsTargetImmobile(LocalTargetInfo target)
		{
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			return thing.def.category != ThingCategory.Pawn || pawn.Downed || pawn.GetPosture() != PawnPosture.Standing;
		}

		// Token: 0x06003871 RID: 14449
		protected abstract DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target);

		// Token: 0x06003872 RID: 14450 RVA: 0x001E238C File Offset: 0x001E078C
		private SoundDef SoundHitPawn()
		{
			if (this.ownerEquipment != null && this.ownerEquipment.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!this.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return this.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!this.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return this.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (base.CasterPawn != null)
			{
				if (!base.CasterPawn.def.race.soundMeleeHitPawn.NullOrUndefined())
				{
					return base.CasterPawn.def.race.soundMeleeHitPawn;
				}
			}
			return SoundDefOf.Pawn_Melee_Punch_HitPawn;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x001E24A4 File Offset: 0x001E08A4
		private SoundDef SoundHitBuilding()
		{
			if (this.ownerEquipment != null && this.ownerEquipment.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!this.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return this.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!this.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return this.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (base.CasterPawn != null)
			{
				if (!base.CasterPawn.def.race.soundMeleeHitBuilding.NullOrUndefined())
				{
					return base.CasterPawn.def.race.soundMeleeHitBuilding;
				}
			}
			return SoundDefOf.Pawn_Melee_Punch_HitBuilding;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x001E25BC File Offset: 0x001E09BC
		private SoundDef SoundMiss()
		{
			if (base.CasterPawn != null)
			{
				if (!base.CasterPawn.def.race.soundMeleeMiss.NullOrUndefined())
				{
					return base.CasterPawn.def.race.soundMeleeMiss;
				}
			}
			return SoundDefOf.Pawn_Melee_Punch_Miss;
		}
	}
}
