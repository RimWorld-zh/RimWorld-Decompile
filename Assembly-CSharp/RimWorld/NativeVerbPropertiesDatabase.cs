using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000992 RID: 2450
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x0600370F RID: 14095 RVA: 0x001D6608 File Offset: 0x001D4A08
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

		// Token: 0x04002382 RID: 9090
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();
	}
}
