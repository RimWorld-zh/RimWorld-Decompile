using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();

		public FastVector2Comparer()
		{
		}

		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FastVector2Comparer()
		{
		}
	}
}
