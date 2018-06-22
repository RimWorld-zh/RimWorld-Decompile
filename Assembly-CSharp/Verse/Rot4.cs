using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF4 RID: 3828
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x06005B76 RID: 23414 RVA: 0x002EA74B File Offset: 0x002E8B4B
		public Rot4(byte newRot)
		{
			this.rotInt = newRot;
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x002EA755 File Offset: 0x002E8B55
		public Rot4(int newRot)
		{
			this.rotInt = (byte)(newRot % 4);
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005B78 RID: 23416 RVA: 0x002EA764 File Offset: 0x002E8B64
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06005B79 RID: 23417 RVA: 0x002EA784 File Offset: 0x002E8B84
		// (set) Token: 0x06005B7A RID: 23418 RVA: 0x002EA79F File Offset: 0x002E8B9F
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

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06005B7B RID: 23419 RVA: 0x002EA7AC File Offset: 0x002E8BAC
		// (set) Token: 0x06005B7C RID: 23420 RVA: 0x002EA7C7 File Offset: 0x002E8BC7
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

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06005B7D RID: 23421 RVA: 0x002EA7E4 File Offset: 0x002E8BE4
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

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06005B7E RID: 23422 RVA: 0x002EA84C File Offset: 0x002E8C4C
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

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06005B7F RID: 23423 RVA: 0x002EA8E0 File Offset: 0x002E8CE0
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x002EA910 File Offset: 0x002E8D10
		public static Rot4 North
		{
			get
			{
				return new Rot4(0);
			}
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06005B81 RID: 23425 RVA: 0x002EA92C File Offset: 0x002E8D2C
		public static Rot4 East
		{
			get
			{
				return new Rot4(1);
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06005B82 RID: 23426 RVA: 0x002EA948 File Offset: 0x002E8D48
		public static Rot4 South
		{
			get
			{
				return new Rot4(2);
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06005B83 RID: 23427 RVA: 0x002EA964 File Offset: 0x002E8D64
		public static Rot4 West
		{
			get
			{
				return new Rot4(3);
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06005B84 RID: 23428 RVA: 0x002EA980 File Offset: 0x002E8D80
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06005B85 RID: 23429 RVA: 0x002EA9A4 File Offset: 0x002E8DA4
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

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x002EA9D0 File Offset: 0x002E8DD0
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

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x002EAA48 File Offset: 0x002E8E48
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

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x002EAAC0 File Offset: 0x002E8EC0
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

		// Token: 0x06005B89 RID: 23433 RVA: 0x002EAB30 File Offset: 0x002E8F30
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

		// Token: 0x06005B8A RID: 23434 RVA: 0x002EAB60 File Offset: 0x002E8F60
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x002EAB88 File Offset: 0x002E8F88
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

		// Token: 0x06005B8C RID: 23436 RVA: 0x002EAC08 File Offset: 0x002E9008
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

		// Token: 0x06005B8D RID: 23437 RVA: 0x002EAC98 File Offset: 0x002E9098
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x002EACBC File Offset: 0x002E90BC
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x002EACE4 File Offset: 0x002E90E4
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x002EAD0C File Offset: 0x002E910C
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

		// Token: 0x06005B91 RID: 23441 RVA: 0x002EAD78 File Offset: 0x002E9178
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x002EADA0 File Offset: 0x002E91A0
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

		// Token: 0x06005B93 RID: 23443 RVA: 0x002EAE1C File Offset: 0x002E921C
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

		// Token: 0x06005B94 RID: 23444 RVA: 0x002EAED0 File Offset: 0x002E92D0
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x002EAF04 File Offset: 0x002E9304
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04003CAB RID: 15531
		private byte rotInt;
	}
}
