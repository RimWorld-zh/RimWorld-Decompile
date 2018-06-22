using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F0 RID: 2032
	public class Designator_PlantsHarvest : Designator_Plants
	{
		// Token: 0x06002D1D RID: 11549 RVA: 0x0017B210 File Offset: 0x00179610
		public Designator_PlantsHarvest()
		{
			this.defaultLabel = "DesignatorHarvest".Translate();
			this.defaultDesc = "DesignatorHarvestDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Harvest", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Harvest;
			this.hotKey = KeyBindingDefOf.Misc2;
			this.designationDef = DesignationDefOf.HarvestPlant;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x0017B294 File Offset: 0x00179694
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
				if (!plant.HarvestableNow || plant.def.plant.harvestTag != "Standard")
				{
					result = "MessageMustDesignateHarvestable".Translate();
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x0017B310 File Offset: 0x00179710
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Standard";
		}
	}
}
