using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A5 RID: 933
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x0008874C File Offset: 0x00086B4C
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit = rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(skipSingleThingIfHasToWipeBuildingOrDoesntFit == null || skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value);
			BaseGen.symbolStack.Push("thing", resolveParams);
		}
	}
}
