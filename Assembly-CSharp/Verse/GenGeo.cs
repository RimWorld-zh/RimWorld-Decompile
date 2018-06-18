using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F3F RID: 3903
	public static class GenGeo
	{
		// Token: 0x06005E1A RID: 24090 RVA: 0x002FCEA4 File Offset: 0x002FB2A4
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

		// Token: 0x06005E1B RID: 24091 RVA: 0x002FCF38 File Offset: 0x002FB338
		public static float MagnitudeHorizontal(this Vector3 v)
		{
			return (float)Math.Sqrt((double)(v.x * v.x + v.z * v.z));
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x002FCF74 File Offset: 0x002FB374
		public static float MagnitudeHorizontalSquared(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x002FCFA8 File Offset: 0x002FB3A8
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

		// Token: 0x06005E1E RID: 24094 RVA: 0x002FD150 File Offset: 0x002FB550
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

		// Token: 0x06005E1F RID: 24095 RVA: 0x002FD1E8 File Offset: 0x002FB5E8
		public static Vector3 RegularPolygonVertexPositionVec3(int polygonVertices, int vertexIndex)
		{
			Vector2 vector = GenGeo.RegularPolygonVertexPosition(polygonVertices, vertexIndex);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06005E20 RID: 24096 RVA: 0x002FD220 File Offset: 0x002FB620
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

		// Token: 0x06005E21 RID: 24097 RVA: 0x002FD29C File Offset: 0x002FB69C
		private static Vector2 CalculatePolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			float num = 6.28318548f / (float)polygonVertices;
			float num2 = num * (float)vertexIndex;
			num2 += 3.14159274f;
			return new Vector3(Mathf.Cos(num2), Mathf.Sin(num2));
		}

		// Token: 0x06005E22 RID: 24098 RVA: 0x002FD2E0 File Offset: 0x002FB6E0
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
