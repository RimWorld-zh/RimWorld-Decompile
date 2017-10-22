using System;
using UnityEngine;

namespace Verse
{
	public struct Rot4 : IEquatable<Rot4>
	{
		private byte rotInt;

		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = (byte)((int)value % 4);
			}
		}

		public int AsInt
		{
			get
			{
				return this.rotInt;
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

		public float AsAngle
		{
			get
			{
				float result;
				switch (this.AsInt)
				{
				case 0:
				{
					result = 0f;
					break;
				}
				case 1:
				{
					result = 90f;
					break;
				}
				case 2:
				{
					result = 180f;
					break;
				}
				case 3:
				{
					result = 270f;
					break;
				}
				default:
				{
					result = 0f;
					break;
				}
				}
				return result;
			}
		}

		public Quaternion AsQuat
		{
			get
			{
				Quaternion result;
				switch (this.rotInt)
				{
				case (byte)0:
				{
					result = Quaternion.identity;
					break;
				}
				case (byte)1:
				{
					result = Quaternion.LookRotation(Vector3.right);
					break;
				}
				case (byte)2:
				{
					result = Quaternion.LookRotation(Vector3.back);
					break;
				}
				case (byte)3:
				{
					result = Quaternion.LookRotation(Vector3.left);
					break;
				}
				default:
				{
					Log.Error("ToQuat with Rot = " + this.AsInt);
					result = Quaternion.identity;
					break;
				}
				}
				return result;
			}
		}

		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		public static Rot4 North
		{
			get
			{
				return new Rot4(0);
			}
		}

		public static Rot4 East
		{
			get
			{
				return new Rot4(1);
			}
		}

		public static Rot4 South
		{
			get
			{
				return new Rot4(2);
			}
		}

		public static Rot4 West
		{
			get
			{
				return new Rot4(3);
			}
		}

		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		public static Rot4 Invalid
		{
			get
			{
				return new Rot4
				{
					rotInt = (byte)200
				};
			}
		}

		public IntVec3 FacingCell
		{
			get
			{
				IntVec3 result;
				switch (this.AsInt)
				{
				case 0:
				{
					result = new IntVec3(0, 0, 1);
					break;
				}
				case 1:
				{
					result = new IntVec3(1, 0, 0);
					break;
				}
				case 2:
				{
					result = new IntVec3(0, 0, -1);
					break;
				}
				case 3:
				{
					result = new IntVec3(-1, 0, 0);
					break;
				}
				default:
				{
					result = default(IntVec3);
					break;
				}
				}
				return result;
			}
		}

		public Rot4 Opposite
		{
			get
			{
				Rot4 result;
				switch (this.AsInt)
				{
				case 0:
				{
					result = new Rot4(2);
					break;
				}
				case 1:
				{
					result = new Rot4(3);
					break;
				}
				case 2:
				{
					result = new Rot4(0);
					break;
				}
				case 3:
				{
					result = new Rot4(1);
					break;
				}
				default:
				{
					result = default(Rot4);
					break;
				}
				}
				return result;
			}
		}

		public Rot4(byte newRot)
		{
			this.rotInt = newRot;
		}

		public Rot4(int newRot)
		{
			this.rotInt = (byte)(newRot % 4);
		}

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

		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			return (!(angle < 45.0)) ? ((!(angle < 135.0)) ? ((!(angle < 225.0)) ? ((!(angle < 315.0)) ? Rot4.North : Rot4.West) : Rot4.South) : Rot4.East) : Rot4.North;
		}

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
				Log.Error("FromIntVec3 with bad offset " + offset);
				result = Rot4.North;
			}
			return result;
		}

		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		public override int GetHashCode()
		{
			int result;
			switch (this.rotInt)
			{
			case (byte)0:
			{
				result = 235515;
				break;
			}
			case (byte)1:
			{
				result = 5612938;
				break;
			}
			case (byte)2:
			{
				result = 1215650;
				break;
			}
			case (byte)3:
			{
				result = 9231792;
				break;
			}
			default:
			{
				throw new InvalidOperationException("IntRot out of range.");
			}
			}
			return result;
		}

		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		public string ToStringHuman()
		{
			string result;
			switch (this.rotInt)
			{
			case (byte)0:
			{
				result = "North".Translate();
				break;
			}
			case (byte)1:
			{
				result = "East".Translate();
				break;
			}
			case (byte)2:
			{
				result = "South".Translate();
				break;
			}
			case (byte)3:
			{
				result = "West".Translate();
				break;
			}
			default:
			{
				result = "error";
				break;
			}
			}
			return result;
		}

		public static Rot4 FromString(string str)
		{
			int num = default(int);
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
				goto IL_0096;
			}
			if (str != null)
			{
				if (!(str == "North"))
				{
					if (!(str == "East"))
					{
						if (!(str == "South"))
						{
							if (str == "West")
							{
								newRot = (byte)3;
								goto IL_0096;
							}
							goto IL_007e;
						}
						newRot = (byte)2;
					}
					else
					{
						newRot = (byte)1;
					}
				}
				else
				{
					newRot = (byte)0;
				}
				goto IL_0096;
			}
			goto IL_007e;
			IL_0096:
			return new Rot4(newRot);
			IL_007e:
			newRot = (byte)0;
			Log.Error("Invalid rotation: " + str);
			goto IL_0096;
		}

		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}
	}
}
