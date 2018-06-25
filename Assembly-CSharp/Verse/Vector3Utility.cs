using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF1 RID: 3825
	public static class Vector3Utility
	{
		// Token: 0x06005B29 RID: 23337 RVA: 0x002E96D4 File Offset: 0x002E7AD4
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x002E9700 File Offset: 0x002E7B00
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

		// Token: 0x06005B2B RID: 23339 RVA: 0x002E9758 File Offset: 0x002E7B58
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x002E97AC File Offset: 0x002E7BAC
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x002E97D4 File Offset: 0x002E7BD4
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

		// Token: 0x06005B2E RID: 23342 RVA: 0x002E9878 File Offset: 0x002E7C78
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x002E98B8 File Offset: 0x002E7CB8
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x002E98EC File Offset: 0x002E7CEC
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x002E991C File Offset: 0x002E7D1C
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
