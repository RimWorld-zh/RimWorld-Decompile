using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000F32 RID: 3890
	public static class GenCollection
	{
		// Token: 0x06005D88 RID: 23944 RVA: 0x002F7B48 File Offset: 0x002F5F48
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			return source.Any((T item) => other.Contains(item));
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x002F7B7C File Offset: 0x002F5F7C
		public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (workingList == null)
			{
				workingList = source.ToList<T>();
			}
			else
			{
				workingList.Clear();
				foreach (T item in source)
				{
					workingList.Add(item);
				}
			}
			int countUnChosen = workingList.Count;
			int rand = 0;
			while (countUnChosen > 0)
			{
				rand = Rand.Range(0, countUnChosen);
				yield return workingList[rand];
				T temp = workingList[rand];
				workingList[rand] = workingList[countUnChosen - 1];
				workingList[countUnChosen - 1] = temp;
				countUnChosen--;
			}
			yield break;
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x002F7BB4 File Offset: 0x002F5FB4
		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list == null)
			{
				list = source.ToList<T>();
			}
			T result;
			if (list.Count == 0)
			{
				Log.Warning("Getting random element from empty collection.", false);
				result = default(T);
			}
			else
			{
				result = list[Rand.Range(0, list.Count)];
			}
			return result;
		}

		// Token: 0x06005D8B RID: 23947 RVA: 0x002F7C28 File Offset: 0x002F6028
		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T t;
			T result;
			if (source.TryRandomElement(out t))
			{
				result = t;
			}
			else
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06005D8C RID: 23948 RVA: 0x002F7C54 File Offset: 0x002F6054
		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				list = source.ToList<T>();
				if (!list.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			result = list.RandomElement<T>();
			return true;
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x002F7CE8 File Offset: 0x002F60E8
		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}), false);
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					return list[0];
				}
			}
			else
			{
				int num3 = 0;
				foreach (T t in source)
				{
					num3++;
					float num4 = weightSelector(t);
					if (num4 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num4,
							" from ",
							t
						}), false);
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0f)
				{
					return source.First<T>();
				}
			}
			T result;
			if (num <= 0f)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.", false);
				result = default(T);
			}
			else
			{
				float num5 = Rand.Value * num;
				float num6 = 0f;
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						float num7 = weightSelector(list[j]);
						if (num7 > 0f)
						{
							num6 += num7;
							if (num6 >= num5)
							{
								return list[j];
							}
						}
					}
				}
				else
				{
					foreach (T t2 in source)
					{
						float num8 = weightSelector(t2);
						if (num8 > 0f)
						{
							num6 += num8;
							if (num6 >= num5)
							{
								return t2;
							}
						}
					}
				}
				result = default(T);
			}
			return result;
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x002F7FBC File Offset: 0x002F63BC
		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T t;
			T result;
			if (source.TryRandomElementByWeight(weightSelector, out t))
			{
				result = t;
			}
			else
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x002F7FE8 File Offset: 0x002F63E8
		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num2,
							" from ",
							list[i]
						}), false);
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					result = list[0];
					return true;
				}
				if (num == 0f)
				{
					result = default(T);
					return false;
				}
				num *= Rand.Value;
				for (int j = 0; j < list.Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (num3 > 0f)
					{
						num -= num3;
						if (num <= 0f)
						{
							result = list[j];
							return true;
						}
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0f && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num4,
						" from ",
						result
					}), false);
					num4 = 0f;
				}
			}
			bool result2;
			if (num4 == 0f)
			{
				result = default(T);
				result2 = false;
			}
			else
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					float num5 = weightSelector(t);
					if (num5 < 0f)
					{
						Log.Error(string.Concat(new object[]
						{
							"Negative weight in selector: ",
							num5,
							" from ",
							t
						}), false);
						num5 = 0f;
					}
					if (Rand.Range(0f, num4 + num5) >= num4)
					{
						result = t;
					}
					num4 += num5;
				}
				result2 = true;
			}
			return result2;
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x002F82AC File Offset: 0x002F66AC
		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0f)
			{
				Log.Error("Negative default value weight.", false);
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T t in source)
			{
				float num2 = weightSelector(t);
				if (num2 < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"Negative weight in selector: ",
						num2,
						" from ",
						t
					}), false);
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			T result;
			if (num3 <= 0f)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3, false);
				result = default(T);
			}
			else if (Rand.Value < defaultValueWeight / num3 || num == 0f)
			{
				result = default(T);
			}
			else
			{
				result = source.RandomElementByWeight(weightSelector);
			}
			return result;
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x002F83E8 File Offset: 0x002F67E8
		public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return fallback;
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x002F8448 File Offset: 0x002F6848
		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x002F846C File Offset: 0x002F686C
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x002F8490 File Offset: 0x002F6890
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) > 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x002F8560 File Offset: 0x002F6960
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

		// Token: 0x06005D96 RID: 23958 RVA: 0x002F8584 File Offset: 0x002F6984
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = fallback;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					result = tsource;
				}
			}
			return result;
		}

		// Token: 0x06005D97 RID: 23959 RVA: 0x002F8650 File Offset: 0x002F6A50
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x002F8674 File Offset: 0x002F6A74
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x002F8758 File Offset: 0x002F6B58
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

		// Token: 0x06005D9A RID: 23962 RVA: 0x002F877C File Offset: 0x002F6B7C
		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x06005D9B RID: 23963 RVA: 0x002F884C File Offset: 0x002F6C4C
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x06005D9C RID: 23964 RVA: 0x002F8870 File Offset: 0x002F6C70
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) < 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x002F8954 File Offset: 0x002F6D54
		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count > 1)
			{
				list.Sort(delegate(T a, T b)
				{
					TSortBy tsortBy = selector(a);
					return tsortBy.CompareTo(selector(b));
				});
			}
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x002F8994 File Offset: 0x002F6D94
		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count > 1)
			{
				list.Sort(delegate(T a, T b)
				{
					TSortBy tsortBy = selector(a);
					TSortBy other = selector(b);
					int result;
					if (!tsortBy.Equals(other))
					{
						result = tsortBy.CompareTo(other);
					}
					else
					{
						TThenBy tthenBy = thenBySelector(a);
						result = tthenBy.CompareTo(thenBySelector(b));
					}
					return result;
				});
			}
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x002F89DC File Offset: 0x002F6DDC
		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count > 1)
			{
				list.Sort(delegate(T a, T b)
				{
					TSortByDescending tsortByDescending = selector(b);
					return tsortByDescending.CompareTo(selector(a));
				});
			}
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x002F8A1C File Offset: 0x002F6E1C
		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count > 1)
			{
				list.Sort(delegate(T a, T b)
				{
					TSortByDescending other = selector(a);
					TSortByDescending other2 = selector(b);
					int result;
					if (!other.Equals(other2))
					{
						result = other2.CompareTo(other);
					}
					else
					{
						TThenByDescending tthenByDescending = thenByDescendingSelector(b);
						result = tthenByDescending.CompareTo(thenByDescendingSelector(a));
					}
					return result;
				});
			}
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x002F8A64 File Offset: 0x002F6E64
		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			List<Pair<T, int>> list2;
			bool flag;
			if (GenCollection.SortStableTempList<T>.working)
			{
				list2 = new List<Pair<T, int>>();
				flag = false;
			}
			else
			{
				list2 = GenCollection.SortStableTempList<T>.list;
				GenCollection.SortStableTempList<T>.working = true;
				flag = true;
			}
			try
			{
				list2.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new Pair<T, int>(list[i], i));
				}
				list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
				{
					int num = comparator(lhs.First, rhs.First);
					int result;
					if (num != 0)
					{
						result = num;
					}
					else
					{
						result = lhs.Second.CompareTo(rhs.Second);
					}
					return result;
				});
				list.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].First);
				}
				list2.Clear();
			}
			finally
			{
				if (flag)
				{
					GenCollection.SortStableTempList<T>.working = false;
				}
			}
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x002F8B54 File Offset: 0x002F6F54
		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			int result;
			try
			{
				foreach (KeyValuePair<TKey, TValue> obj in dictionary)
				{
					if (predicate(obj))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(obj.Key);
					}
				}
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						dictionary.Remove(list[i]);
						i++;
					}
					result = list.Count;
				}
				else
				{
					result = 0;
				}
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
			return result;
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x002F8C40 File Offset: 0x002F7040
		public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
		{
			int num = 0;
			int count = list.Count;
			while (num < count && !predicate(list[num], num))
			{
				num++;
			}
			if (num < count)
			{
				int i = num + 1;
				while (i < count)
				{
					while (i < count && predicate(list[i], i))
					{
						i++;
					}
					if (i < count)
					{
						list[num++] = list[i++];
					}
				}
			}
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x002F8CDC File Offset: 0x002F70DC
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x002F8CF0 File Offset: 0x002F70F0
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x06005DA6 RID: 23974 RVA: 0x002F8D24 File Offset: 0x002F7124
		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x002F8D48 File Offset: 0x002F7148
		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x06005DA8 RID: 23976 RVA: 0x002F8D6C File Offset: 0x002F716C
		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		// Token: 0x06005DA9 RID: 23977 RVA: 0x002F8DA4 File Offset: 0x002F71A4
		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

		// Token: 0x06005DAA RID: 23978 RVA: 0x002F8E00 File Offset: 0x002F7200
		public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (T arg in list)
			{
				float num3 = weight(arg);
				num += num3;
				num2 += value(arg) * num3;
			}
			return num2 / num;
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x002F8E84 File Offset: 0x002F7284
		public static void ExecuteEnumerable(this IEnumerable enumerable)
		{
			IEnumerator enumerator = enumerable.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x002F8EDC File Offset: 0x002F72DC
		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				if (predicate(arg))
				{
					break;
				}
				num++;
			}
			return num;
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x002F8F4C File Offset: 0x002F734C
		public static V TryGetValue<T, V>(this IDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (!dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x002F8F74 File Offset: 0x002F7374
		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			T[] lhsv = lhs.ToArray<T>();
			V[] rhsv = rhs.ToArray<V>();
			for (int i = 0; i < lhsv.Length; i++)
			{
				for (int j = 0; j < rhsv.Length; j++)
				{
					yield return new Pair<T, V>(lhsv[i], rhsv[j]);
				}
			}
			yield break;
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x002F8FA8 File Offset: 0x002F73A8
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			foreach (T t in lhs)
			{
				yield return t;
			}
			yield return rhs;
			yield break;
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x002F8FDC File Offset: 0x002F73DC
		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			LocalTargetInfo invalid;
			if (source == null)
			{
				invalid = LocalTargetInfo.Invalid;
			}
			else
			{
				for (int i = 0; i < source.Count; i++)
				{
					if (source[i].IsValid)
					{
						return source[i];
					}
				}
				invalid = LocalTargetInfo.Invalid;
			}
			return invalid;
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x002F9044 File Offset: 0x002F7444
		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			foreach (T t in lhs)
			{
				if (t != rhs)
				{
					yield return t;
				}
			}
			yield break;
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x002F9078 File Offset: 0x002F7478
		public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
		{
			bool result;
			if (a == b)
			{
				result = true;
			}
			else if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				result = true;
			}
			else if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				result = false;
			}
			else if (a.Count != b.Count)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < a.Count; i++)
				{
					if (a[i] != b[i])
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x002F912C File Offset: 0x002F752C
		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return list[Rand.Range(0, list.Count)];
			}
			yield break;
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x002F9160 File Offset: 0x002F7560
		public static void AddDistinct<T>(this List<T> list, T element) where T : class
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == element)
				{
					return;
				}
			}
			list.Add(element);
		}

		// Token: 0x02000F33 RID: 3891
		private static class SortStableTempList<T>
		{
			// Token: 0x04003DBC RID: 15804
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			// Token: 0x04003DBD RID: 15805
			public static bool working;
		}
	}
}
