using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CC RID: 972
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x04000A36 RID: 2614
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;

		// Token: 0x060010C0 RID: 4288 RVA: 0x0008E838 File Offset: 0x0008CC38
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
