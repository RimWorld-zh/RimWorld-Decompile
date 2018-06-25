using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class FactionBaseNameGenerator
	{
		private static List<string> usedNames = new List<string>();

		public static string GenerateFactionBaseName(FactionBase factionBase, RulePackDef rulePack = null)
		{
			if (rulePack == null)
			{
				if (factionBase.Faction == null || factionBase.Faction.def.settlementNameMaker == null)
				{
					return factionBase.def.label;
				}
				rulePack = factionBase.Faction.def.settlementNameMaker;
			}
			FactionBaseNameGenerator.usedNames.Clear();
			List<FactionBase> factionBases = Find.WorldObjects.FactionBases;
			for (int i = 0; i < factionBases.Count; i++)
			{
				FactionBase factionBase2 = factionBases[i];
				if (factionBase2.Name != null)
				{
					FactionBaseNameGenerator.usedNames.Add(factionBase2.Name);
				}
			}
			return NameGenerator.GenerateName(rulePack, FactionBaseNameGenerator.usedNames, true, null);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FactionBaseNameGenerator()
		{
		}
	}
}
