using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000990 RID: 2448
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x04002381 RID: 9089
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();

		// Token: 0x0600370C RID: 14092 RVA: 0x001D6944 File Offset: 0x001D4D44
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
	}
}
