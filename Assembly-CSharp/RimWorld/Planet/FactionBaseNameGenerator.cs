using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FE RID: 1534
	public static class FactionBaseNameGenerator
	{
		// Token: 0x04001217 RID: 4631
		private static List<string> usedNames = new List<string>();

		// Token: 0x06001E8C RID: 7820 RVA: 0x0010B854 File Offset: 0x00109C54
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
