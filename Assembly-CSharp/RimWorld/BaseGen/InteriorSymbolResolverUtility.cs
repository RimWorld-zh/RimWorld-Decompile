using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D4 RID: 980
	public static class InteriorSymbolResolverUtility
	{
		// Token: 0x04000A44 RID: 2628
		private const float SpawnHeaterIfTemperatureBelow = 3f;

		// Token: 0x04000A45 RID: 2629
		private const float SpawnSecondHeaterIfTemperatureBelow = -45f;

		// Token: 0x04000A46 RID: 2630
		private const float NonIndustrial_SpawnCampfireIfTemperatureBelow = -20f;

		// Token: 0x04000A47 RID: 2631
		private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;

		// Token: 0x060010E3 RID: 4323 RVA: 0x000901A4 File Offset: 0x0008E5A4
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
