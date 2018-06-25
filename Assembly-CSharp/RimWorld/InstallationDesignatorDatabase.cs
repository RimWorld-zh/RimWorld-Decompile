using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000673 RID: 1651
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x04001392 RID: 5010
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();

		// Token: 0x060022A5 RID: 8869 RVA: 0x0012B0A4 File Offset: 0x001294A4
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

		// Token: 0x060022A6 RID: 8870 RVA: 0x0012B0E8 File Offset: 0x001294E8
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}
	}
}
