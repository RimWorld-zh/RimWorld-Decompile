using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EED RID: 3821
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x06005ACE RID: 23246 RVA: 0x002E6646 File Offset: 0x002E4A46
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x002E665E File Offset: 0x002E4A5E
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x06005AD0 RID: 23248 RVA: 0x002E6684 File Offset: 0x002E4A84
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x002E66AC File Offset: 0x002E4AAC
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005AD2 RID: 23250 RVA: 0x002E66D4 File Offset: 0x002E4AD4
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x002E66F8 File Offset: 0x002E4AF8
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06005AD4 RID: 23252 RVA: 0x002E6728 File Offset: 0x002E4B28
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x002E6760 File Offset: 0x002E4B60
		public int LengthManhattan
		{
			get
			{
				return ((this.x < 0) ? (-this.x) : this.x) + ((this.z < 0) ? (-this.z) : this.z);
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x002E67B4 File Offset: 0x002E4BB4
		public float AngleFlat
		{
			get
			{
				float result;
				if (this.x == 0 && this.z == 0)
				{
					result = 0f;
				}
				else
				{
					result = Quaternion.LookRotation(this.ToVector3()).eulerAngles.y;
				}
				return result;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x002E6808 File Offset: 0x002E4C08
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06005AD8 RID: 23256 RVA: 0x002E6828 File Offset: 0x002E4C28
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x002E6848 File Offset: 0x002E4C48
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x002E6868 File Offset: 0x002E4C68
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x002E6888 File Offset: 0x002E4C88
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x002E68A8 File Offset: 0x002E4CA8
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x002E68C8 File Offset: 0x002E4CC8
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x002E68E8 File Offset: 0x002E4CE8
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x002E6908 File Offset: 0x002E4D08
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x002E6928 File Offset: 0x002E4D28
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x002E6954 File Offset: 0x002E4D54
		public static IntVec3 FromString(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			IntVec3 result;
			try
			{
				int newX = Convert.ToInt32(array[0]);
				int newY = Convert.ToInt32(array[1]);
				int newZ = Convert.ToInt32(array[2]);
				result = new IntVec3(newX, newY, newZ);
			}
			catch (Exception arg)
			{
				Log.Warning(str + " is not a valid IntVec3 format. Exception: " + arg, false);
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x002E69FC File Offset: 0x002E4DFC
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x002E6A2C File Offset: 0x002E4E2C
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x002E6A68 File Offset: 0x002E4E68
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x002E6A8C File Offset: 0x002E4E8C
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x002E6ACC File Offset: 0x002E4ECC
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x002E6B10 File Offset: 0x002E4F10
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x002E6B2C File Offset: 0x002E4F2C
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x002E6B58 File Offset: 0x002E4F58
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x002E6B78 File Offset: 0x002E4F78
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x002E6C24 File Offset: 0x002E5024
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x002E6C80 File Offset: 0x002E5080
		public bool AdjacentToCardinal(Room room)
		{
			bool result;
			if (!this.IsValid)
			{
				result = false;
			}
			else
			{
				Map map = room.Map;
				if (this.InBounds(map) && this.GetRoom(map, RegionType.Set_All) == room)
				{
					result = true;
				}
				else
				{
					IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
					for (int i = 0; i < cardinalDirections.Length; i++)
					{
						IntVec3 intVec = this + cardinalDirections[i];
						if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_All) == room)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x002E6D34 File Offset: 0x002E5134
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x002E6D58 File Offset: 0x002E5158
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x002E6DB8 File Offset: 0x002E51B8
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x002E6E00 File Offset: 0x002E5200
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x002E6E48 File Offset: 0x002E5248
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x002E6E80 File Offset: 0x002E5280
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x002E6ED8 File Offset: 0x002E52D8
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x002E6F30 File Offset: 0x002E5330
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x002E6F60 File Offset: 0x002E5360
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x002E6FAC File Offset: 0x002E53AC
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.x);
			seed = Gen.HashCombineInt(seed, this.y);
			return Gen.HashCombineInt(seed, this.z);
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x002E6FEC File Offset: 0x002E53EC
		public ulong UniqueHashCode()
		{
			ulong num = 0UL;
			num += (ulong)((long)this.x);
			num += (ulong)(4096L * (long)this.z);
			return num + (ulong)(16777216L * (long)this.y);
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x002E7034 File Offset: 0x002E5434
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x04003C8F RID: 15503
		public int x;

		// Token: 0x04003C90 RID: 15504
		public int y;

		// Token: 0x04003C91 RID: 15505
		public int z;
	}
}
