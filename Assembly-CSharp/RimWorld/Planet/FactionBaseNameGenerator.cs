using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000602 RID: 1538
	public static class FactionBaseNameGenerator
	{
		// Token: 0x06001E95 RID: 7829 RVA: 0x0010B80C File Offset: 0x00109C0C
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

		// Token: 0x0400121A RID: 4634
		private static List<string> usedNames = new List<string>();
	}
}
