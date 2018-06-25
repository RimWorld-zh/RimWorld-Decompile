using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DA RID: 986
	public class SymbolResolver_Interior_Brewery : SymbolResolver
	{
		// Token: 0x04000A4C RID: 2636
		private const float SpawnHeaterIfTemperatureBelow = 7f;

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x000906A4 File Offset: 0x0008EAA4
		private float SpawnPassiveCoolerIfTemperatureAbove
		{
			get
			{
				return ThingDefOf.FermentingBarrel.GetCompProperties<CompProperties_TemperatureRuinable>().maxSafeTemperature;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000906C8 File Offset: 0x0008EAC8
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > this.SpawnPassiveCoolerIfTemperatureAbove)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams);
			}
			if (map.mapTemperature.OutdoorTemp < 7f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ThingDefOf.Campfire;
				}
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = singleThingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams2);
			}
			BaseGen.symbolStack.Push("addWortToFermentingBarrels", rp);
			ResolveParams resolveParams3 = rp;
			resolveParams3.singleThingDef = ThingDefOf.FermentingBarrel;
			resolveParams3.thingRot = new Rot4?((!Rand.Bool) ? Rot4.East : Rot4.North);
			int? fillWithThingsPadding = rp.fillWithThingsPadding;
			resolveParams3.fillWithThingsPadding = new int?((fillWithThingsPadding == null) ? 1 : fillWithThingsPadding.Value);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams3);
		}
	}
}
