using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class ExpectationsUtility
	{
		private static List<ExpectationDef> expectationsInOrder;

		[CompilerGenerated]
		private static Func<ExpectationDef, float> <>f__am$cache0;

		public static ExpectationDef CurrentExpectationFor(Pawn p)
		{
			ExpectationDef result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = null;
			}
			else if (p.Faction != Faction.OfPlayer)
			{
				result = ExpectationDefOf.ExtremelyLow;
			}
			else if (p.MapHeld != null)
			{
				result = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
			}
			else
			{
				result = ExpectationDefOf.VeryLow;
			}
			return result;
		}

		public static ExpectationDef CurrentExpectationFor(Map m)
		{
			if (ExpectationsUtility.expectationsInOrder == null)
			{
				ExpectationsUtility.expectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
				orderby ed.maxMapWealth
				select ed).ToList<ExpectationDef>();
			}
			float wealthTotal = m.wealthWatcher.WealthTotal;
			for (int i = 0; i < ExpectationsUtility.expectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.expectationsInOrder[i];
				if (wealthTotal < expectationDef.maxMapWealth)
				{
					return expectationDef;
				}
			}
			return ExpectationsUtility.expectationsInOrder[ExpectationsUtility.expectationsInOrder.Count - 1];
		}

		[CompilerGenerated]
		private static float <CurrentExpectationFor>m__0(ExpectationDef ed)
		{
			return ed.maxMapWealth;
		}
	}
}
