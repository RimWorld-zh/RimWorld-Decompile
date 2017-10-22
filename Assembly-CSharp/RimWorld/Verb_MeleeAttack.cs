using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Verb_MeleeAttack : Verb
	{
		private const int TargetCooldown = 50;

		protected override bool TryCastShot()
		{
			Pawn casterPawn = base.CasterPawn;
			if (casterPawn.stances.FullBodyBusy)
			{
				return false;
			}
			Thing thing = base.currentTarget.Thing;
			if (!base.CanHitTarget(thing))
			{
				Log.Warning(casterPawn + " meleed " + thing + " from out of melee position.");
			}
			casterPawn.Drawer.rotator.Face(thing.DrawPos);
			if (!this.IsTargetImmobile(base.currentTarget) && casterPawn.skills != null)
			{
				casterPawn.skills.Learn(SkillDefOf.Melee, 250f, false);
			}
			bool result;
			SoundDef soundDef;
			if (Rand.Value < this.GetNonMissChance(thing))
			{
				if (Rand.Value > this.GetDodgeChance(thing))
				{
					result = true;
					this.ApplyMeleeDamageToTarget(base.currentTarget);
					soundDef = ((thing.def.category != ThingCategory.Building) ? this.SoundHitPawn() : this.SoundHitBuilding());
				}
				else
				{
					result = false;
					soundDef = this.SoundMiss();
					MoteMaker.ThrowText(thing.DrawPos, thing.Map, "TextMote_Dodge".Translate(), 1.9f);
				}
			}
			else
			{
				result = false;
				soundDef = this.SoundMiss();
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
			casterPawn.Drawer.rotator.FaceCell(thing.Position);
			if (casterPawn.caller != null)
			{
				casterPawn.caller.Notify_DidMeleeAttack();
			}
			return result;
		}

		private IEnumerable<DamageInfo> DamageInfosToApply(LocalTargetInfo target)
		{
			float damAmount = (float)base.verbProps.AdjustedMeleeDamageAmount(this, base.CasterPawn, base.ownerEquipment);
			DamageDef damDef = base.verbProps.meleeDamageDef;
			BodyPartGroupDef bodyPartGroupDef = null;
			HediffDef hediffDef = null;
			if (base.CasterIsPawn)
			{
				if (damAmount >= 1.0)
				{
					bodyPartGroupDef = base.verbProps.linkedBodyPartsGroup;
					if (base.ownerHediffComp != null)
					{
						hediffDef = base.ownerHediffComp.Def;
					}
				}
				else
				{
					damAmount = 1f;
					damDef = DamageDefOf.Blunt;
				}
			}
			ThingDef source = (base.ownerEquipment == null) ? base.CasterPawn.def : base.ownerEquipment.def;
			Vector3 direction = (target.Thing.Position - base.CasterPawn.Position).ToVector3();
			Thing caster = base.caster;
			DamageInfo mainDinfo = new DamageInfo(damDef, GenMath.RoundRandom(damAmount), -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown);
			mainDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			mainDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
			mainDinfo.SetWeaponHediff(hediffDef);
			mainDinfo.SetAngle(direction);
			yield return mainDinfo;
			if (base.surpriseAttack && base.verbProps.surpriseAttack != null && base.verbProps.surpriseAttack.extraMeleeDamages != null)
			{
				List<ExtraMeleeDamage> extraDamages = base.verbProps.surpriseAttack.extraMeleeDamages;
				for (int i = 0; i < extraDamages.Count; i++)
				{
					ExtraMeleeDamage extraDamage = extraDamages[i];
					int amount = GenMath.RoundRandom((float)extraDamage.amount * base.GetDamageFactorFor(base.CasterPawn));
					caster = base.caster;
					DamageInfo extraDinfo = new DamageInfo(extraDamage.def, amount, -1f, caster, null, source, DamageInfo.SourceCategory.ThingOrUnknown);
					extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
					extraDinfo.SetWeaponHediff(hediffDef);
					extraDinfo.SetAngle(direction);
					yield return extraDinfo;
				}
			}
		}

		private float GetNonMissChance(LocalTargetInfo target)
		{
			if (base.surpriseAttack)
			{
				return 1f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 1f;
			}
			return base.CasterPawn.GetStatValue(StatDefOf.MeleeHitChance, true);
		}

		private float GetDodgeChance(LocalTargetInfo target)
		{
			if (base.surpriseAttack)
			{
				return 0f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 0f;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && stance_Busy.verb != null && !stance_Busy.verb.verbProps.MeleeRange)
			{
				return 0f;
			}
			return pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true);
		}

		private bool IsTargetImmobile(LocalTargetInfo target)
		{
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			return thing.def.category != ThingCategory.Pawn || pawn.Downed || pawn.GetPosture() != PawnPosture.Standing;
		}

		private void ApplyMeleeDamageToTarget(LocalTargetInfo target)
		{
			foreach (DamageInfo item in this.DamageInfosToApply(target))
			{
				if (!target.ThingDestroyed)
				{
					target.Thing.TakeDamage(item);
					continue;
				}
				break;
			}
		}

		private SoundDef SoundHitPawn()
		{
			if (base.ownerEquipment != null && base.ownerEquipment.Stuff != null)
			{
				if (base.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (base.CasterPawn != null && !base.CasterPawn.def.race.soundMeleeHitPawn.NullOrUndefined())
			{
				return base.CasterPawn.def.race.soundMeleeHitPawn;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitPawn;
		}

		private SoundDef SoundHitBuilding()
		{
			if (base.ownerEquipment != null && base.ownerEquipment.Stuff != null)
			{
				if (base.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.ownerEquipment.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.ownerEquipment.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (base.CasterPawn != null && !base.CasterPawn.def.race.soundMeleeHitBuilding.NullOrUndefined())
			{
				return base.CasterPawn.def.race.soundMeleeHitBuilding;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitBuilding;
		}

		private SoundDef SoundMiss()
		{
			if (base.CasterPawn != null && !base.CasterPawn.def.race.soundMeleeMiss.NullOrUndefined())
			{
				return base.CasterPawn.def.race.soundMeleeMiss;
			}
			return SoundDefOf.Pawn_Melee_Punch_Miss;
		}
	}
}
