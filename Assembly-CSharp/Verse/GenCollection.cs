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
			while (countUnChosen > 0)
			{
				rand2 = Rand.Range(0, countUnChosen);
				yield return workingList[rand2];
				T temp = workingList[rand2];
				workingList[rand2] = workingList[countUnChosen - 1];
				workingList[countUnChosen - 1] = temp;
				countUnChosen--;
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
			if (((ICollection<T>)list).Count == 0)
			{
				Log.Warning("Getting random element from empty collection.");
				return default(T);
			}
			return list[Rand.Range(0, ((ICollection<T>)list).Count)];
		}

		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T result = default(T);
			if (source.TryRandomElement<T>(out result))
			{
				return result;
			}
			return fallback;
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
				if (((ICollection<T>)list).Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else if (!source.Any<T>())
			{
				result = default(T);
				return false;
			}
			result = source.RandomElement<T>();
			return true;
		}

		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
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
					return list[0];
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
					return source.First<T>();
				}
			}
			if (num <= 0.0)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.");
				return default(T);
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < ((ICollection<T>)list).Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (!(num7 <= 0.0))
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
			return default(T);
		}

		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T result = default(T);
			if (source.TryRandomElementByWeight<T>(weightSelector, out result))
			{
				return result;
			}
			return fallback;
		}

		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
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
					return true;
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
					return true;
				}
			}
			if (num <= 0.0)
			{
				result = default(T);
				return false;
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < ((ICollection<T>)list).Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (!(num7 <= 0.0))
					{
						num6 += num7;
						if (num6 >= num5)
						{
							result = list[j];
							return true;
						}
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
							result = item2;
							return true;
						}
					}
				}
			}
			result = default(T);
			return false;
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
			if (num3 <= 0.0)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3);
				return default(T);
			}
			if (!(Rand.Value < defaultValueWeight / num3) && num != 0.0)
			{
				return source.RandomElementByWeight<T>(weightSelector);
			}
			return default(T);
		}

		public static T RandomEnumValue<T>()
		{
			return ((IEnumerable)Enum.GetValues(typeof(T))).Cast<T>().RandomElement<T>();
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
				IL_009b:
				TSource result;
				return result;
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
				IL_0098:
				TSource result;
				return result;
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
				IL_009b:
				TSource result;
				return result;
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
					if (!((IEquatable<TSortBy>)val).Equals(other))
					{
						return ((IComparable<TSortBy>)val).CompareTo(other);
					}
					return ((IComparable<TThenBy>)thenBySelector(a)).CompareTo(thenBySelector(b));
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
					if (!((IEquatable<TSortByDescending>)other).Equals(other2))
					{
						return ((IComparable<TSortByDescending>)other2).CompareTo(other);
					}
					return ((IComparable<TThenByDescending>)thenByDescendingSelector(b)).CompareTo(thenByDescendingSelector(a));
				});
			}
		}

		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			try
			{
				Dictionary<TKey, TValue>.Enumerator enumerator = dictionary.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> current = enumerator.Current;
						if (predicate(current))
						{
							if (list == null)
							{
								list = SimplePool<List<TKey>>.Get();
							}
							list.Add(current.Key);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
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
				IL_009c:
				int result;
				return result;
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
			foreach (object item in enumerable)
			{
			}
		}

		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
			{
				while (true)
				{
					if (enumerator.MoveNext())
					{
						T current = enumerator.Current;
						if (!predicate(current))
						{
							num++;
							continue;
						}
						break;
					}
					return num;
				}
				return num;
			}
		}
	}
}
