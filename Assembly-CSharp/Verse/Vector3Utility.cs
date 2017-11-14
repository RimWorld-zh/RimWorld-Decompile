using UnityEngine;

namespace Verse
{
	public static class Vector3Utility
	{
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0.0 && v.z == 0.0)
			{
				return 0f;
			}
			Vector3 eulerAngles = Quaternion.LookRotation(v).eulerAngles;
			return eulerAngles.y;
		}

		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new Vector3(orig.z, orig.y, (float)(0.0 - orig.x));
			case 2:
				return new Vector3((float)(0.0 - orig.x), orig.y, (float)(0.0 - orig.z));
			case 3:
				return new Vector3((float)(0.0 - orig.z), orig.y, orig.x);
			default:
				return orig;
			}
		}

		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}
	}
}
