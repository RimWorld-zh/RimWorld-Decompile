using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class Triangulator
	{
		private List<Vector2> m_points = new List<Vector2>();

		public Triangulator(Vector2[] points)
		{
			this.m_points = new List<Vector2>(points);
		}

		public int[] Triangulate()
		{
			List<int> list = new List<int>();
			int count = this.m_points.Count;
			if (count < 3)
			{
				return list.ToArray();
			}
			int[] array = new int[count];
			if (this.Area() > 0.0)
			{
				for (int i = 0; i < count; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					array[j] = count - 1 - j;
				}
			}
			int num = count;
			int num2 = 2 * num;
			int num3 = 0;
			int num4 = num - 1;
			while (num > 2)
			{
				if (num2-- <= 0)
				{
					return list.ToArray();
				}
				int num6 = num4;
				if (num <= num6)
				{
					num6 = 0;
				}
				num4 = num6 + 1;
				if (num <= num4)
				{
					num4 = 0;
				}
				int num7 = num4 + 1;
				if (num <= num7)
				{
					num7 = 0;
				}
				if (this.Snip(num6, num4, num7, num, array))
				{
					int item = array[num6];
					int item2 = array[num4];
					int item3 = array[num7];
					list.Add(item);
					list.Add(item2);
					list.Add(item3);
					num3++;
					int num8 = num4;
					for (int k = num4 + 1; k < num; k++)
					{
						array[num8] = array[k];
						num8++;
					}
					num--;
					num2 = 2 * num;
				}
			}
			list.Reverse();
			return list.ToArray();
		}

		private float Area()
		{
			int count = this.m_points.Count;
			float num = 0f;
			int index = count - 1;
			int num2 = 0;
			while (num2 < count)
			{
				Vector2 vector = this.m_points[index];
				Vector2 vector2 = this.m_points[num2];
				num += vector.x * vector2.y - vector2.x * vector.y;
				index = num2++;
			}
			return (float)(num * 0.5);
		}

		private bool Snip(int u, int v, int w, int n, int[] V)
		{
			Vector2 a = this.m_points[V[u]];
			Vector2 b = this.m_points[V[v]];
			Vector2 c = this.m_points[V[w]];
			if (Mathf.Epsilon > (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector2 p = this.m_points[V[i]];
					if (this.InsideTriangle(a, b, c, p))
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
		{
			float num = C.x - B.x;
			float num2 = C.y - B.y;
			float num3 = A.x - C.x;
			float num4 = A.y - C.y;
			float num5 = B.x - A.x;
			float num6 = B.y - A.y;
			float num7 = P.x - A.x;
			float num8 = P.y - A.y;
			float num9 = P.x - B.x;
			float num10 = P.y - B.y;
			float num11 = P.x - C.x;
			float num12 = P.y - C.y;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0.0 && num15 >= 0.0 && num14 >= 0.0;
		}
	}
}
