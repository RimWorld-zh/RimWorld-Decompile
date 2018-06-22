using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F1 RID: 2033
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x06002D20 RID: 11552 RVA: 0x0017B348 File Offset: 0x00179748
		public Designator_PlantsHarvestWood()
		{
			this.defaultLabel = "DesignatorHarvestWood".Translate();
			this.defaultDesc = "DesignatorHarvestWoodDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Harvest;
			this.hotKey = KeyBindingDefOf.Misc1;
			this.designationDef = DesignationDefOf.HarvestPlant;
			this.tutorTag = "PlantsHarvestWood";
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x0017B3D8 File Offset: 0x001797D8
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
				if (!plant.HarvestableNow || plant.def.plant.harvestTag != "Wood")
				{
					result = "MessageMustDesignateHarvestableWood".Translate();
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x0017B454 File Offset: 0x00179854
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Wood";
		}
	}
}
