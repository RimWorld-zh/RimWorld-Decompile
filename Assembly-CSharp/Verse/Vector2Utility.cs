using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEF RID: 3823
	public static class Vector2Utility
	{
		// Token: 0x06005B2F RID: 23343 RVA: 0x002E961C File Offset: 0x002E7A1C
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x002E9644 File Offset: 0x002E7A44
		public static Vector2 RotatedBy(this Vector2 v, float angle)
		{
			Vector3 point = new Vector3(v.x, 0f, v.y);
			point = Quaternion.AngleAxis(angle, Vector3.up) * point;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x002E9698 File Offset: 0x002E7A98
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x002E96D8 File Offset: 0x002E7AD8
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.0174532924f) * distance, v.y - Mathf.Sin(angle * 0.0174532924f) * distance);
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x002E9720 File Offset: 0x002E7B20
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.0174532924f), -Mathf.Sin(angle * 0.0174532924f));
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x002E9754 File Offset: 0x002E7B54
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x002E9784 File Offset: 0x002E7B84
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x06005B36 RID: 23350 RVA: 0x002E97B8 File Offset: 0x002E7BB8
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
