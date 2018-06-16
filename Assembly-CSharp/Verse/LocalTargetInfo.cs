using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF1 RID: 3825
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x06005B11 RID: 23313 RVA: 0x002E7874 File Offset: 0x002E5C74
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x06005B12 RID: 23314 RVA: 0x002E7889 File Offset: 0x002E5C89
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06005B13 RID: 23315 RVA: 0x002E789C File Offset: 0x002E5C9C
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06005B14 RID: 23316 RVA: 0x002E78CC File Offset: 0x002E5CCC
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005B15 RID: 23317 RVA: 0x002E78F0 File Offset: 0x002E5CF0
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06005B16 RID: 23318 RVA: 0x002E790C File Offset: 0x002E5D0C
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06005B17 RID: 23319 RVA: 0x002E793C File Offset: 0x002E5D3C
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06005B18 RID: 23320 RVA: 0x002E795C File Offset: 0x002E5D5C
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

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005B19 RID: 23321 RVA: 0x002E7994 File Offset: 0x002E5D94
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

		// Token: 0x06005B1A RID: 23322 RVA: 0x002E7A44 File Offset: 0x002E5E44
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x002E7A60 File Offset: 0x002E5E60
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x002E7A7C File Offset: 0x002E5E7C
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x002E7AC0 File Offset: 0x002E5EC0
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x002E7B10 File Offset: 0x002E5F10
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

		// Token: 0x06005B1F RID: 23327 RVA: 0x002E7B64 File Offset: 0x002E5F64
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

		// Token: 0x06005B20 RID: 23328 RVA: 0x002E7BB8 File Offset: 0x002E5FB8
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

		// Token: 0x06005B21 RID: 23329 RVA: 0x002E7C38 File Offset: 0x002E6038
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x002E7C58 File Offset: 0x002E6058
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x002E7C8C File Offset: 0x002E608C
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x002E7CB0 File Offset: 0x002E60B0
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

		// Token: 0x06005B25 RID: 23333 RVA: 0x002E7CF4 File Offset: 0x002E60F4
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

		// Token: 0x04003C92 RID: 15506
		private Thing thingInt;

		// Token: 0x04003C93 RID: 15507
		private IntVec3 cellInt;
	}
}
