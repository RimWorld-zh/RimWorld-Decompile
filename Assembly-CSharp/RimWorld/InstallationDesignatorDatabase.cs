using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000675 RID: 1653
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x060022A8 RID: 8872 RVA: 0x0012AB2C File Offset: 0x00128F2C
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

		// Token: 0x060022A9 RID: 8873 RVA: 0x0012AB70 File Offset: 0x00128F70
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}

		// Token: 0x04001390 RID: 5008
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
