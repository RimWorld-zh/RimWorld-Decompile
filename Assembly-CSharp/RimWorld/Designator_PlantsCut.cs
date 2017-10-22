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
			Texture2D result;
			if (!t.def.plant.IsTree)
			{
				result = base.IconReverseDesignating(t, out angle);
			}
			else
			{
				angle = 0f;
				result = TexCommand.TreeChop;
			}
			return result;
		}

		public override string LabelCapReverseDesignating(Thing t)
		{
			return t.def.plant.IsTree ? "DesignatorHarvestWood".Translate() : base.LabelCapReverseDesignating(t);
		}

		public override string DescReverseDesignating(Thing t)
		{
			return t.def.plant.IsTree ? "DesignatorHarvestWoodDesc".Translate() : base.DescReverseDesignating(t);
		}
	}
}
