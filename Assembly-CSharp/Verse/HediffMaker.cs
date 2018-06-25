using System;

namespace Verse
{
	// Token: 0x02000D24 RID: 3364
	public static class HediffMaker
	{
		// Token: 0x06004A18 RID: 18968 RVA: 0x0026BEA0 File Offset: 0x0026A2A0
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

		// Token: 0x06004A19 RID: 18969 RVA: 0x0026BF18 File Offset: 0x0026A318
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
