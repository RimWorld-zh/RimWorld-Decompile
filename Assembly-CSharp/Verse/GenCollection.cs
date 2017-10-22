using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class GenCollection
	{
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			return source.Any<T>((Func<T, bool>)((T item) => ((IEnumerable<T>)other).Contains<T>(item)));
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
				((ICollection<T>)workingList).Clear();
				foreach (T item in source)
				{
					((ICollection<T>)workingList).Add(item);
				}
			}
			int countUnChosen = ((ICollection<T>)workingList).Count;
			int rand2 = 0;
			if (countUnChosen > 0)
			{
				rand2 = Rand.Range(0, countUnChosen);
				yield return workingList[rand2];
				/*Error: Unable to find new state assignment for yield return*/;
			}
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
			if (((ICollection<T>)list).Count == 0)
			{
				Log.Warning("Getting random element from empty collection.");
				result = default(T);
			}
			else
			{
				result = list[Rand.Range(0, ((ICollection<T>)list).Count)];
			}
			return result;
		}

		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T val = default(T);
			return (!source.TryRandomElement<T>(out val)) ? fallback : val;
		}

		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			bool result2;
			if (list != null)
			{
				if (((ICollection<T>)list).Count == 0)
				{
					result = default(T);
					result2 = false;
					goto IL_007f;
				}
			}
			else if (!source.Any<T>())
			{
				result = default(T);
				result2 = false;
				goto IL_007f;
			}
			result = source.RandomElement<T>();
			result2 = true;
			goto IL_007f;
			IL_007f:
			return result2;
		}

		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			T result;
			if (list != null)
			{
				for (int i = 0; i < ((ICollection<T>)list).Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0.0)
					{
						Log.Error("Negative weight in selector: " + num2 + " from " + list[i]);
						num2 = 0f;
					}
					num += num2;
				}
				if (((ICollection<T>)list).Count == 1 && num > 0.0)
				{
					result = list[0];
					goto IL_02a3;
				}
			}
			else
			{
				int num3 = 0;
				foreach (T item in source)
				{
					num3++;
					float num4 = weightSelector(item);
					if (num4 < 0.0)
					{
						Log.Error("Negative weight in selector: " + num4 + " from " + item);
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0.0)
				{
					result = source.First<T>();
					goto IL_02a3;
				}
			}
			int j;
			if (num <= 0.0)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.");
				result = default(T);
			}
			else
			{
				float num5 = Rand.Value * num;
				float num6 = 0f;
				if (list != null)
				{
					for (j = 0; j < ((ICollection<T>)list).Count; j++)
					{
						float num7 = weightSelector(list[j]);
						if (!(num7 <= 0.0))
						{
							num6 += num7;
							if (num6 >= num5)
								goto IL_01fa;
						}
					}
				}
				else
				{
					foreach (T item2 in source)
					{
						float num8 = weightSelector(item2);
						if (!(num8 <= 0.0))
						{
							num6 += num8;
							if (num6 >= num5)
							{
								return item2;
							}
						}
					}
				}
				result = default(T);
			}
			goto IL_02a3;
			IL_01fa:
			result = list[j];
			goto IL_02a3;
			IL_02a3:
			return result;
		}

		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T val = default(T);
			return (!source.TryRandomElementByWeight<T>(weightSelector, out val)) ? fallback : val;
		}

		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			bool result2;
			int j;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < ((ICollection<T>)list).Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0.0)
					{
						Log.Error("Negative weight in selector: " + num2 + " from " + list[i]);
						num2 = 0f;
					}
					num += num2;
				}
				if (((ICollection<T>)list).Count == 1 && num > 0.0)
				{
					result = list[0];
					result2 = true;
					goto IL_02b2;
				}
				if (num == 0.0)
				{
					result = default(T);
					result2 = false;
					goto IL_02b2;
				}
				num *= Rand.Value;
				for (j = 0; j < ((ICollection<T>)list).Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (!(num3 <= 0.0))
					{
						num -= num3;
						if (num <= 0.0)
							goto IL_011e;
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0.0 && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0.0)
				{
					Log.Error("Negative weight in selector: " + num4 + " from " + result);
					num4 = 0f;
				}
			}
			if (num4 == 0.0)
			{
				result = default(T);
				result2 = false;
			}
			else
			{
				while (enumerator.MoveNext())
				{
					T current = enumerator.Current;
					float num5 = weightSelector(current);
					if (num5 < 0.0)
					{
						Log.Error("Negative weight in selector: " + num5 + " from " + current);
						num5 = 0f;
					}
					if (Rand.Range(0f, num4 + num5) >= num4)
					{
						result = current;
					}
					num4 += num5;
				}
				result2 = true;
			}
			goto IL_02b2;
			IL_02b2:
			return result2;
			IL_011e:
			result = list[j];
			result2 = true;
			goto IL_02b2;
		}

		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0.0)
			{
				Log.Error("Negative default value weight.");
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T item in source)
			{
				float num2 = weightSelector(item);
				if (num2 < 0.0)
				{
					Log.Error("Negative weight in selector: " + num2 + " from " + item);
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			T result;
			if (num3 <= 0.0)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3);
				result = default(T);
			}
			else
			{
				result = ((!(Rand.Value < defaultValueWeight / num3) && num != 0.0) ? source.RandomElementByWeight<T>(weightSelector) : default(T));
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

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MaxBy<TSource, TKey>(selector, (IComparer<TKey>)Comparer<TKey>.Default);
		}

		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((object)selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource val = enumerator.Current;
				TKey y = selector(val);
				while (enumerator.MoveNext())
				{
					TSource current = enumerator.Current;
					TKey val2 = selector(current);
					if (comparer.Compare(val2, y) > 0)
					{
						val = current;
						y = val2;
					}
				}
				return val;
			}
		}

		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback<TSource, TKey>(selector, (IComparer<TKey>)Comparer<TKey>.Default, fallback);
		}

		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((object)selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					return fallback;
				}
				TSource val = enumerator.Current;
				TKey y = selector(val);
				while (enumerator.MoveNext())
				{
					TSource current = enumerator.Current;
					TKey val2 = selector(current);
					if (comparer.Compare(val2, y) > 0)
					{
						val = current;
						y = val2;
					}
				}
				return val;
			}
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.MinBy<TSource, TKey>(selector, (IComparer<TKey>)Comparer<TKey>.Default);
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((object)selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource val = enumerator.Current;
				TKey y = selector(val);
				while (enumerator.MoveNext())
				{
					TSource current = enumerator.Current;
					TKey val2 = selector(current);
					if (comparer.Compare(val2, y) < 0)
					{
						val = current;
						y = val2;
					}
				}
				return val;
			}
		}

		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count > 1)
			{
				list.Sort((Comparison<T>)((T a, T b) => ((IComparable<TSortBy>)selector(a)).CompareTo(selector(b))));
			}
		}

		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count > 1)
			{
				list.Sort((Comparison<T>)delegate(T a, T b)
				{
					TSortBy val = selector(a);
					TSortBy other = selector(b);
					return ((IEquatable<TSortBy>)val).Equals(other) ? ((IComparable<TThenBy>)thenBySelector(a)).CompareTo(thenBySelector(b)) : ((IComparable<TSortBy>)val).CompareTo(other);
				});
			}
		}

		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count > 1)
			{
				list.Sort((Comparison<T>)((T a, T b) => ((IComparable<TSortByDescending>)selector(b)).CompareTo(selector(a))));
			}
		}

		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count > 1)
			{
				list.Sort((Comparison<T>)delegate(T a, T b)
				{
					TSortByDescending other = selector(a);
					TSortByDescending other2 = selector(b);
					return ((IEquatable<TSortByDescending>)other).Equals(other2) ? ((IComparable<TThenByDescending>)thenByDescendingSelector(b)).CompareTo(thenByDescendingSelector(a)) : ((IComparable<TSortByDescending>)other2).CompareTo(other);
				});
			}
		}

		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			try
			{
				foreach (KeyValuePair<TKey, TValue> item in dictionary)
				{
					if (predicate(item))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(item.Key);
					}
				}
				if (list != null)
				{
					int num = 0;
					int count = list.Count;
					while (num < count)
					{
						dictionary.Remove(list[num]);
						num++;
					}
					return list.Count;
				}
				return 0;
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
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
				int num2 = num + 1;
				while (num2 < count)
				{
					while (num2 < count && predicate(list[num2], num2))
					{
						num2++;
					}
					if (num2 < count)
					{
						list[num++] = list[num2++];
					}
				}
			}
		}

		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
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
			foreach (T item in list)
			{
				float num3 = weight(item);
				num += num3;
				num2 += value(item) * num3;
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
					object current = enumerator.Current;
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
			foreach (T item in enumerable)
			{
				if (!predicate(item))
				{
					num++;
					continue;
				}
				break;
			}
			return num;
		}

		public static V TryGetValue<T, V>(this IDictionary<T, V> dict, T key)
		{
			V result = default(V);
			dict.TryGetValue(key, out result);
			return result;
		}

		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			T[] lhsv = lhs.ToArray<T>();
			V[] rhsv = rhs.ToArray<V>();
			int j = 0;
			int i;
			while (true)
			{
				if (j < lhsv.Length)
				{
					i = 0;
					if (i < rhsv.Length)
						break;
					j++;
					continue;
				}
				yield break;
			}
			yield return new Pair<T, V>(lhsv[j], rhsv[i]);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			using (IEnumerator<T> enumerator = lhs.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					yield return t;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return rhs;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00dc:
			/*Error near IL_00dd: Unexpected return in MoveNext()*/;
		}

		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			LocalTargetInfo result;
			int i;
			if (source == null)
			{
				result = LocalTargetInfo.Invalid;
			}
			else
			{
				for (i = 0; i < source.Count; i++)
				{
					if (source[i].IsValid)
						goto IL_002e;
				}
				result = LocalTargetInfo.Invalid;
			}
			goto IL_0057;
			IL_002e:
			result = source[i];
			goto IL_0057;
			IL_0057:
			return result;
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			using (IEnumerator<T> enumerator = lhs.GetEnumerator())
			{
				T t;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						if ((object)t != (object)rhs)
							break;
						continue;
					}
					yield break;
				}
				yield return t;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_00d3:
			/*Error near IL_00d4: Unexpected return in MoveNext()*/;
		}
	}
}
