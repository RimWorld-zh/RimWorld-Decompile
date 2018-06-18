using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEC RID: 3820
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x06005ACC RID: 23244 RVA: 0x002E671E File Offset: 0x002E4B1E
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x002E6736 File Offset: 0x002E4B36
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x002E675C File Offset: 0x002E4B5C
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x002E6784 File Offset: 0x002E4B84
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005AD0 RID: 23248 RVA: 0x002E67AC File Offset: 0x002E4BAC
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x002E67D0 File Offset: 0x002E4BD0
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06005AD2 RID: 23250 RVA: 0x002E6800 File Offset: 0x002E4C00
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x002E6838 File Offset: 0x002E4C38
		public int LengthManhattan
		{
			get
			{
				return ((this.x < 0) ? (-this.x) : this.x) + ((this.z < 0) ? (-this.z) : this.z);
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005AD4 RID: 23252 RVA: 0x002E688C File Offset: 0x002E4C8C
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

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x002E68E0 File Offset: 0x002E4CE0
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x002E6900 File Offset: 0x002E4D00
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x002E6920 File Offset: 0x002E4D20
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005AD8 RID: 23256 RVA: 0x002E6940 File Offset: 0x002E4D40
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x002E6960 File Offset: 0x002E4D60
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x002E6980 File Offset: 0x002E4D80
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x002E69A0 File Offset: 0x002E4DA0
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x002E69C0 File Offset: 0x002E4DC0
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x002E69E0 File Offset: 0x002E4DE0
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x002E6A00 File Offset: 0x002E4E00
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x002E6A2C File Offset: 0x002E4E2C
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

		// Token: 0x06005AE0 RID: 23264 RVA: 0x002E6AD4 File Offset: 0x002E4ED4
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x002E6B04 File Offset: 0x002E4F04
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x002E6B40 File Offset: 0x002E4F40
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x002E6B64 File Offset: 0x002E4F64
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x002E6BA4 File Offset: 0x002E4FA4
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x002E6BE8 File Offset: 0x002E4FE8
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x002E6C04 File Offset: 0x002E5004
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x002E6C30 File Offset: 0x002E5030
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x002E6C50 File Offset: 0x002E5050
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x002E6CFC File Offset: 0x002E50FC
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x002E6D58 File Offset: 0x002E5158
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

		// Token: 0x06005AEB RID: 23275 RVA: 0x002E6E0C File Offset: 0x002E520C
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x002E6E30 File Offset: 0x002E5230
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x002E6E90 File Offset: 0x002E5290
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x002E6ED8 File Offset: 0x002E52D8
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x002E6F20 File Offset: 0x002E5320
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x002E6F58 File Offset: 0x002E5358
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x002E6FB0 File Offset: 0x002E53B0
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x002E7008 File Offset: 0x002E5408
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x002E7038 File Offset: 0x002E5438
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x002E7084 File Offset: 0x002E5484
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.x);
			seed = Gen.HashCombineInt(seed, this.y);
			return Gen.HashCombineInt(seed, this.z);
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x002E70C4 File Offset: 0x002E54C4
		public ulong UniqueHashCode()
		{
			ulong num = 0UL;
			num += (ulong)((long)this.x);
			num += (ulong)(4096L * (long)this.z);
			return num + (ulong)(16777216L * (long)this.y);
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x002E710C File Offset: 0x002E550C
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

		// Token: 0x04003C8E RID: 15502
		public int x;

		// Token: 0x04003C8F RID: 15503
		public int y;

		// Token: 0x04003C90 RID: 15504
		public int z;
	}
}
