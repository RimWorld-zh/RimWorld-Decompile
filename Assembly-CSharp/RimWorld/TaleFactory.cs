using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class TaleFactory
	{
		[CompilerGenerated]
		private static Func<object, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<TaleDef, bool> <>f__am$cache1;

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

		[CompilerGenerated]
		private static string <MakeRawTale>m__0(object arg)
		{
			return arg.ToStringSafe<object>();
		}

		[CompilerGenerated]
		private static bool <MakeRandomTestTale>m__1(TaleDef d)
		{
			return d.usableForArt;
		}
	}
}
