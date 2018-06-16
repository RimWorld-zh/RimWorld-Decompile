using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A1 RID: 929
	public class SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant : SymbolResolver
	{
		// Token: 0x0600102D RID: 4141 RVA: 0x000881B4 File Offset: 0x000865B4
		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else if (BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings)
			{
				result = false;
			}
			else if (BaseGen.globalSettings.basePart_emptyNodesResolved < BaseGen.globalSettings.minEmptyNodes)
			{
				result = false;
			}
			else if (BaseGen.globalSettings.basePart_powerPlantsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area >= 0.09f)
			{
				result = false;
			}
			else if (rp.faction != null && rp.faction.def.techLevel < TechLevel.Industrial)
			{
				result = false;
			}
			else if (rp.rect.Width > 13 || rp.rect.Height > 13)
			{
				result = false;
			}
			else
			{
				this.CalculateAvailablePowerPlants(rp.rect);
				result = SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Any<ThingDef>();
			}
			return result;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000882C4 File Offset: 0x000866C4
		public override void Resolve(ResolveParams rp)
		{
			this.CalculateAvailablePowerPlants(rp.rect);
			if (SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Any<ThingDef>())
			{
				BaseGen.symbolStack.Push("refuel", rp);
				ThingDef thingDef = SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.RandomElement<ThingDef>();
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = thingDef;
				int? fillWithThingsPadding = rp.fillWithThingsPadding;
				resolveParams.fillWithThingsPadding = new int?((fillWithThingsPadding == null) ? Mathf.Max(5 - thingDef.size.x, 1) : fillWithThingsPadding.Value);
				BaseGen.symbolStack.Push("fillWithThings", resolveParams);
				BaseGen.globalSettings.basePart_powerPlantsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0008839C File Offset: 0x0008679C
		private void CalculateAvailablePowerPlants(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Clear();
			if (rect.Width >= ThingDefOf.SolarGenerator.size.x && rect.Height >= ThingDefOf.SolarGenerator.size.z)
			{
				int num = 0;
				CellRect.CellRectIterator iterator = rect.GetIterator();
				while (!iterator.Done())
				{
					if (!iterator.Current.Roofed(map))
					{
						num++;
					}
					iterator.MoveNext();
				}
				if ((float)num / (float)rect.Area >= 0.5f)
				{
					SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Add(ThingDefOf.SolarGenerator);
				}
			}
			if (rect.Width >= ThingDefOf.WoodFiredGenerator.size.x && rect.Height >= ThingDefOf.WoodFiredGenerator.size.z)
			{
				SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Add(ThingDefOf.WoodFiredGenerator);
			}
		}

		// Token: 0x04000A0C RID: 2572
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x04000A0D RID: 2573
		private const float MaxCoverage = 0.09f;
	}
}
