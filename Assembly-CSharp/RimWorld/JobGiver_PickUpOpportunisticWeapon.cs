using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PickUpOpportunisticWeapon : ThinkNode_JobGiver
	{
		private bool preferBuildingDestroyers;

		public JobGiver_PickUpOpportunisticWeapon()
		{
		}

		private float MinMeleeWeaponDPSThreshold
		{
			get
			{
				List<Tool> tools = ThingDefOf.Human.tools;
				float num = 0f;
				for (int i = 0; i < tools.Count; i++)
				{
					if (tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.LeftHand || tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.RightHand)
					{
						num = tools[i].power / tools[i].cooldownTime;
						break;
					}
				}
				return num + 2f;
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
			else if (pawn.GetRegion(RegionType.Set_Passable) == null)
			{
				result = null;
			}
			else
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && this.ShouldEquip(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false);
				if (thing != null)
				{
					result = new Job(JobDefOf.Equip, thing);
				}
				else
				{
					result = null;
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
						return false;
					}
				}
				else if (!primary.def.IsRangedWeapon)
				{
					return false;
				}
				result = true;
			}
			return result;
		}

		private bool ShouldEquip(Thing newWep, Pawn pawn)
		{
			return this.GetWeaponScore(newWep) > this.GetWeaponScore(pawn.equipment.Primary);
		}

		private int GetWeaponScore(Thing wep)
		{
			int result;
			if (wep == null)
			{
				result = 0;
			}
			else if (wep.def.IsMeleeWeapon && wep.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true) < this.MinMeleeWeaponDPSThreshold)
			{
				result = 0;
			}
			else if (this.preferBuildingDestroyers && wep.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				result = 3;
			}
			else if (wep.def.IsRangedWeapon)
			{
				result = 2;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			internal JobGiver_PickUpOpportunisticWeapon $this;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return this.pawn.CanReserve(x, 1, -1, null, false) && this.$this.ShouldEquip(x, this.pawn);
			}
		}
	}
}
