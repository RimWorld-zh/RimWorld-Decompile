using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EF RID: 2031
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x06002D19 RID: 11545 RVA: 0x0017B0B4 File Offset: 0x001794B4
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

		// Token: 0x06002D1A RID: 11546 RVA: 0x0017B138 File Offset: 0x00179538
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

		// Token: 0x06002D1B RID: 11547 RVA: 0x0017B188 File Offset: 0x00179588
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

		// Token: 0x06002D1C RID: 11548 RVA: 0x0017B1CC File Offset: 0x001795CC
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
