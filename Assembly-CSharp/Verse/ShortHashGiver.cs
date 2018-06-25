using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class ShortHashGiver
	{
		private static Dictionary<Type, HashSet<ushort>> takenHashesPerDeftype = new Dictionary<Type, HashSet<ushort>>();

		[CompilerGenerated]
		private static Func<Def, string> <>f__am$cache0;

		public static void GiveAllShortHashes()
		{
			ShortHashGiver.takenHashesPerDeftype.Clear();
			List<Def> list = new List<Def>();
			foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
			{
				Type type2 = typeof(DefDatabase<>).MakeGenericType(new Type[]
				{
					type
				});
				PropertyInfo property = type2.GetProperty("AllDefs");
				MethodInfo getMethod = property.GetGetMethod();
				IEnumerable enumerable = (IEnumerable)getMethod.Invoke(null, null);
				list.Clear();
				IEnumerator enumerator2 = enumerable.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj = enumerator2.Current;
						Def item = (Def)obj;
						list.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				list.SortBy((Def d) => d.defName);
				for (int i = 0; i < list.Count; i++)
				{
					ShortHashGiver.GiveShortHash(list[i], type);
				}
			}
		}

		private static void GiveShortHash(Def def, Type defType)
		{
			if (def.shortHash != 0)
			{
				Log.Error(def + " already has short hash.", false);
			}
			else
			{
				HashSet<ushort> hashSet;
				if (!ShortHashGiver.takenHashesPerDeftype.TryGetValue(defType, out hashSet))
				{
					hashSet = new HashSet<ushort>();
					ShortHashGiver.takenHashesPerDeftype.Add(defType, hashSet);
				}
				ushort num = (ushort)(GenText.StableStringHash(def.defName) % 65535);
				int num2 = 0;
				while (num == 0 || hashSet.Contains(num))
				{
					num += 1;
					num2++;
					if (num2 > 5000)
					{
						Log.Message("Short hashes are saturated. There are probably too many Defs.", false);
					}
				}
				def.shortHash = num;
				hashSet.Add(num);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ShortHashGiver()
		{
		}

		[CompilerGenerated]
		private static string <GiveAllShortHashes>m__0(Def d)
		{
			return d.defName;
		}
	}
}
