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
			int[] result;
			if (count < 3)
			{
				result = list.ToArray();
			}
			else
			{
				int[] array = new int[count];
				if (this.Area() > 0.0)
				{
					for (int num = 0; num < count; num++)
					{
						array[num] = num;
					}
				}
				else
				{
					for (int num2 = 0; num2 < count; num2++)
					{
						array[num2] = count - 1 - num2;
					}
				}
				int num3 = count;
				int num4 = 2 * num3;
				int num5 = 0;
				int num6 = num3 - 1;
				while (num3 > 2)
				{
					if (num4-- <= 0)
						goto IL_00a7;
					int num8 = num6;
					if (num3 <= num8)
					{
						num8 = 0;
					}
					num6 = num8 + 1;
					if (num3 <= num6)
					{
						num6 = 0;
					}
					int num9 = num6 + 1;
					if (num3 <= num9)
					{
						num9 = 0;
					}
					if (this.Snip(num8, num6, num9, num3, array))
					{
						int item = array[num8];
						int item2 = array[num6];
						int item3 = array[num9];
						list.Add(item);
						list.Add(item2);
						list.Add(item3);
						num5++;
						int num10 = num6;
						for (int num11 = num6 + 1; num11 < num3; num11++)
						{
							array[num10] = array[num11];
							num10++;
						}
						num3--;
						num4 = 2 * num3;
					}
				}
				list.Reverse();
				result = list.ToArray();
			}
			goto IL_0180;
			IL_00a7:
			result = list.ToArray();
			goto IL_0180;
			IL_0180:
			return result;
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
			bool result;
			if (Mathf.Epsilon > (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x))
			{
				result = false;
			}
			else
			{
				for (int num = 0; num < n; num++)
				{
					if (num != u && num != v && num != w)
					{
						Vector2 p = this.m_points[V[num]];
						if (this.InsideTriangle(a, b, c, p))
							goto IL_00c5;
					}
				}
				result = true;
			}
			goto IL_00e2;
			IL_00e2:
			return result;
			IL_00c5:
			result = false;
			goto IL_00e2;
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
