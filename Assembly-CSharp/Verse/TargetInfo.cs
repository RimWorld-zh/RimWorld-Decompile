using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFD RID: 3837
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x06005BD0 RID: 23504 RVA: 0x002EB3FF File Offset: 0x002E97FF
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06005BD1 RID: 23505 RVA: 0x002EB41C File Offset: 0x002E981C
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

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x002EB474 File Offset: 0x002E9874
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x002EB4A4 File Offset: 0x002E98A4
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06005BD4 RID: 23508 RVA: 0x002EB4C8 File Offset: 0x002E98C8
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x002EB4E4 File Offset: 0x002E98E4
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x002EB514 File Offset: 0x002E9914
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x002EB538 File Offset: 0x002E9938
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

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x002EB570 File Offset: 0x002E9970
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

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x002EB5E0 File Offset: 0x002E99E0
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06005BDA RID: 23514 RVA: 0x002EB608 File Offset: 0x002E9A08
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

		// Token: 0x06005BDB RID: 23515 RVA: 0x002EB640 File Offset: 0x002E9A40
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x002EB65C File Offset: 0x002E9A5C
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

		// Token: 0x06005BDD RID: 23517 RVA: 0x002EB69C File Offset: 0x002E9A9C
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted TargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06005BDE RID: 23518 RVA: 0x002EB6E0 File Offset: 0x002E9AE0
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted TargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x002EB730 File Offset: 0x002E9B30
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

		// Token: 0x06005BE0 RID: 23520 RVA: 0x002EB7C8 File Offset: 0x002E9BC8
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x002EB7E8 File Offset: 0x002E9BE8
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x06005BE2 RID: 23522 RVA: 0x002EB81C File Offset: 0x002E9C1C
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x002EB840 File Offset: 0x002E9C40
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

		// Token: 0x06005BE4 RID: 23524 RVA: 0x002EB890 File Offset: 0x002E9C90
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

		// Token: 0x04003CC9 RID: 15561
		private Thing thingInt;

		// Token: 0x04003CCA RID: 15562
		private IntVec3 cellInt;

		// Token: 0x04003CCB RID: 15563
		private Map mapInt;
	}
}
