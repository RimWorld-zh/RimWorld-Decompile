using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Leather
	{
		private const float HumanlikeLeatherCommonalityFactor = 0.02f;

		private const float BaseLeatherCommonality = 1.2f;

		private static bool GeneratesLeather(ThingDef sourceDef)
		{
			return sourceDef.category == ThingCategory.Pawn && sourceDef.GetStatValueAbstract(StatDefOf.LeatherAmount, null) > 0f;
		}

		[DebuggerHidden]
		public static IEnumerable<ThingDef> ImpliedLeatherDefs()
		{
			ThingDefGenerator_Leather.<ImpliedLeatherDefs>c__Iterator74 <ImpliedLeatherDefs>c__Iterator = new ThingDefGenerator_Leather.<ImpliedLeatherDefs>c__Iterator74();
			ThingDefGenerator_Leather.<ImpliedLeatherDefs>c__Iterator74 expr_07 = <ImpliedLeatherDefs>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
