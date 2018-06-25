using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F1 RID: 2033
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x06002D1C RID: 11548 RVA: 0x0017B468 File Offset: 0x00179868
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

		// Token: 0x06002D1D RID: 11549 RVA: 0x0017B4EC File Offset: 0x001798EC
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

		// Token: 0x06002D1E RID: 11550 RVA: 0x0017B53C File Offset: 0x0017993C
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

		// Token: 0x06002D1F RID: 11551 RVA: 0x0017B580 File Offset: 0x00179980
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
