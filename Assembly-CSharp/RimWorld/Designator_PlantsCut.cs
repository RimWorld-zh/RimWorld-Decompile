using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_PlantsCut : Designator_Plants
	{
		public Designator_PlantsCut()
		{
			this.defaultLabel = "DesignatorCutPlants".Translate();
			this.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_CutPlants;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.designationDef = DesignationDefOf.CutPlant;
		}

		public override Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			Texture2D result;
			if (!t.def.plant.IsTree)
			{
				result = base.IconReverseDesignating(t, out angle, out offset);
			}
			else
			{
				angle = 0f;
				offset = default(Vector2);
				result = TexCommand.TreeChop;
			}
			return result;
		}

		public override string LabelCapReverseDesignating(Thing t)
		{
			string result;
			if (!t.def.plant.IsTree)
			{
				result = base.LabelCapReverseDesignating(t);
			}
			else
			{
				result = "DesignatorHarvestWood".Translate();
			}
			return result;
		}

		public override string DescReverseDesignating(Thing t)
		{
			string result;
			if (!t.def.plant.IsTree)
			{
				result = base.DescReverseDesignating(t);
			}
			else
			{
				result = "DesignatorHarvestWoodDesc".Translate();
			}
			return result;
		}
	}
}
