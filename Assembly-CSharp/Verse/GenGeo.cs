using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F43 RID: 3907
	public static class GenGeo
	{
		// Token: 0x06005E4C RID: 24140 RVA: 0x002FF560 File Offset: 0x002FD960
		public static float AngleDifferenceBetween(float A, float B)
		{
			float num = A + 360f;
			float num2 = B + 360f;
			float num3 = 9999f;
			float num4 = A - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = num - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = A - num2;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			return num3;
		}

		// Token: 0x06005E4D RID: 24141 RVA: 0x002FF5F4 File Offset: 0x002FD9F4
		public static float MagnitudeHorizontal(this Vector3 v)
		{
			return (float)Math.Sqrt((double)(v.x * v.x + v.z * v.z));
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x002FF630 File Offset: 0x002FDA30
		public static float MagnitudeHorizontalSquared(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x002FF664 File Offset: 0x002FDA64
		public static bool LinesIntersect(Vector3 line1V1, Vector3 line1V2, Vector3 line2V1, Vector3 line2V2)
		{
			float num = line1V2.z - line1V1.z;
			float num2 = line1V1.x - line1V2.x;
			float num3 = num * line1V1.x + num2 * line1V1.z;
			float num4 = line2V2.z - line2V1.z;
			float num5 = line2V1.x - line2V2.x;
			float num6 = num4 * line2V1.x + num5 * line2V1.z;
			float num7 = num * num5 - num4 * num2;
			bool result;
			if (num7 == 0f)
			{
				result = false;
			}
			else
			{
				float num8 = (num5 * num3 - num2 * num6) / num7;
				float num9 = (num * num6 - num4 * num3) / num7;
				result = ((num8 <= line1V1.x || num8 <= line1V2.x) && (num8 <= line2V1.x || num8 <= line2V2.x) && (num8 >= line1V1.x || num8 >= line1V2.x) && (num8 >= line2V1.x || num8 >= line2V2.x) && (num9 <= line1V1.z || num9 <= line1V2.z) && (num9 <= line2V1.z || num9 <= line2V2.z) && (num9 >= line1V1.z || num9 >= line1V2.z) && (num9 >= line2V1.z || num9 >= line2V2.z));
			}
			return result;
		}

		// Token: 0x06005E50 RID: 24144 RVA: 0x002FF80C File Offset: 0x002FDC0C
		public static bool IntersectLineCircle(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			Vector2 lhs = center - lineA;
			Vector2 vector = lineB - lineA;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(lhs, vector);
			float num3 = num2 / num;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			else if (num3 > 1f)
			{
				num3 = 1f;
			}
			Vector2 vector2 = vector * num3 + lineA - center;
			float num4 = Vector2.Dot(vector2, vector2);
			return num4 <= radius * radius;
		}

		// Token: 0x06005E51 RID: 24145 RVA: 0x002FF8A4 File Offset: 0x002FDCA4
		public static Vector3 RegularPolygonVertexPositionVec3(int polygonVertices, int vertexIndex)
		{
			Vector2 vector = GenGeo.RegularPolygonVertexPosition(polygonVertices, vertexIndex);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06005E52 RID: 24146 RVA: 0x002FF8DC File Offset: 0x002FDCDC
		public static Vector2 RegularPolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			Vector2 result;
			if (vertexIndex < 0 || vertexIndex >= polygonVertices)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Vertex index out of bounds. polygonVertices=",
					polygonVertices,
					" vertexIndex=",
					vertexIndex
				}), false);
				result = Vector2.zero;
			}
			else if (polygonVertices == 1)
			{
				result = Vector2.zero;
			}
			else
			{
				result = GenGeo.CalculatePolygonVertexPosition(polygonVertices, vertexIndex);
			}
			return result;
		}

		// Token: 0x06005E53 RID: 24147 RVA: 0x002FF958 File Offset: 0x002FDD58
		private static Vector2 CalculatePolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			float num = 6.28318548f / (float)polygonVertices;
			float num2 = num * (float)vertexIndex;
			num2 += 3.14159274f;
			return new Vector3(Mathf.Cos(num2), Mathf.Sin(num2));
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x002FF99C File Offset: 0x002FDD9C
		public static Vector2 InverseQuadBilinear(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = (p0 - p).Cross(p0 - p2);
			float num2 = ((p0 - p).Cross(p1 - p3) + (p1 - p).Cross(p0 - p2)) / 2f;
			float num3 = (p1 - p).Cross(p1 - p3);
			float num4 = num2 * num2 - num * num3;
			Vector2 result;
			if (num4 < 0f)
			{
				result = new Vector2(-1f, -1f);
			}
			else
			{
				num4 = Mathf.Sqrt(num4);
				float num5;
				if (Mathf.Abs(num - 2f * num2 + num3) < 0.0001f)
				{
					num5 = num / (num - num3);
				}
				else
				{
					float num6 = (num - num2 + num4) / (num - 2f * num2 + num3);
					float num7 = (num - num2 - num4) / (num - 2f * num2 + num3);
					if (Mathf.Abs(num6 - 0.5f) < Mathf.Abs(num7 - 0.5f))
					{
						num5 = num6;
					}
					else
					{
						num5 = num7;
					}
				}
				float num8 = (1f - num5) * (p0.x - p2.x) + num5 * (p1.x - p3.x);
				float num9 = (1f - num5) * (p0.y - p2.y) + num5 * (p1.y - p3.y);
				if (Mathf.Abs(num8) < Mathf.Abs(num9))
				{
					result = new Vector2(num5, ((1f - num5) * (p0.y - p.y) + num5 * (p1.y - p.y)) / num9);
				}
				else
				{
					result = new Vector2(num5, ((1f - num5) * (p0.x - p.x) + num5 * (p1.x - p.x)) / num8);
				}
			}
			return result;
		}
	}
}
