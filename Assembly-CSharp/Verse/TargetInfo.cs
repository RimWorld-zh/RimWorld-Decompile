using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F00 RID: 3840
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x04003CDE RID: 15582
		private Thing thingInt;

		// Token: 0x04003CDF RID: 15583
		private IntVec3 cellInt;

		// Token: 0x04003CE0 RID: 15584
		private Map mapInt;

		// Token: 0x06005C00 RID: 23552 RVA: 0x002EDB8F File Offset: 0x002EBF8F
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x002EDBAC File Offset: 0x002EBFAC
		public TargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed TargetInfo with cell=" + cell + " and a null map.", false);
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06005C02 RID: 23554 RVA: 0x002EDC04 File Offset: 0x002EC004
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06005C03 RID: 23555 RVA: 0x002EDC34 File Offset: 0x002EC034
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06005C04 RID: 23556 RVA: 0x002EDC58 File Offset: 0x002EC058
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06005C05 RID: 23557 RVA: 0x002EDC74 File Offset: 0x002EC074
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06005C06 RID: 23558 RVA: 0x002EDCA4 File Offset: 0x002EC0A4
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06005C07 RID: 23559 RVA: 0x002EDCC8 File Offset: 0x002EC0C8
		public IntVec3 Cell
		{
			get
			{
				IntVec3 positionHeld;
				if (this.thingInt != null)
				{
					positionHeld = this.thingInt.PositionHeld;
				}
				else
				{
					positionHeld = this.cellInt;
				}
				return positionHeld;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06005C08 RID: 23560 RVA: 0x002EDD00 File Offset: 0x002EC100
		public int Tile
		{
			get
			{
				int result;
				if (this.thingInt != null && this.thingInt.Tile >= 0)
				{
					result = this.thingInt.Tile;
				}
				else if (this.cellInt.IsValid && this.mapInt != null)
				{
					result = this.mapInt.Tile;
				}
				else
				{
					result = -1;
				}
				return result;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06005C09 RID: 23561 RVA: 0x002EDD70 File Offset: 0x002EC170
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005C0A RID: 23562 RVA: 0x002EDD98 File Offset: 0x002EC198
		public Map Map
		{
			get
			{
				Map mapHeld;
				if (this.thingInt != null)
				{
					mapHeld = this.thingInt.MapHeld;
				}
				else
				{
					mapHeld = this.mapInt;
				}
				return mapHeld;
			}
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x002EDDD0 File Offset: 0x002EC1D0
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x002EDDEC File Offset: 0x002EC1EC
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			LocalTargetInfo result;
			if (t.HasThing)
			{
				result = new LocalTargetInfo(t.Thing);
			}
			else
			{
				result = new LocalTargetInfo(t.Cell);
			}
			return result;
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x002EDE2C File Offset: 0x002EC22C
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x002EDE70 File Offset: 0x002EC270
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x002EDEC0 File Offset: 0x002EC2C0
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			bool result;
			if (a.Thing != null || b.Thing != null)
			{
				result = (a.Thing == b.Thing);
			}
			else
			{
				result = ((!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt));
			}
			return result;
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x002EDF58 File Offset: 0x002EC358
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x002EDF78 File Offset: 0x002EC378
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x002EDFAC File Offset: 0x002EC3AC
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x002EDFD0 File Offset: 0x002EC3D0
		public override int GetHashCode()
		{
			int result;
			if (this.thingInt != null)
			{
				result = this.thingInt.GetHashCode();
			}
			else
			{
				result = Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
			}
			return result;
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x002EE020 File Offset: 0x002EC420
		public override string ToString()
		{
			string result;
			if (this.Thing != null)
			{
				result = this.Thing.GetUniqueLoadID();
			}
			else if (this.Cell.IsValid)
			{
				result = this.Cell.ToString() + ", " + ((this.mapInt == null) ? "null" : this.mapInt.GetUniqueLoadID());
			}
			else
			{
				result = "null";
			}
			return result;
		}
	}
}
