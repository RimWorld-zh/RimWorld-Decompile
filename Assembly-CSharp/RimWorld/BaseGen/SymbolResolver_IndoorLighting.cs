using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CC RID: 972
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x04000A39 RID: 2617
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;

		// Token: 0x060010BF RID: 4287 RVA: 0x0008E848 File Offset: 0x0008CC48
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else if (map.mapTemperature.OutdoorTemp > 18f)
			{
				thingDef = null;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			if (thingDef != null)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = thingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams);
			}
		}
	}
}
