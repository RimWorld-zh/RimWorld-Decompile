using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000673 RID: 1651
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x0400138E RID: 5006
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();

		// Token: 0x060022A6 RID: 8870 RVA: 0x0012AE3C File Offset: 0x0012923C
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

		// Token: 0x060022A7 RID: 8871 RVA: 0x0012AE80 File Offset: 0x00129280
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}
	}
}
