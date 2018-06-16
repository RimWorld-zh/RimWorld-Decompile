using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF0 RID: 3824
	public static class Vector2Utility
	{
		// Token: 0x06005B09 RID: 23305 RVA: 0x002E7510 File Offset: 0x002E5910
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x002E7538 File Offset: 0x002E5938
		public static Vector2 RotatedBy(this Vector2 v, float angle)
		{
			Vector3 point = new Vector3(v.x, 0f, v.y);
			point = Quaternion.AngleAxis(angle, Vector3.up) * point;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x002E758C File Offset: 0x002E598C
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x002E75CC File Offset: 0x002E59CC
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.0174532924f) * distance, v.y - Mathf.Sin(angle * 0.0174532924f) * distance);
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x002E7614 File Offset: 0x002E5A14
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.0174532924f), -Mathf.Sin(angle * 0.0174532924f));
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x002E7648 File Offset: 0x002E5A48
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x002E7678 File Offset: 0x002E5A78
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x002E76AC File Offset: 0x002E5AAC
		public static float DistanceToRect(this Vector2 u, Rect rect)
		{
			float result;
			if (rect.Contains(u))
			{
				result = 0f;
			}
			else if (u.x < rect.xMin && u.y < rect.yMin)
			{
				result = Vector2.Distance(u, new Vector2(rect.xMin, rect.yMin));
			}
			else if (u.x > rect.xMax && u.y < rect.yMin)
			{
				result = Vector2.Distance(u, new Vector2(rect.xMax, rect.yMin));
			}
			else if (u.x < rect.xMin && u.y > rect.yMax)
			{
				result = Vector2.Distance(u, new Vector2(rect.xMin, rect.yMax));
			}
			else if (u.x > rect.xMax && u.y > rect.yMax)
			{
				result = Vector2.Distance(u, new Vector2(rect.xMax, rect.yMax));
			}
			else if (u.x < rect.xMin)
			{
				result = rect.xMin - u.x;
			}
			else if (u.x > rect.xMax)
			{
				result = u.x - rect.xMax;
			}
			else if (u.y < rect.yMin)
			{
				result = rect.yMin - u.y;
			}
			else
			{
				result = u.y - rect.yMax;
			}
			return result;
		}
	}
}
