using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DD RID: 989
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x04000A4E RID: 2638
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;

		// Token: 0x060010F7 RID: 4343 RVA: 0x0009094C File Offset: 0x0008ED4C
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
	}
}
