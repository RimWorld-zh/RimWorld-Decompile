using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000671 RID: 1649
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x0012ACEC File Offset: 0x001290EC
		public static Designator_Install DesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			Designator_Install result;
			if (InstallationDesignatorDatabase.designators.TryGetValue(artDef, out designator_Install))
			{
				result = designator_Install;
			}
			else
			{
				designator_Install = InstallationDesignatorDatabase.NewDesignatorFor(artDef);
				InstallationDesignatorDatabase.designators.Add(artDef, designator_Install);
				result = designator_Install;
			}
			return result;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0012AD30 File Offset: 0x00129130
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}

		// Token: 0x0400138E RID: 5006
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
