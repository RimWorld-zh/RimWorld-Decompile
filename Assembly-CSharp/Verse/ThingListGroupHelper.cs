using RimWorld;
using System;
using Verse.AI;

namespace Verse
{
	public static class ThingListGroupHelper
	{
		public static readonly ThingRequestGroup[] AllGroups;

		static ThingListGroupHelper()
		{
			int length = Enum.GetValues(typeof(ThingRequestGroup)).Length;
			ThingListGroupHelper.AllGroups = new ThingRequestGroup[length];
			int num = 0;
			foreach (object value in Enum.GetValues(typeof(ThingRequestGroup)))
			{
				ThingListGroupHelper.AllGroups[num] = (ThingRequestGroup)(byte)value;
				num++;
			}
		}

		public static bool Includes(this ThingRequestGroup group, ThingDef def)
		{
			switch (group)
			{
			case ThingRequestGroup.Undefined:
			{
				return false;
			}
			case ThingRequestGroup.Nothing:
			{
				return false;
			}
			case ThingRequestGroup.Everything:
			{
				return true;
			}
			case ThingRequestGroup.HaulableEver:
			{
				return def.EverHaulable;
			}
			case ThingRequestGroup.HaulableAlways:
			{
				return def.alwaysHaulable;
			}
			case ThingRequestGroup.Plant:
			{
				return def.category == ThingCategory.Plant;
			}
			case ThingRequestGroup.FoodSource:
			{
				return def.IsNutritionGivingIngestible || def.thingClass == typeof(Building_NutrientPasteDispenser);
			}
			case ThingRequestGroup.FoodSourceNotPlantOrTree:
			{
				return (def.IsNutritionGivingIngestible ? ((int)def.ingestible.foodType & -65 & -129) : 0) != 0 || def.thingClass == typeof(Building_NutrientPasteDispenser);
			}
			case ThingRequestGroup.HasGUIOverlay:
			{
				return def.drawGUIOverlay;
			}
			case ThingRequestGroup.Corpse:
			{
				return def.thingClass == typeof(Corpse);
			}
			case ThingRequestGroup.Blueprint:
			{
				return def.IsBlueprint;
			}
			case ThingRequestGroup.Construction:
			{
				return def.IsBlueprint || def.IsFrame;
			}
			case ThingRequestGroup.BuildingArtificial:
			{
				return (def.category == ThingCategory.Building || def.IsFrame) && (def.building == null || (!def.building.isNaturalRock && !def.building.isResourceRock));
			}
			case ThingRequestGroup.BuildingFrame:
			{
				return def.IsFrame;
			}
			case ThingRequestGroup.Pawn:
			{
				return def.category == ThingCategory.Pawn;
			}
			case ThingRequestGroup.PotentialBillGiver:
			{
				return !def.AllRecipes.NullOrEmpty();
			}
			case ThingRequestGroup.Medicine:
			{
				return def.IsMedicine;
			}
			case ThingRequestGroup.Apparel:
			{
				return def.IsApparel;
			}
			case ThingRequestGroup.MinifiedThing:
			{
				return typeof(MinifiedThing).IsAssignableFrom(def.thingClass);
			}
			case ThingRequestGroup.Filth:
			{
				return def.filth != null;
			}
			case ThingRequestGroup.AttackTarget:
			{
				return typeof(IAttackTarget).IsAssignableFrom(def.thingClass);
			}
			case ThingRequestGroup.Weapon:
			{
				return def.IsWeapon;
			}
			case ThingRequestGroup.Refuelable:
			{
				return def.HasComp(typeof(CompRefuelable));
			}
			case ThingRequestGroup.HaulableEverOrMinifiable:
			{
				return def.EverHaulable || def.Minifiable;
			}
			case ThingRequestGroup.Drug:
			{
				return def.IsDrug;
			}
			case ThingRequestGroup.Grave:
			{
				return typeof(Building_Grave).IsAssignableFrom(def.thingClass);
			}
			case ThingRequestGroup.Art:
			{
				return def.HasComp(typeof(CompArt));
			}
			case ThingRequestGroup.ThisOrAnyCompIsThingHolder:
			{
				return def.ThisOrAnyCompIsThingHolder();
			}
			case ThingRequestGroup.ActiveDropPod:
			{
				return typeof(IActiveDropPod).IsAssignableFrom(def.thingClass);
			}
			case ThingRequestGroup.Transporter:
			{
				return def.HasComp(typeof(CompTransporter));
			}
			case ThingRequestGroup.LongRangeMineralScanner:
			{
				return def.HasComp(typeof(CompLongRangeMineralScanner));
			}
			default:
			{
				throw new ArgumentException("group");
			}
			}
		}
	}
}
