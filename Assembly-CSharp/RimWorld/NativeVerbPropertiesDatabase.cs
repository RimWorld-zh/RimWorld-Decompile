using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098E RID: 2446
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x06003708 RID: 14088 RVA: 0x001D6804 File Offset: 0x001D4C04
		public static VerbProperties VerbWithCategory(VerbCategory id)
		{
			VerbProperties verbProperties = (from v in NativeVerbPropertiesDatabase.allVerbDefs
			where v.category == id
			select v).FirstOrDefault<VerbProperties>();
			if (verbProperties == null)
			{
				Log.Error("Failed to find Verb with id " + id, false);
			}
			return verbProperties;
		}

		// Token: 0x04002380 RID: 9088
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();
	}
}
