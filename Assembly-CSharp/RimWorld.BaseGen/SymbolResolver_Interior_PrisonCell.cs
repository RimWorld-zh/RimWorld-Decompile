using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		private static readonly FloatRange TotalNutritionRange = new FloatRange(4f, 8f);

		private const int FoodStockpileSize = 3;

		public override void Resolve(ResolveParams rp)
		{
			ItemCollectionGeneratorParams value = default(ItemCollectionGeneratorParams);
			value.techLevel = ((rp.faction == null) ? TechLevel.Spacer : rp.faction.def.techLevel);
			value.totalNutrition = SymbolResolver_Interior_PrisonCell.TotalNutritionRange.RandomInRange;
			value.minPreferability = FoodPreferability.RawBad;
			ResolveParams resolveParams = rp;
			resolveParams.itemCollectionGeneratorDef = ItemCollectionGeneratorDefOf.Food;
			resolveParams.itemCollectionGeneratorParams = value;
			resolveParams.innerStockpileSize = 3;
			BaseGen.symbolStack.Push("innerStockpile", resolveParams);
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
			BaseGen.symbolStack.Push("prisonerBed", rp);
		}
	}
}
