using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CA RID: 970
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x060010BC RID: 4284 RVA: 0x0008E6E8 File Offset: 0x0008CAE8
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

		// Token: 0x04000A36 RID: 2614
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;
	}
}
