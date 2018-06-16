using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F5 RID: 2037
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x06002D25 RID: 11557 RVA: 0x0017B0DC File Offset: 0x001794DC
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

		// Token: 0x06002D26 RID: 11558 RVA: 0x0017B16C File Offset: 0x0017956C
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

		// Token: 0x06002D27 RID: 11559 RVA: 0x0017B1E8 File Offset: 0x001795E8
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Wood";
		}
	}
}
