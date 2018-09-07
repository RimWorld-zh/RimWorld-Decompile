using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class TaleFactory
	{
		[CompilerGenerated]
		private static Func<object, string> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<TaleDef, bool> <>f__am$cache0;

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
				string format = "Failed to create tale object {0} with parameters {1}: {2}";
				if (TaleFactory.<>f__mg$cache0 == null)
				{
					TaleFactory.<>f__mg$cache0 = new Func<object, string>(Gen.ToStringSafe<object>);
				}
				Log.Error(string.Format(format, def, args.Select(TaleFactory.<>f__mg$cache0).ToCommaList(false), arg), false);
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
		private static bool <MakeRandomTestTale>m__0(TaleDef d)
		{
			return d.usableForArt;
		}
	}
}
