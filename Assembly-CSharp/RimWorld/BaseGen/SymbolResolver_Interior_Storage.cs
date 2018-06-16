using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DB RID: 987
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x00090610 File Offset: 0x0008EA10
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			BaseGen.symbolStack.Push("stockpile", rp);
			if (map.mapTemperature.OutdoorTemp > 15f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams);
			}
		}

		// Token: 0x04000A4C RID: 2636
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
