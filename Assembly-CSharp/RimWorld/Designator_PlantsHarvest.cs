using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_PlantsHarvest : Designator_Plants
	{
		public Designator_PlantsHarvest()
		{
			base.defaultLabel = "DesignatorHarvest".Translate();
			base.defaultDesc = "DesignatorHarvestDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Harvest", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateHarvest;
			base.hotKey = KeyBindingDefOf.Misc2;
			base.designationDef = DesignationDefOf.HarvestPlant;
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (plant.HarvestableNow && !(plant.def.plant.harvestTag != "Standard"))
			{
				return true;
			}
			return "MessageMustDesignateHarvestable".Translate();
		}
	}
}
