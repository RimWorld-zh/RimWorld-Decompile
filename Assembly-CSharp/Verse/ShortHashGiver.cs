using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000AF8 RID: 2808
	public static class ShortHashGiver
	{
		// Token: 0x06003E2A RID: 15914 RVA: 0x0020C5D8 File Offset: 0x0020A9D8
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

		// Token: 0x06003E2B RID: 15915 RVA: 0x0020C744 File Offset: 0x0020AB44
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

		// Token: 0x0400274C RID: 10060
		private static Dictionary<Type, HashSet<ushort>> takenHashesPerDeftype = new Dictionary<Type, HashSet<ushort>>();
	}
}
