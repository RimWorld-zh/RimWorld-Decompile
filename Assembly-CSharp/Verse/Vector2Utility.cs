using UnityEngine;

namespace Verse
{
	public static class Vector2Utility
	{
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		public static Vector2 RotatedBy(this Vector2 v, float angle)
		{
			Vector3 point = new Vector3(v.x, 0f, v.y);
			point = Quaternion.AngleAxis(angle, Vector3.up) * point;
			return new Vector2(point.x, point.z);
		}

		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return (float)(Mathf.Atan2((float)(0.0 - (b.y - a.y)), b.x - a.x) * 57.295780181884766);
		}

		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos((float)(angle * 0.01745329238474369)) * distance, v.y - Mathf.Sin((float)(angle * 0.01745329238474369)) * distance);
		}

		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos((float)(angle * 0.01745329238474369)), (float)(0.0 - Mathf.Sin((float)(angle * 0.01745329238474369))));
		}

		public static float ToAngle(this Vector2 v)
		{
			return (float)(Mathf.Atan2((float)(0.0 - v.y), v.x) * 57.295780181884766);
		}

		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}
	}
}
