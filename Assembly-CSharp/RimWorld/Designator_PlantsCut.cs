using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_PlantsCut : Designator_Plants
	{
		public Designator_PlantsCut()
		{
			base.defaultLabel = "DesignatorCutPlants".Translate();
			base.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateCutPlants;
			base.hotKey = KeyBindingDefOf.Misc3;
			base.designationDef = DesignationDefOf.CutPlant;
		}

		public override Texture2D IconReverseDesignating(Thing t, out float angle)
		{
			if (!t.def.plant.IsTree)
			{
				return base.IconReverseDesignating(t, out angle);
			}
			angle = 0f;
			return TexCommand.TreeChop;
		}

		public override string LabelCapReverseDesignating(Thing t)
		{
			if (!t.def.plant.IsTree)
			{
				return base.LabelCapReverseDesignating(t);
			}
			return "DesignatorHarvestWood".Translate();
		}

		public override string DescReverseDesignating(Thing t)
		{
			if (!t.def.plant.IsTree)
			{
				return base.DescReverseDesignating(t);
			}
			return "DesignatorHarvestWoodDesc".Translate();
		}
	}
}
