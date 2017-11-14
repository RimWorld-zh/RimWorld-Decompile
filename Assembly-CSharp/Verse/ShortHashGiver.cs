using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Verse
{
	public static class ShortHashGiver
	{
		private static Dictionary<Type, HashSet<ushort>> takenHashesPerDeftype = new Dictionary<Type, HashSet<ushort>>();

		public static void GiveAllShortHashes()
		{
			ShortHashGiver.takenHashesPerDeftype.Clear();
			List<Def> list = new List<Def>();
			foreach (Type item2 in GenDefDatabase.AllDefTypesWithDatabases())
			{
				Type type = typeof(DefDatabase<>).MakeGenericType(item2);
				PropertyInfo property = type.GetProperty("AllDefs");
				MethodInfo getMethod = property.GetGetMethod();
				IEnumerable enumerable = (IEnumerable)getMethod.Invoke(null, null);
				list.Clear();
				IEnumerator enumerator2 = enumerable.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Def item = (Def)enumerator2.Current;
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
					ShortHashGiver.GiveShortHash(list[i], item2);
				}
			}
		}

		private static void GiveShortHash(Def def, Type defType)
		{
			if (def.shortHash != 0)
			{
				Log.Error(def + " already has short hash.");
			}
			else
			{
				HashSet<ushort> hashSet = default(HashSet<ushort>);
				if (!ShortHashGiver.takenHashesPerDeftype.TryGetValue(defType, out hashSet))
				{
					hashSet = new HashSet<ushort>();
					ShortHashGiver.takenHashesPerDeftype.Add(defType, hashSet);
				}
				ushort num = (ushort)(GenText.StableStringHash(def.defName) % 65535);
				int num2 = 0;
				while (true)
				{
					if (num != 0 && !hashSet.Contains(num))
						break;
					num = (ushort)(num + 1);
					num2++;
					if (num2 > 5000)
					{
						Log.Message("Short hashes are saturated. There are probably too many Defs.");
					}
				}
				def.shortHash = num;
				hashSet.Add(num);
			}
		}
	}
}
