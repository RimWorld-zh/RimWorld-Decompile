using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000600 RID: 1536
	public static class FactionBaseNameGenerator
	{
		// Token: 0x0400121B RID: 4635
		private static List<string> usedNames = new List<string>();

		// Token: 0x06001E8F RID: 7823 RVA: 0x0010BC0C File Offset: 0x0010A00C
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
	}
}
