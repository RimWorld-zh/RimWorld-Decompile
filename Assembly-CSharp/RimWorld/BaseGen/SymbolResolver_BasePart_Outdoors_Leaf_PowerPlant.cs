using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A3 RID: 931
	public class SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant : SymbolResolver
	{
		// Token: 0x04000A11 RID: 2577
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x04000A12 RID: 2578
		private const float MaxCoverage = 0.09f;

		// Token: 0x06001030 RID: 4144 RVA: 0x00088500 File Offset: 0x00086900
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

		// Token: 0x06001031 RID: 4145 RVA: 0x00088610 File Offset: 0x00086A10
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

		// Token: 0x06001032 RID: 4146 RVA: 0x000886E8 File Offset: 0x00086AE8
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
	}
}
