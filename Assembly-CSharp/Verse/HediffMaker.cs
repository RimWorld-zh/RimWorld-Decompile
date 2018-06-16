using System;

namespace Verse
{
	// Token: 0x02000D25 RID: 3365
	public static class HediffMaker
	{
		// Token: 0x06004A06 RID: 18950 RVA: 0x0026A6D8 File Offset: 0x00268AD8
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

		// Token: 0x06004A07 RID: 18951 RVA: 0x0026A750 File Offset: 0x00268B50
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
