using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DC RID: 988
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		// Token: 0x04000A50 RID: 2640
		private const int FoodStockpileSize = 3;

		// Token: 0x060010F4 RID: 4340 RVA: 0x000908BC File Offset: 0x0008ECBC
		public override void Resolve(ResolveParams rp)
		{
			ThingSetMakerParams value = default(ThingSetMakerParams);
			value.techLevel = new TechLevel?((rp.faction == null) ? TechLevel.Spacer : rp.faction.def.techLevel);
			ResolveParams resolveParams = rp;
			resolveParams.thingSetMakerDef = ThingSetMakerDefOf.MapGen_PrisonCellStockpile;
			resolveParams.thingSetMakerParams = new ThingSetMakerParams?(value);
			resolveParams.innerStockpileSize = new int?(3);
			BaseGen.symbolStack.Push("innerStockpile", resolveParams);
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
			BaseGen.symbolStack.Push("prisonerBed", rp);
		}
	}
}
