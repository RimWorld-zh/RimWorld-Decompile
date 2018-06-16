using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF5 RID: 3829
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x06005B50 RID: 23376 RVA: 0x002E863F File Offset: 0x002E6A3F
		public Rot4(byte newRot)
		{
			this.rotInt = newRot;
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x002E8649 File Offset: 0x002E6A49
		public Rot4(int newRot)
		{
			this.rotInt = (byte)(newRot % 4);
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005B52 RID: 23378 RVA: 0x002E8658 File Offset: 0x002E6A58
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005B53 RID: 23379 RVA: 0x002E8678 File Offset: 0x002E6A78
		// (set) Token: 0x06005B54 RID: 23380 RVA: 0x002E8693 File Offset: 0x002E6A93
		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = value % 4;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005B55 RID: 23381 RVA: 0x002E86A0 File Offset: 0x002E6AA0
		// (set) Token: 0x06005B56 RID: 23382 RVA: 0x002E86BB File Offset: 0x002E6ABB
		public int AsInt
		{
			get
			{
				return (int)this.rotInt;
			}
			set
			{
				if (value < 0)
				{
					value += 4000;
				}
				this.rotInt = (byte)(value % 4);
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005B57 RID: 23383 RVA: 0x002E86D8 File Offset: 0x002E6AD8
		public float AsAngle
		{
			get
			{
				float result;
				switch (this.AsInt)
				{
				case 0:
					result = 0f;
					break;
				case 1:
					result = 90f;
					break;
				case 2:
					result = 180f;
					break;
				case 3:
					result = 270f;
					break;
				default:
					result = 0f;
					break;
				}
				return result;
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06005B58 RID: 23384 RVA: 0x002E8740 File Offset: 0x002E6B40
		public Quaternion AsQuat
		{
			get
			{
				Quaternion result;
				switch (this.rotInt)
				{
				case 0:
					result = Quaternion.identity;
					break;
				case 1:
					result = Quaternion.LookRotation(Vector3.right);
					break;
				case 2:
					result = Quaternion.LookRotation(Vector3.back);
					break;
				case 3:
					result = Quaternion.LookRotation(Vector3.left);
					break;
				default:
					Log.Error("ToQuat with Rot = " + this.AsInt, false);
					result = Quaternion.identity;
					break;
				}
				return result;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06005B59 RID: 23385 RVA: 0x002E87D4 File Offset: 0x002E6BD4
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06005B5A RID: 23386 RVA: 0x002E8804 File Offset: 0x002E6C04
		public static Rot4 North
		{
			get
			{
				return new Rot4(0);
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06005B5B RID: 23387 RVA: 0x002E8820 File Offset: 0x002E6C20
		public static Rot4 East
		{
			get
			{
				return new Rot4(1);
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x002E883C File Offset: 0x002E6C3C
		public static Rot4 South
		{
			get
			{
				return new Rot4(2);
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x002E8858 File Offset: 0x002E6C58
		public static Rot4 West
		{
			get
			{
				return new Rot4(3);
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x002E8874 File Offset: 0x002E6C74
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x002E8898 File Offset: 0x002E6C98
		public static Rot4 Invalid
		{
			get
			{
				return new Rot4
				{
					rotInt = 200
				};
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x002E88C4 File Offset: 0x002E6CC4
		public IntVec3 FacingCell
		{
			get
			{
				IntVec3 result;
				switch (this.AsInt)
				{
				case 0:
					result = new IntVec3(0, 0, 1);
					break;
				case 1:
					result = new IntVec3(1, 0, 0);
					break;
				case 2:
					result = new IntVec3(0, 0, -1);
					break;
				case 3:
					result = new IntVec3(-1, 0, 0);
					break;
				default:
					result = default(IntVec3);
					break;
				}
				return result;
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x002E893C File Offset: 0x002E6D3C
		public IntVec3 RighthandCell
		{
			get
			{
				IntVec3 result;
				switch (this.AsInt)
				{
				case 0:
					result = new IntVec3(1, 0, 0);
					break;
				case 1:
					result = new IntVec3(0, 0, -1);
					break;
				case 2:
					result = new IntVec3(-1, 0, 0);
					break;
				case 3:
					result = new IntVec3(0, 0, 1);
					break;
				default:
					result = default(IntVec3);
					break;
				}
				return result;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x002E89B4 File Offset: 0x002E6DB4
		public Rot4 Opposite
		{
			get
			{
				Rot4 result;
				switch (this.AsInt)
				{
				case 0:
					result = new Rot4(2);
					break;
				case 1:
					result = new Rot4(3);
					break;
				case 2:
					result = new Rot4(0);
					break;
				case 3:
					result = new Rot4(1);
					break;
				default:
					result = default(Rot4);
					break;
				}
				return result;
			}
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x002E8A24 File Offset: 0x002E6E24
		public void Rotate(RotationDirection RotDir)
		{
			if (RotDir == RotationDirection.Clockwise)
			{
				this.AsInt++;
			}
			if (RotDir == RotationDirection.Counterclockwise)
			{
				this.AsInt--;
			}
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x002E8A54 File Offset: 0x002E6E54
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			this.Rotate(RotDir);
			return result;
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x002E8A78 File Offset: 0x002E6E78
		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			Rot4 result;
			if (angle < 45f)
			{
				result = Rot4.North;
			}
			else if (angle < 135f)
			{
				result = Rot4.East;
			}
			else if (angle < 225f)
			{
				result = Rot4.South;
			}
			else if (angle < 315f)
			{
				result = Rot4.West;
			}
			else
			{
				result = Rot4.North;
			}
			return result;
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x002E8AF8 File Offset: 0x002E6EF8
		public static Rot4 FromIntVec3(IntVec3 offset)
		{
			Rot4 result;
			if (offset.x == 1)
			{
				result = Rot4.East;
			}
			else if (offset.x == -1)
			{
				result = Rot4.West;
			}
			else if (offset.z == 1)
			{
				result = Rot4.North;
			}
			else if (offset.z == -1)
			{
				result = Rot4.South;
			}
			else
			{
				Log.Error("FromIntVec3 with bad offset " + offset, false);
				result = Rot4.North;
			}
			return result;
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x002E8B88 File Offset: 0x002E6F88
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x002E8BAC File Offset: 0x002E6FAC
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x002E8BD4 File Offset: 0x002E6FD4
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x002E8BFC File Offset: 0x002E6FFC
		public override int GetHashCode()
		{
			int result;
			switch (this.rotInt)
			{
			case 0:
				result = 235515;
				break;
			case 1:
				result = 5612938;
				break;
			case 2:
				result = 1215650;
				break;
			case 3:
				result = 9231792;
				break;
			default:
				result = (int)this.rotInt;
				break;
			}
			return result;
		}

		// Token: 0x06005B6B RID: 23403 RVA: 0x002E8C68 File Offset: 0x002E7068
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x002E8C90 File Offset: 0x002E7090
		public string ToStringHuman()
		{
			string result;
			switch (this.rotInt)
			{
			case 0:
				result = "North".Translate();
				break;
			case 1:
				result = "East".Translate();
				break;
			case 2:
				result = "South".Translate();
				break;
			case 3:
				result = "West".Translate();
				break;
			default:
				result = "error";
				break;
			}
			return result;
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x002E8D0C File Offset: 0x002E710C
		public static Rot4 FromString(string str)
		{
			int num;
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
			}
			else
			{
				if (str != null)
				{
					if (str == "North")
					{
						newRot = 0;
						goto IL_96;
					}
					if (str == "East")
					{
						newRot = 1;
						goto IL_96;
					}
					if (str == "South")
					{
						newRot = 2;
						goto IL_96;
					}
					if (str == "West")
					{
						newRot = 3;
						goto IL_96;
					}
				}
				newRot = 0;
				Log.Error("Invalid rotation: " + str, false);
				IL_96:;
			}
			return new Rot4(newRot);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x002E8DC0 File Offset: 0x002E71C0
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x002E8DF4 File Offset: 0x002E71F4
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04003C99 RID: 15513
		private byte rotInt;
	}
}
