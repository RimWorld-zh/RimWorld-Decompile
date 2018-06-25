using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D6 RID: 982
	public static class InteriorSymbolResolverUtility
	{
		// Token: 0x04000A47 RID: 2631
		private const float SpawnHeaterIfTemperatureBelow = 3f;

		// Token: 0x04000A48 RID: 2632
		private const float SpawnSecondHeaterIfTemperatureBelow = -45f;

		// Token: 0x04000A49 RID: 2633
		private const float NonIndustrial_SpawnCampfireIfTemperatureBelow = -20f;

		// Token: 0x04000A4A RID: 2634
		private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;

		// Token: 0x060010E6 RID: 4326 RVA: 0x00090304 File Offset: 0x0008E704
		public static void PushBedroomHeatersCoolersAndLightSourcesSymbols(ResolveParams rp, bool hasToSpawnLightSource = true)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > 22f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams);
			}
			bool flag = false;
			if (map.mapTemperature.OutdoorTemp < 3f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ((map.mapTemperature.OutdoorTemp >= -20f) ? ThingDefOf.TorchLamp : ThingDefOf.Campfire);
					flag = true;
				}
				int num = (map.mapTemperature.OutdoorTemp >= -45f) ? 1 : 2;
				for (int i = 0; i < num; i++)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = singleThingDef;
					BaseGen.symbolStack.Push("edgeThing", resolveParams2);
				}
			}
			if (!flag && hasToSpawnLightSource)
			{
				BaseGen.symbolStack.Push("indoorLighting", rp);
			}
		}
	}
}
