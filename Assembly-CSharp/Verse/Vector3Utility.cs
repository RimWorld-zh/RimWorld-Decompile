using System;
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
			float result;
			if (v.x == 0f && v.z == 0f)
			{
				result = 0f;
			}
			else
			{
				result = Quaternion.LookRotation(v).eulerAngles.y;
			}
			return result;
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
			Vector3 result;
			switch (rot.AsInt)
			{
			case 0:
				result = orig;
				break;
			case 1:
				result = new Vector3(orig.z, orig.y, -orig.x);
				break;
			case 2:
				result = new Vector3(-orig.x, orig.y, -orig.z);
				break;
			case 3:
				result = new Vector3(-orig.z, orig.y, orig.x);
				break;
			default:
				result = orig;
				break;
			}
			return result;
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

		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
