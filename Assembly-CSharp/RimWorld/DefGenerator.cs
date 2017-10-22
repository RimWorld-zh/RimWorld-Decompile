using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class DefGenerator
	{
		public static void GenerateImpliedDefs_PreResolve()
		{
			IEnumerable<ThingDef> enumerable = ThingDefGenerator_Buildings.ImpliedBlueprintAndFrameDefs().Concat(ThingDefGenerator_Meat.ImpliedMeatDefs()).Concat(ThingDefGenerator_Corpses.ImpliedCorpseDefs()).Concat(ThingDefGenerator_Leather.ImpliedLeatherDefs());
			foreach (ThingDef item in enumerable)
			{
				item.PostLoad();
				DefDatabase<ThingDef>.Add(item);
			}
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			foreach (TerrainDef item2 in TerrainDefGenerator_Stone.ImpliedTerrainDefs())
			{
				item2.PostLoad();
				DefDatabase<TerrainDef>.Add(item2);
			}
			foreach (RecipeDef item3 in RecipeDefGenerator.ImpliedRecipeDefs())
			{
				item3.PostLoad();
				DefDatabase<RecipeDef>.Add(item3);
			}
			foreach (PawnColumnDef item4 in PawnColumnDefgenerator.ImpliedPawnColumnDefs())
			{
				item4.PostLoad();
				DefDatabase<PawnColumnDef>.Add(item4);
			}
		}

		public static void GenerateImpliedDefs_PostResolve()
		{
			foreach (KeyBindingCategoryDef item in KeyBindingDefGenerator.ImpliedKeyBindingCategoryDefs())
			{
				item.PostLoad();
				DefDatabase<KeyBindingCategoryDef>.Add(item);
			}
			foreach (KeyBindingDef item2 in KeyBindingDefGenerator.ImpliedKeyBindingDefs())
			{
				item2.PostLoad();
				DefDatabase<KeyBindingDef>.Add(item2);
			}
		}
	}
}
