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
			AcceptanceReport acceptanceReport = base.CanDesignateThing(t);
			AcceptanceReport result;
			if (!acceptanceReport.Accepted)
			{
				result = acceptanceReport;
			}
			else
			{
				Plant plant = (Plant)t;
				result = ((plant.HarvestableNow && !(plant.def.plant.harvestTag != "Standard")) ? true : "MessageMustDesignateHarvestable".Translate());
			}
			return result;
		}
	}
}
