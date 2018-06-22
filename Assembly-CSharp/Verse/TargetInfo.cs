using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFC RID: 3836
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x06005BF6 RID: 23542 RVA: 0x002ED50F File Offset: 0x002EB90F
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06005BF7 RID: 23543 RVA: 0x002ED52C File Offset: 0x002EB92C
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

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x002ED584 File Offset: 0x002EB984
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x002ED5B4 File Offset: 0x002EB9B4
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x002ED5D8 File Offset: 0x002EB9D8
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x002ED5F4 File Offset: 0x002EB9F4
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x002ED624 File Offset: 0x002EBA24
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06005BFD RID: 23549 RVA: 0x002ED648 File Offset: 0x002EBA48
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

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x002ED680 File Offset: 0x002EBA80
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

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005BFF RID: 23551 RVA: 0x002ED6F0 File Offset: 0x002EBAF0
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005C00 RID: 23552 RVA: 0x002ED718 File Offset: 0x002EBB18
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

		// Token: 0x06005C01 RID: 23553 RVA: 0x002ED750 File Offset: 0x002EBB50
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x002ED76C File Offset: 0x002EBB6C
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

		// Token: 0x06005C03 RID: 23555 RVA: 0x002ED7AC File Offset: 0x002EBBAC
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x002ED7F0 File Offset: 0x002EBBF0
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x002ED840 File Offset: 0x002EBC40
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

		// Token: 0x06005C06 RID: 23558 RVA: 0x002ED8D8 File Offset: 0x002EBCD8
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x002ED8F8 File Offset: 0x002EBCF8
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x002ED92C File Offset: 0x002EBD2C
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x002ED950 File Offset: 0x002EBD50
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

		// Token: 0x06005C0A RID: 23562 RVA: 0x002ED9A0 File Offset: 0x002EBDA0
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

		// Token: 0x04003CDB RID: 15579
		private Thing thingInt;

		// Token: 0x04003CDC RID: 15580
		private IntVec3 cellInt;

		// Token: 0x04003CDD RID: 15581
		private Map mapInt;
	}
}
