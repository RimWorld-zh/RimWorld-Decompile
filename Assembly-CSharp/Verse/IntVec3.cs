using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEC RID: 3820
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x06005AF4 RID: 23284 RVA: 0x002E8752 File Offset: 0x002E6B52
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x002E876A File Offset: 0x002E6B6A
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x002E8790 File Offset: 0x002E6B90
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06005AF7 RID: 23287 RVA: 0x002E87B8 File Offset: 0x002E6BB8
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005AF8 RID: 23288 RVA: 0x002E87E0 File Offset: 0x002E6BE0
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005AF9 RID: 23289 RVA: 0x002E8804 File Offset: 0x002E6C04
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06005AFA RID: 23290 RVA: 0x002E8834 File Offset: 0x002E6C34
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06005AFB RID: 23291 RVA: 0x002E886C File Offset: 0x002E6C6C
		public int LengthManhattan
		{
			get
			{
				return ((this.x < 0) ? (-this.x) : this.x) + ((this.z < 0) ? (-this.z) : this.z);
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005AFC RID: 23292 RVA: 0x002E88C0 File Offset: 0x002E6CC0
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

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x002E8914 File Offset: 0x002E6D14
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06005AFE RID: 23294 RVA: 0x002E8934 File Offset: 0x002E6D34
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005AFF RID: 23295 RVA: 0x002E8954 File Offset: 0x002E6D54
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005B00 RID: 23296 RVA: 0x002E8974 File Offset: 0x002E6D74
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005B01 RID: 23297 RVA: 0x002E8994 File Offset: 0x002E6D94
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005B02 RID: 23298 RVA: 0x002E89B4 File Offset: 0x002E6DB4
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005B03 RID: 23299 RVA: 0x002E89D4 File Offset: 0x002E6DD4
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005B04 RID: 23300 RVA: 0x002E89F4 File Offset: 0x002E6DF4
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06005B05 RID: 23301 RVA: 0x002E8A14 File Offset: 0x002E6E14
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005B06 RID: 23302 RVA: 0x002E8A34 File Offset: 0x002E6E34
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x002E8A60 File Offset: 0x002E6E60
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

		// Token: 0x06005B08 RID: 23304 RVA: 0x002E8B08 File Offset: 0x002E6F08
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x002E8B38 File Offset: 0x002E6F38
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x06005B0A RID: 23306 RVA: 0x002E8B74 File Offset: 0x002E6F74
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x06005B0B RID: 23307 RVA: 0x002E8B98 File Offset: 0x002E6F98
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x06005B0C RID: 23308 RVA: 0x002E8BD8 File Offset: 0x002E6FD8
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x002E8C1C File Offset: 0x002E701C
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x002E8C38 File Offset: 0x002E7038
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x002E8C64 File Offset: 0x002E7064
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x002E8C84 File Offset: 0x002E7084
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x06005B11 RID: 23313 RVA: 0x002E8D30 File Offset: 0x002E7130
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x06005B12 RID: 23314 RVA: 0x002E8D8C File Offset: 0x002E718C
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

		// Token: 0x06005B13 RID: 23315 RVA: 0x002E8E40 File Offset: 0x002E7240
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x002E8E64 File Offset: 0x002E7264
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x002E8EC4 File Offset: 0x002E72C4
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x002E8F0C File Offset: 0x002E730C
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x002E8F54 File Offset: 0x002E7354
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x002E8F8C File Offset: 0x002E738C
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x002E8FE4 File Offset: 0x002E73E4
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x002E903C File Offset: 0x002E743C
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x002E906C File Offset: 0x002E746C
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x002E90B8 File Offset: 0x002E74B8
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.x);
			seed = Gen.HashCombineInt(seed, this.y);
			return Gen.HashCombineInt(seed, this.z);
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x002E90F8 File Offset: 0x002E74F8
		public ulong UniqueHashCode()
		{
			ulong num = 0UL;
			num += (ulong)((long)this.x);
			num += (ulong)(4096L * (long)this.z);
			return num + (ulong)(16777216L * (long)this.y);
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x002E9140 File Offset: 0x002E7540
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

		// Token: 0x04003CA1 RID: 15521
		public int x;

		// Token: 0x04003CA2 RID: 15522
		public int y;

		// Token: 0x04003CA3 RID: 15523
		public int z;
	}
}
