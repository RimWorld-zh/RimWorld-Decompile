using System;
using UnityEngine;

namespace Verse
{
	public static class GenGeo
	{
		public static float AngleDifferenceBetween(float A, float B)
		{
			float num = (float)(A + 360.0);
			float num2 = (float)(B + 360.0);
			float num3 = 9999f;
			float num4 = 0f;
			num4 = A - B;
			if (num4 < 0.0)
			{
				num4 = (float)(num4 * -1.0);
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = num - B;
			if (num4 < 0.0)
			{
				num4 = (float)(num4 * -1.0);
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = A - num2;
			if (num4 < 0.0)
			{
				num4 = (float)(num4 * -1.0);
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			return num3;
		}

		public static float MagnitudeHorizontal(this Vector3 v)
		{
			return (float)Math.Sqrt((double)(v.x * v.x + v.z * v.z));
		}

		public static float MagnitudeHorizontalSquared(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		public static bool LinesIntersect(Vector3 line1V1, Vector3 line1V2, Vector3 line2V1, Vector3 line2V2)
		{
			float num = line1V2.z - line1V1.z;
			float num2 = line1V1.x - line1V2.x;
			float num3 = num * line1V1.x + num2 * line1V1.z;
			float num4 = line2V2.z - line2V1.z;
			float num5 = line2V1.x - line2V2.x;
			float num6 = num4 * line2V1.x + num5 * line2V1.z;
			float num7 = num * num5 - num4 * num2;
			if (num7 == 0.0)
			{
				return false;
			}
			float num8 = (num5 * num3 - num2 * num6) / num7;
			float num9 = (num * num6 - num4 * num3) / num7;
			if (num8 > line1V1.x && num8 > line1V2.x)
			{
				goto IL_017e;
			}
			if (num8 > line2V1.x && num8 > line2V2.x)
			{
				goto IL_017e;
			}
			if (num8 < line1V1.x && num8 < line1V2.x)
			{
				goto IL_017e;
			}
			if (num8 < line2V1.x && num8 < line2V2.x)
			{
				goto IL_017e;
			}
			if (num9 > line1V1.z && num9 > line1V2.z)
			{
				goto IL_017e;
			}
			if (num9 > line2V1.z && num9 > line2V2.z)
			{
				goto IL_017e;
			}
			if (num9 < line1V1.z && num9 < line1V2.z)
			{
				goto IL_017e;
			}
			if (num9 < line2V1.z && num9 < line2V2.z)
				goto IL_017e;
			return true;
			IL_017e:
			return false;
		}

		public static bool IntersectLineCircle(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			Vector2 lhs = center - lineA;
			Vector2 vector = lineB - lineA;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(lhs, vector);
			float num3 = num2 / num;
			if (num3 < 0.0)
			{
				num3 = 0f;
			}
			else if (num3 > 1.0)
			{
				num3 = 1f;
			}
			Vector2 vector2 = vector * num3 + lineA - center;
			float num4 = Vector2.Dot(vector2, vector2);
			return num4 <= radius * radius;
		}

		public static Vector3 RegularPolygonVertexPositionVec3(int polygonVertices, int vertexIndex)
		{
			Vector2 vector = GenGeo.RegularPolygonVertexPosition(polygonVertices, vertexIndex);
			return new Vector3(vector.x, 0f, vector.y);
		}

		public static Vector2 RegularPolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			if (vertexIndex >= 0 && vertexIndex < polygonVertices)
			{
				if (polygonVertices == 1)
				{
					return Vector2.zero;
				}
				return GenGeo.CalculatePolygonVertexPosition(polygonVertices, vertexIndex);
			}
			Log.Warning("Vertex index out of bounds. polygonVertices=" + polygonVertices + " vertexIndex=" + vertexIndex);
			return Vector2.zero;
		}

		private static Vector2 CalculatePolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			float num = (float)(6.2831854820251465 / (float)polygonVertices);
			float num2 = num * (float)vertexIndex;
			num2 = (float)(num2 + 3.1415927410125732);
			return new Vector3(Mathf.Cos(num2), Mathf.Sin(num2));
		}
	}
}
