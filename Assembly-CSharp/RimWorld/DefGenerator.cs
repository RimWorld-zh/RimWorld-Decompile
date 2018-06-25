using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class DefGenerator
	{
		public static void GenerateImpliedDefs_PreResolve()
		{
			IEnumerable<ThingDef> enumerable = ThingDefGenerator_Buildings.ImpliedBlueprintAndFrameDefs().Concat(ThingDefGenerator_Meat.ImpliedMeatDefs()).Concat(ThingDefGenerator_Corpses.ImpliedCorpseDefs());
			foreach (ThingDef def in enumerable)
			{
				DefGenerator.AddImpliedDef<ThingDef>(def);
			}
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			foreach (TerrainDef def2 in TerrainDefGenerator_Stone.ImpliedTerrainDefs())
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def2);
			}
			foreach (RecipeDef def3 in RecipeDefGenerator.ImpliedRecipeDefs())
			{
				DefGenerator.AddImpliedDef<RecipeDef>(def3);
			}
			foreach (PawnColumnDef def4 in PawnColumnDefgenerator.ImpliedPawnColumnDefs())
			{
				DefGenerator.AddImpliedDef<PawnColumnDef>(def4);
			}
		}

		public static void GenerateImpliedDefs_PostResolve()
		{
			foreach (KeyBindingCategoryDef def in KeyBindingDefGenerator.ImpliedKeyBindingCategoryDefs())
			{
				DefGenerator.AddImpliedDef<KeyBindingCategoryDef>(def);
			}
			foreach (KeyBindingDef def2 in KeyBindingDefGenerator.ImpliedKeyBindingDefs())
			{
				DefGenerator.AddImpliedDef<KeyBindingDef>(def2);
			}
		}

		public static void AddImpliedDef<T>(T def) where T : Def, new()
		{
			def.generated = true;
			if (def.modContentPack == null)
			{
				Log.Error(string.Format("Added def {0}:{1} without an associated modContentPack", def.GetType(), def.defName), false);
			}
			else
			{
				def.modContentPack.AddImpliedDef(def);
			}
			def.PostLoad();
			DefDatabase<T>.Add(def);
		}
	}
}
