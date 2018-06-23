using System;
using System.Collections;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C30 RID: 3120
	public static class ThingListGroupHelper
	{
		// Token: 0x04002EB3 RID: 11955
		public static readonly ThingRequestGroup[] AllGroups;

		// Token: 0x06004498 RID: 17560 RVA: 0x00241140 File Offset: 0x0023F540
		static ThingListGroupHelper()
		{
			int length = Enum.GetValues(typeof(ThingRequestGroup)).Length;
			ThingListGroupHelper.AllGroups = new ThingRequestGroup[length];
			int num = 0;
			IEnumerator enumerator = Enum.GetValues(typeof(ThingRequestGroup)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					ThingListGroupHelper.AllGroups[num] = (ThingRequestGroup)obj;
					num++;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x002411DC File Offset: 0x0023F5DC
		public static bool Includes(this ThingRequestGroup group, ThingDef def)
		{
			bool result;
			switch (group)
			{
			case ThingRequestGroup.Undefined:
				result = false;
				break;
			case ThingRequestGroup.Nothing:
				result = false;
				break;
			case ThingRequestGroup.Everything:
				result = true;
				break;
			case ThingRequestGroup.HaulableEver:
				result = def.EverHaulable;
				break;
			case ThingRequestGroup.HaulableAlways:
				result = def.alwaysHaulable;
				break;
			case ThingRequestGroup.FoodSource:
				result = (def.IsNutritionGivingIngestible || def.thingClass == typeof(Building_NutrientPasteDispenser));
				break;
			case ThingRequestGroup.FoodSourceNotPlantOrTree:
				result = ((def.IsNutritionGivingIngestible && (def.ingestible.foodType & ~FoodTypeFlags.Plant & ~FoodTypeFlags.Tree) != FoodTypeFlags.None) || def.thingClass == typeof(Building_NutrientPasteDispenser));
				break;
			case ThingRequestGroup.Corpse:
				result = (def.thingClass == typeof(Corpse));
				break;
			case ThingRequestGroup.Blueprint:
				result = def.IsBlueprint;
				break;
			case ThingRequestGroup.BuildingArtificial:
				result = def.IsBuildingArtificial;
				break;
			case ThingRequestGroup.BuildingFrame:
				result = def.IsFrame;
				break;
			case ThingRequestGroup.Pawn:
				result = (def.category == ThingCategory.Pawn);
				break;
			case ThingRequestGroup.PotentialBillGiver:
				result = !def.AllRecipes.NullOrEmpty<RecipeDef>();
				break;
			case ThingRequestGroup.Medicine:
				result = def.IsMedicine;
				break;
			case ThingRequestGroup.Filth:
				result = (def.filth != null);
				break;
			case ThingRequestGroup.AttackTarget:
				result = typeof(IAttackTarget).IsAssignableFrom(def.thingClass);
				break;
			case ThingRequestGroup.Weapon:
				result = def.IsWeapon;
				break;
			case ThingRequestGroup.Refuelable:
				result = def.HasComp(typeof(CompRefuelable));
				break;
			case ThingRequestGroup.HaulableEverOrMinifiable:
				result = (def.EverHaulable || def.Minifiable);
				break;
			case ThingRequestGroup.Drug:
				result = def.IsDrug;
				break;
			case ThingRequestGroup.Shell:
				result = def.IsShell;
				break;
			case ThingRequestGroup.HarvestablePlant:
				result = (def.category == ThingCategory.Plant && def.plant.Harvestable);
				break;
			case ThingRequestGroup.Fire:
				result = typeof(Fire).IsAssignableFrom(def.thingClass);
				break;
			case ThingRequestGroup.Plant:
				result = (def.category == ThingCategory.Plant);
				break;
			case ThingRequestGroup.Construction:
				result = (def.IsBlueprint || def.IsFrame);
				break;
			case ThingRequestGroup.HasGUIOverlay:
				result = def.drawGUIOverlay;
				break;
			case ThingRequestGroup.Apparel:
				result = def.IsApparel;
				break;
			case ThingRequestGroup.MinifiedThing:
				result = typeof(MinifiedThing).IsAssignableFrom(def.thingClass);
				break;
			case ThingRequestGroup.Grave:
				result = typeof(Building_Grave).IsAssignableFrom(def.thingClass);
				break;
			case ThingRequestGroup.Art:
				result = def.HasComp(typeof(CompArt));
				break;
			case ThingRequestGroup.ThingHolder:
				result = def.ThisOrAnyCompIsThingHolder();
				break;
			case ThingRequestGroup.ActiveDropPod:
				result = typeof(IActiveDropPod).IsAssignableFrom(def.thingClass);
				break;
			case ThingRequestGroup.Transporter:
				result = def.HasComp(typeof(CompTransporter));
				break;
			case ThingRequestGroup.LongRangeMineralScanner:
				result = def.HasComp(typeof(CompLongRangeMineralScanner));
				break;
			case ThingRequestGroup.AffectsSky:
				result = def.HasComp(typeof(CompAffectsSky));
				break;
			case ThingRequestGroup.PsychicDroneEmanator:
				result = def.HasComp(typeof(CompPsychicDrone));
				break;
			case ThingRequestGroup.WindSource:
				result = def.HasComp(typeof(CompWindSource));
				break;
			case ThingRequestGroup.AlwaysFlee:
				result = def.alwaysFlee;
				break;
			case ThingRequestGroup.ResearchBench:
				result = typeof(Building_ResearchBench).IsAssignableFrom(def.thingClass);
				break;
			case ThingRequestGroup.Facility:
				result = def.HasComp(typeof(CompFacility));
				break;
			case ThingRequestGroup.AffectedByFacilities:
				result = def.HasComp(typeof(CompAffectedByFacilities));
				break;
			case ThingRequestGroup.CreatesInfestations:
				result = def.HasComp(typeof(CompCreatesInfestations));
				break;
			default:
				throw new ArgumentException("group");
			}
			return result;
		}
	}
}
