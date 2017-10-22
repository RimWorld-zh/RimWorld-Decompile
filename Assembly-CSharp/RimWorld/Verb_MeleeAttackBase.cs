using Verse;
using Verse.Sound;

namespace RimWorld
{
	public abstract class Verb_MeleeAttackBase : Verb
	{
		private const int TargetCooldown = 50;

		protected override bool TryCastShot()
		{
			Pawn casterPawn = base.CasterPawn;
			bool result;
			if (casterPawn.stances.FullBodyBusy)
			{
				result = false;
			}
			else
			{
				Thing thing = base.currentTarget.Thing;
				if (!base.CanHitTarget(thing))
				{
					Log.Warning(casterPawn + " meleed " + thing + " from out of melee position.");
				}
				casterPawn.rotationTracker.Face(thing.DrawPos);
				if (!this.IsTargetImmobile(base.currentTarget) && casterPawn.skills != null)
				{
					casterPawn.skills.Learn(SkillDefOf.Melee, 250f, false);
				}
				bool flag;
				SoundDef soundDef;
				if (Rand.Value < this.GetNonMissChance(thing))
				{
					if (Rand.Value > this.GetDodgeChance(thing))
					{
						BattleLogEntry_MeleeCombat log = this.CreateCombatLog(RulePackDefOf.Combat_Hit);
						flag = true;
						this.ApplyMeleeDamageToTarget(base.currentTarget).InsertIntoLog(log);
						soundDef = ((thing.def.category != ThingCategory.Building) ? this.SoundHitPawn() : this.SoundHitBuilding());
					}
					else
					{
						flag = false;
						soundDef = this.SoundMiss();
						MoteMaker.ThrowText(thing.DrawPos, thing.Map, "TextMote_Dodge".Translate(), 1.9f);
						this.CreateCombatLog(RulePackDefOf.Combat_Dodge);
					}
				}
				else
				{
					flag = false;
					soundDef = this.SoundMiss();
					this.CreateCombatLog(RulePackDefOf.Combat_Miss);
				}
				soundDef.PlayOneShot(new TargetInfo(thing.Position, casterPawn.Map, false));
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
				Pawn pawn = thing as Pawn;
				if (pawn != null && !pawn.Dead)
				{
					pawn.stances.StaggerFor(95);
					if (casterPawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn.MentalStateDef != MentalStateDefOf.SocialFighting)
					{
						pawn.mindState.meleeThreat = casterPawn;
						pawn.mindState.lastMeleeThreatHarmTick = Find.TickManager.TicksGame;
					}
				}
				casterPawn.rotationTracker.FaceCell(thing.Position);
				if (casterPawn.caller != null)
				{
					casterPawn.caller.Notify_DidMeleeAttack();
				}
				result = flag;
			}
			return result;
		}

		public BattleLogEntry_MeleeCombat CreateCombatLog(RulePackDef rulePack)
		{
			Pawn pawn = base.currentTarget.Thing as Pawn;
			BattleLogEntry_MeleeCombat result;
			if (pawn == null)
			{
				result = null;
			}
			else if (base.maneuver == null)
			{
				result = null;
			}
			else if (base.tool == null)
			{
				result = null;
			}
			else
			{
				BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePack, base.maneuver.combatLogRules, base.CasterPawn, pawn, base.implementOwnerType, base.tool.label, (base.ownerEquipment != null) ? base.ownerEquipment.def : null, (base.ownerHediffComp != null) ? base.ownerHediffComp.Def : null);
				Find.BattleLog.Add(battleLogEntry_MeleeCombat);
				result = battleLogEntry_MeleeCombat;
			}
			return result;
		}

		private float GetNonMissChance(LocalTargetInfo target)
		{
			return (float)((!base.surpriseAttack) ? ((!this.IsTargetImmobile(target)) ? base.CasterPawn.GetStatValue(StatDefOf.MeleeHitChance, true) : 1.0) : 1.0);
		}

		private float GetDodgeChance(LocalTargetInfo target)
		{
			float result;
			if (base.surpriseAttack)
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
					result = (float)((stance_Busy == null || stance_Busy.verb == null || stance_Busy.verb.verbProps.MeleeRange) ? pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true) : 0.0);
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
			SoundDef result;
			if (base.ownerEquipment != null && base.ownerEquipment.Stuff != null)
			{
				if (base.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						result = base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp;
						goto IL_0107;
					}
				}
				else if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					result = base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt;
					goto IL_0107;
				}
			}
			result = ((base.CasterPawn == null || base.CasterPawn.def.race.soundMeleeHitPawn.NullOrUndefined()) ? SoundDefOf.Pawn_Melee_Punch_HitPawn : base.CasterPawn.def.race.soundMeleeHitPawn);
			goto IL_0107;
			IL_0107:
			return result;
		}

		private SoundDef SoundHitBuilding()
		{
			SoundDef result;
			if (base.ownerEquipment != null && base.ownerEquipment.Stuff != null)
			{
				if (base.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						result = base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp;
						goto IL_0107;
					}
				}
				else if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					result = base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt;
					goto IL_0107;
				}
			}
			result = ((base.CasterPawn == null || base.CasterPawn.def.race.soundMeleeHitBuilding.NullOrUndefined()) ? SoundDefOf.Pawn_Melee_Punch_HitBuilding : base.CasterPawn.def.race.soundMeleeHitBuilding);
			goto IL_0107;
			IL_0107:
			return result;
		}

		private SoundDef SoundMiss()
		{
			return (base.CasterPawn == null || base.CasterPawn.def.race.soundMeleeMiss.NullOrUndefined()) ? SoundDefOf.Pawn_Melee_Punch_Miss : base.CasterPawn.def.race.soundMeleeMiss;
		}
	}
}
