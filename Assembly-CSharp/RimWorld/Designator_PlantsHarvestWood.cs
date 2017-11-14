using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		public Designator_PlantsHarvestWood()
		{
			base.defaultLabel = "DesignatorHarvestWood".Translate();
			base.defaultDesc = "DesignatorHarvestWoodDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateHarvest;
			base.hotKey = KeyBindingDefOf.Misc1;
			base.designationDef = DesignationDefOf.HarvestPlant;
			base.tutorTag = "PlantsHarvestWood";
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (plant.HarvestableNow && !(plant.def.plant.harvestTag != "Wood"))
			{
				return true;
			}
			return "MessageMustDesignateHarvestableWood".Translate();
		}
	}
}
