using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_HiveRelatedData : LordToilData
	{
		public Dictionary<Pawn, Hive> assignedHives = new Dictionary<Pawn, Hive>();

		[CompilerGenerated]
		private static Predicate<KeyValuePair<Pawn, Hive>> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<KeyValuePair<Pawn, Hive>> <>f__am$cache1;

		public LordToil_HiveRelatedData()
		{
		}

		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, Hive>(ref this.assignedHives, "assignedHives", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null);
			}
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(KeyValuePair<Pawn, Hive> x)
		{
			return x.Key.Destroyed;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(KeyValuePair<Pawn, Hive> x)
		{
			return x.Value == null;
		}
	}
}
