using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F2 RID: 2034
	public class Designator_PlantsHarvest : Designator_Plants
	{
		// Token: 0x06002D20 RID: 11552 RVA: 0x0017B5C4 File Offset: 0x001799C4
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

		// Token: 0x06002D21 RID: 11553 RVA: 0x0017B648 File Offset: 0x00179A48
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

		// Token: 0x06002D22 RID: 11554 RVA: 0x0017B6C4 File Offset: 0x00179AC4
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Standard";
		}
	}
}
