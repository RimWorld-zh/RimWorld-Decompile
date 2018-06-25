using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class NativeVerbPropertiesDatabase
	{
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();

		static NativeVerbPropertiesDatabase()
		{
		}

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

		[CompilerGenerated]
		private sealed class <VerbWithCategory>c__AnonStorey0
		{
			internal VerbCategory id;

			public <VerbWithCategory>c__AnonStorey0()
			{
			}

			internal bool <>m__0(VerbProperties v)
			{
				return v.category == this.id;
			}
		}
	}
}
