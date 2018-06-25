using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public static class GenCollection
	{
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			return source.Any((T item) => other.Contains(item));
		}

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

		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy(selector, Comparer<TKey>.Default);
		}

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

		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

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

		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

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

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy(selector, Comparer<TKey>.Default);
		}

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

		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

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

		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			if (list.Count > 1)
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
		}

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

		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

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

		public static V TryGetValue<T, V>(this IDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (!dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

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

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			foreach (T t in lhs)
			{
				yield return t;
			}
			yield return rhs;
			yield break;
		}

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

		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return list[Rand.Range(0, list.Count)];
			}
			yield break;
		}

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

		private static class SortStableTempList<T>
		{
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			public static bool working;

			// Note: this type is marked as 'beforefieldinit'.
			static SortStableTempList()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <SharesElementWith>c__AnonStorey5<T>
		{
			internal IEnumerable<T> other;

			public <SharesElementWith>c__AnonStorey5()
			{
			}

			internal bool <>m__0(T item)
			{
				return this.other.Contains(item);
			}
		}

		[CompilerGenerated]
		private sealed class <InRandomOrder>c__Iterator0<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal IEnumerable<T> source;

			internal IList<T> workingList;

			internal int <countUnChosen>__0;

			internal int <rand>__0;

			internal T <temp>__1;

			internal T $current;

			internal bool $disposing;

			internal IList<T> <$>workingList;

			internal int $PC;

			[DebuggerHidden]
			public <InRandomOrder>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
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
					countUnChosen = workingList.Count;
					rand = 0;
					break;
				case 1u:
					temp = workingList[rand];
					workingList[rand] = workingList[countUnChosen - 1];
					workingList[countUnChosen - 1] = temp;
					countUnChosen--;
					break;
				default:
					return false;
				}
				if (countUnChosen > 0)
				{
					rand = Rand.Range(0, countUnChosen);
					this.$current = workingList[rand];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenCollection.<InRandomOrder>c__Iterator0<T> <InRandomOrder>c__Iterator = new GenCollection.<InRandomOrder>c__Iterator0<T>();
				<InRandomOrder>c__Iterator.source = source;
				<InRandomOrder>c__Iterator.workingList = workingList;
				return <InRandomOrder>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SortBy>c__AnonStorey6<T, TSortBy> where TSortBy : IComparable<TSortBy>
		{
			internal Func<T, TSortBy> selector;

			public <SortBy>c__AnonStorey6()
			{
			}

			internal int <>m__0(T a, T b)
			{
				TSortBy tsortBy = this.selector(a);
				return tsortBy.CompareTo(this.selector(b));
			}
		}

		[CompilerGenerated]
		private sealed class <SortBy>c__AnonStorey7<T, TSortBy, TThenBy> where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			internal Func<T, TSortBy> selector;

			internal Func<T, TThenBy> thenBySelector;

			public <SortBy>c__AnonStorey7()
			{
			}

			internal int <>m__0(T a, T b)
			{
				TSortBy tsortBy = this.selector(a);
				TSortBy other = this.selector(b);
				int result;
				if (!tsortBy.Equals(other))
				{
					result = tsortBy.CompareTo(other);
				}
				else
				{
					TThenBy tthenBy = this.thenBySelector(a);
					result = tthenBy.CompareTo(this.thenBySelector(b));
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <SortByDescending>c__AnonStorey8<T, TSortByDescending> where TSortByDescending : IComparable<TSortByDescending>
		{
			internal Func<T, TSortByDescending> selector;

			public <SortByDescending>c__AnonStorey8()
			{
			}

			internal int <>m__0(T a, T b)
			{
				TSortByDescending tsortByDescending = this.selector(b);
				return tsortByDescending.CompareTo(this.selector(a));
			}
		}

		[CompilerGenerated]
		private sealed class <SortByDescending>c__AnonStorey9<T, TSortByDescending, TThenByDescending> where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			internal Func<T, TSortByDescending> selector;

			internal Func<T, TThenByDescending> thenByDescendingSelector;

			public <SortByDescending>c__AnonStorey9()
			{
			}

			internal int <>m__0(T a, T b)
			{
				TSortByDescending other = this.selector(a);
				TSortByDescending other2 = this.selector(b);
				int result;
				if (!other.Equals(other2))
				{
					result = other2.CompareTo(other);
				}
				else
				{
					TThenByDescending tthenByDescending = this.thenByDescendingSelector(b);
					result = tthenByDescending.CompareTo(this.thenByDescendingSelector(a));
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <SortStable>c__AnonStoreyA<T>
		{
			internal Func<T, T, int> comparator;

			public <SortStable>c__AnonStoreyA()
			{
			}

			internal int <>m__0(Pair<T, int> lhs, Pair<T, int> rhs)
			{
				int num = this.comparator(lhs.First, rhs.First);
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
			}
		}

		[CompilerGenerated]
		private sealed class <Cross>c__Iterator1<T, V> : IEnumerable, IEnumerable<Pair<T, V>>, IEnumerator, IDisposable, IEnumerator<Pair<T, V>>
		{
			internal IEnumerable<T> lhs;

			internal T[] <lhsv>__0;

			internal IEnumerable<V> rhs;

			internal V[] <rhsv>__0;

			internal int <i>__1;

			internal int <j>__2;

			internal Pair<T, V> $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <Cross>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					lhsv = lhs.ToArray<T>();
					rhsv = rhs.ToArray<V>();
					i = 0;
					goto IL_D0;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_AE:
				if (j < rhsv.Length)
				{
					this.$current = new Pair<T, V>(lhsv[i], rhsv[j]);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_D0:
				if (i < lhsv.Length)
				{
					j = 0;
					goto IL_AE;
				}
				this.$PC = -1;
				return false;
			}

			Pair<T, V> IEnumerator<Pair<T, V>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pair<T,V>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pair<T, V>> IEnumerable<Pair<T, V>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenCollection.<Cross>c__Iterator1<T, V> <Cross>c__Iterator = new GenCollection.<Cross>c__Iterator1<T, V>();
				<Cross>c__Iterator.lhs = lhs;
				<Cross>c__Iterator.rhs = rhs;
				return <Cross>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <Concat>c__Iterator2<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal IEnumerable<T> lhs;

			internal IEnumerator<T> $locvar0;

			internal T <t>__1;

			internal T rhs;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <Concat>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = lhs.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						this.$current = t;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$current = rhs;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenCollection.<Concat>c__Iterator2<T> <Concat>c__Iterator = new GenCollection.<Concat>c__Iterator2<T>();
				<Concat>c__Iterator.lhs = lhs;
				<Concat>c__Iterator.rhs = rhs;
				return <Concat>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <Except>c__Iterator3<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T> where T : class
		{
			internal IEnumerable<T> lhs;

			internal IEnumerator<T> $locvar0;

			internal T <t>__1;

			internal T rhs;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <Except>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = lhs.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_99:
						break;
					}
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						if (t != rhs)
						{
							this.$current = t;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_99;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenCollection.<Except>c__Iterator3<T> <Except>c__Iterator = new GenCollection.<Except>c__Iterator3<T>();
				<Except>c__Iterator.lhs = lhs;
				<Except>c__Iterator.rhs = rhs;
				return <Except>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <TakeRandom>c__Iterator4<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal int <i>__1;

			internal int count;

			internal List<T> list;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <TakeRandom>c__Iterator4()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < count)
				{
					this.$current = list[Rand.Range(0, list.Count)];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenCollection.<TakeRandom>c__Iterator4<T> <TakeRandom>c__Iterator = new GenCollection.<TakeRandom>c__Iterator4<T>();
				<TakeRandom>c__Iterator.count = count;
				<TakeRandom>c__Iterator.list = list;
				return <TakeRandom>c__Iterator;
			}
		}
	}
}
