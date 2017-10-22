using System;
using UnityEngine;

namespace Verse
{
	public struct IntVec3 : IEquatable<IntVec3>
	{
		public int x;

		public int y;

		public int z;

		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		public int LengthManhattan
		{
			get
			{
				return ((this.x < 0) ? (-this.x) : this.x) + ((this.z < 0) ? (-this.z) : this.z);
			}
		}

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
					Vector3 eulerAngles = Quaternion.LookRotation(this.ToVector3()).eulerAngles;
					result = eulerAngles.y;
				}
				return result;
			}
		}

		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		public static IntVec3 FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',');
			try
			{
				int newX = Convert.ToInt32(array[0]);
				int newY = Convert.ToInt32(array[1]);
				int newZ = Convert.ToInt32(array[2]);
				return new IntVec3(newX, newY, newZ);
			}
			catch (Exception arg)
			{
				Log.Warning(str + " is not a valid IntVec3 format. Exception: " + arg);
				return IntVec3.Invalid;
			}
		}

		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)((float)this.x + 0.5), (float)this.y, (float)((float)this.z + 0.5));
		}

		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(Altitudes.AltitudeFor(AltLayer));
		}

		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)((float)this.x + 0.5), (float)this.y + AddedAltitude, (float)((float)this.z + 0.5));
		}

		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		public bool AdjacentToCardinal(IntVec3 other)
		{
			return (byte)(this.IsValid ? ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) ? 1 : ((other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)) ? 1 : 0)) : 0) != 0;
		}

		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

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
							goto IL_0088;
					}
					result = false;
				}
			}
			goto IL_00a4;
			IL_00a4:
			return result;
			IL_0088:
			result = true;
			goto IL_00a4;
		}

		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return (byte)((a.x == b.x && a.z == b.z && a.y == b.y) ? 1 : 0) != 0;
		}

		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return (byte)((a.x != b.x || a.z != b.z || a.y != b.y) ? 1 : 0) != 0;
		}

		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.x);
			seed = Gen.HashCombineInt(seed, this.y);
			return Gen.HashCombineInt(seed, this.z);
		}

		public ulong UniqueHashCode()
		{
			ulong num = 0uL;
			num = (ulong)((long)num + (long)this.x);
			num = (ulong)((long)num + 4096L * (long)this.z);
			return (ulong)((long)num + 16777216L * (long)this.y);
		}

		public override string ToString()
		{
			return "(" + this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString() + ")";
		}
	}
}
