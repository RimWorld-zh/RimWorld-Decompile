using System;

namespace Verse
{
	public static class HediffMaker
	{
		public static Hediff MakeHediff(HediffDef def, Pawn pawn, BodyPartRecord partRecord = null)
		{
			Hediff result;
			if (pawn == null)
			{
				Log.Error("Cannot make hediff " + def + " for null pawn.", false);
				result = null;
			}
			else
			{
				Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
				hediff.def = def;
				hediff.pawn = pawn;
				hediff.Part = partRecord;
				hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
				hediff.PostMake();
				result = hediff;
			}
			return result;
		}

		public static Hediff MakeConcreteExampleHediff(HediffDef def)
		{
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}
	}
}
