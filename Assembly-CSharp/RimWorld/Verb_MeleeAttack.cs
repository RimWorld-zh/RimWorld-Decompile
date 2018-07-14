using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public abstract class Verb_MeleeAttack : Verb
	{
		private const int TargetCooldown = 50;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ManeuverDef, RulePackDef> <>f__am$cache2;

		protected Verb_MeleeAttack()
		{
		}

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
					casterPawn.skills.Learn(SkillDefOf.Melee, 200f * this.verbProps.AdjustedFullCycleTime(this, casterPawn), false);
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
				BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackGetter(this.maneuver), alwaysShow, base.CasterPawn, this.currentTarget.Thing, base.ImplementOwnerType, (!this.tool.labelUsedInLogging) ? "" : this.tool.label, (base.EquipmentSource != null) ? base.EquipmentSource.def : null, (base.HediffCompSource != null) ? base.HediffCompSource.Def : null, this.maneuver.logEntryDef);
				Find.BattleLog.Add(battleLogEntry_MeleeCombat);
				result = battleLogEntry_MeleeCombat;
			}
			return result;
		}

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

		private bool IsTargetImmobile(LocalTargetInfo target)
		{
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			return thing.def.category != ThingCategory.Pawn || pawn.Downed || pawn.GetPosture() != PawnPosture.Standing;
		}

		protected abstract DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target);

		private SoundDef SoundHitPawn()
		{
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
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

		private SoundDef SoundHitBuilding()
		{
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
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

		[CompilerGenerated]
		private static RulePackDef <TryCastShot>m__0(ManeuverDef maneuver)
		{
			return maneuver.combatLogRulesHit;
		}

		[CompilerGenerated]
		private static RulePackDef <TryCastShot>m__1(ManeuverDef maneuver)
		{
			return maneuver.combatLogRulesDodge;
		}

		[CompilerGenerated]
		private static RulePackDef <TryCastShot>m__2(ManeuverDef maneuver)
		{
			return maneuver.combatLogRulesMiss;
		}
	}
}
