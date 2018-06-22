using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000233 RID: 563
	public static class DefGenerator
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x0005A06C File Offset: 0x0005846C
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

		// Token: 0x06000A34 RID: 2612 RVA: 0x0005A1DC File Offset: 0x000585DC
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

		// Token: 0x06000A35 RID: 2613 RVA: 0x0005A284 File Offset: 0x00058684
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
