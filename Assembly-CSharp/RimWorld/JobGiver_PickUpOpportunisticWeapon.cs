using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PickUpOpportunisticWeapon : ThinkNode_JobGiver
	{
		private bool preferBuildingDestroyers;

		private float MinMeleeWeaponDPSThreshold
		{
			get
			{
				List<Tool> tools = ThingDefOf.Human.tools;
				float num = 0f;
				int num2 = 0;
				while (num2 < tools.Count)
				{
					if (tools[num2].linkedBodyPartsGroup != BodyPartGroupDefOf.LeftHand && tools[num2].linkedBodyPartsGroup != BodyPartGroupDefOf.RightHand)
					{
						num2++;
						continue;
					}
					num = tools[num2].power / tools[num2].cooldownTime;
					break;
				}
				return (float)(num + 2.0);
			}
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PickUpOpportunisticWeapon jobGiver_PickUpOpportunisticWeapon = (JobGiver_PickUpOpportunisticWeapon)base.DeepCopy(resolve);
			jobGiver_PickUpOpportunisticWeapon.preferBuildingDestroyers = this.preferBuildingDestroyers;
			return jobGiver_PickUpOpportunisticWeapon;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.equipment == null)
			{
				result = null;
			}
			else if (this.AlreadySatisfiedWithCurrentWeapon(pawn))
			{
				result = null;
			}
			else if (pawn.RaceProps.Humanlike && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = null;
			}
			else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				result = null;
			}
			else
			{
				Region region = pawn.GetRegion(RegionType.Set_Passable);
				if (region == null)
				{
					result = null;
				}
				else
				{
					Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 8f, (Predicate<Thing>)((Thing x) => pawn.CanReserve(x, 1, -1, null, false) && this.ShouldEquip(x, pawn)), null, 0, 15, false, RegionType.Set_Passable, false);
					result = ((thing == null) ? null : new Job(JobDefOf.Equip, thing));
				}
			}
			return result;
		}

		private bool AlreadySatisfiedWithCurrentWeapon(Pawn pawn)
		{
			ThingWithComps primary = pawn.equipment.Primary;
			bool result;
			if (primary == null)
			{
				result = false;
			}
			else
			{
				if (this.preferBuildingDestroyers)
				{
					if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
					{
						result = false;
						goto IL_0072;
					}
				}
				else if (!primary.def.IsRangedWeapon)
				{
					result = false;
					goto IL_0072;
				}
				result = true;
			}
			goto IL_0072;
			IL_0072:
			return result;
		}

		private bool ShouldEquip(Thing newWep, Pawn pawn)
		{
			return this.GetWeaponScore(newWep) > this.GetWeaponScore(pawn.equipment.Primary);
		}

		private int GetWeaponScore(Thing wep)
		{
			return (wep != null) ? ((!wep.def.IsMeleeWeapon || !(wep.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true) < this.MinMeleeWeaponDPSThreshold)) ? ((!this.preferBuildingDestroyers || !wep.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.ai_IsBuildingDestroyer) ? ((!wep.def.IsRangedWeapon) ? 1 : 2) : 3) : 0) : 0;
		}
	}
}
