using System;
using System.Collections.Generic;

namespace Verse
{
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		private T1 first;

		private T2 second;

		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		public override string ToString()
		{
			return "(" + this.First.ToString() + ", " + this.Second.ToString() + ")";
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<T1>(seed, this.first);
			return Gen.HashCombine<T2>(seed, this.second);
		}

		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}
	}
}
