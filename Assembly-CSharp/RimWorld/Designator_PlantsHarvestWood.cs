using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F3 RID: 2035
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x06002D23 RID: 11555 RVA: 0x0017B6FC File Offset: 0x00179AFC
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

		// Token: 0x06002D24 RID: 11556 RVA: 0x0017B78C File Offset: 0x00179B8C
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

		// Token: 0x06002D25 RID: 11557 RVA: 0x0017B808 File Offset: 0x00179C08
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Wood";
		}
	}
}
