using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000662 RID: 1634
	public static class TaleFactory
	{
		// Token: 0x06002219 RID: 8729 RVA: 0x00121500 File Offset: 0x0011F900
		public static Tale MakeRawTale(TaleDef def, params object[] args)
		{
			Tale result;
			try
			{
				Tale tale = (Tale)Activator.CreateInstance(def.taleClass, args);
				tale.def = def;
				tale.id = Find.UniqueIDsManager.GetNextTaleID();
				tale.date = Find.TickManager.TicksAbs;
				result = tale;
			}
			catch (Exception arg)
			{
				Exception arg2;
				Log.Error(string.Format("Failed to create tale object {0} with parameters {1}: {2}", def, (from arg in args
				select arg.ToStringSafe<object>()).ToCommaList(false), arg2), false);
				result = null;
			}
			return result;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x001215A4 File Offset: 0x0011F9A4
		public static Tale MakeRandomTestTale(TaleDef def = null)
		{
			if (def == null)
			{
				def = (from d in DefDatabase<TaleDef>.AllDefs
				where d.usableForArt
				select d).RandomElement<TaleDef>();
			}
			Tale tale = TaleFactory.MakeRawTale(def, new object[0]);
			tale.GenerateTestData();
			return tale;
		}
	}
}
