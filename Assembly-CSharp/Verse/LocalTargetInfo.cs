using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF0 RID: 3824
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x06005B0F RID: 23311 RVA: 0x002E794C File Offset: 0x002E5D4C
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x002E7961 File Offset: 0x002E5D61
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005B11 RID: 23313 RVA: 0x002E7974 File Offset: 0x002E5D74
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005B12 RID: 23314 RVA: 0x002E79A4 File Offset: 0x002E5DA4
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06005B13 RID: 23315 RVA: 0x002E79C8 File Offset: 0x002E5DC8
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005B14 RID: 23316 RVA: 0x002E79E4 File Offset: 0x002E5DE4
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06005B15 RID: 23317 RVA: 0x002E7A14 File Offset: 0x002E5E14
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06005B16 RID: 23318 RVA: 0x002E7A34 File Offset: 0x002E5E34
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

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06005B17 RID: 23319 RVA: 0x002E7A6C File Offset: 0x002E5E6C
		public Vector3 CenterVector3
		{
			get
			{
				Vector3 result;
				if (this.thingInt != null)
				{
					if (this.thingInt.Spawned)
					{
						result = this.thingInt.DrawPos;
					}
					else if (this.thingInt.SpawnedOrAnyParentSpawned)
					{
						result = this.thingInt.PositionHeld.ToVector3Shifted();
					}
					else
					{
						result = this.thingInt.Position.ToVector3Shifted();
					}
				}
				else if (this.cellInt.IsValid)
				{
					result = this.cellInt.ToVector3Shifted();
				}
				else
				{
					result = default(Vector3);
				}
				return result;
			}
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x002E7B1C File Offset: 0x002E5F1C
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x002E7B38 File Offset: 0x002E5F38
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x002E7B54 File Offset: 0x002E5F54
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x002E7B98 File Offset: 0x002E5F98
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x002E7BE8 File Offset: 0x002E5FE8
		public TargetInfo ToTargetInfo(Map map)
		{
			TargetInfo result;
			if (!this.IsValid)
			{
				result = TargetInfo.Invalid;
			}
			else if (this.Thing != null)
			{
				result = new TargetInfo(this.Thing);
			}
			else
			{
				result = new TargetInfo(this.Cell, map, false);
			}
			return result;
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x002E7C3C File Offset: 0x002E603C
		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			GlobalTargetInfo result;
			if (!this.IsValid)
			{
				result = GlobalTargetInfo.Invalid;
			}
			else if (this.Thing != null)
			{
				result = new GlobalTargetInfo(this.Thing);
			}
			else
			{
				result = new GlobalTargetInfo(this.Cell, map, false);
			}
			return result;
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x002E7C90 File Offset: 0x002E6090
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			bool result;
			if (a.Thing != null || b.Thing != null)
			{
				result = (a.Thing == b.Thing);
			}
			else
			{
				result = ((!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt);
			}
			return result;
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x002E7D10 File Offset: 0x002E6110
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x002E7D30 File Offset: 0x002E6130
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x002E7D64 File Offset: 0x002E6164
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x002E7D88 File Offset: 0x002E6188
		public override int GetHashCode()
		{
			int hashCode;
			if (this.thingInt != null)
			{
				hashCode = this.thingInt.GetHashCode();
			}
			else
			{
				hashCode = this.cellInt.GetHashCode();
			}
			return hashCode;
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x002E7DCC File Offset: 0x002E61CC
		public override string ToString()
		{
			string result;
			if (this.Thing != null)
			{
				result = this.Thing.GetUniqueLoadID();
			}
			else if (this.Cell.IsValid)
			{
				result = this.Cell.ToString();
			}
			else
			{
				result = "null";
			}
			return result;
		}

		// Token: 0x04003C91 RID: 15505
		private Thing thingInt;

		// Token: 0x04003C92 RID: 15506
		private IntVec3 cellInt;
	}
}
