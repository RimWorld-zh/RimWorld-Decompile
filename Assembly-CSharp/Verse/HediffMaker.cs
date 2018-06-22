using System;

namespace Verse
{
	// Token: 0x02000D21 RID: 3361
	public static class HediffMaker
	{
		// Token: 0x06004A15 RID: 18965 RVA: 0x0026BAE4 File Offset: 0x00269EE4
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

		// Token: 0x06004A16 RID: 18966 RVA: 0x0026BB5C File Offset: 0x00269F5C
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
