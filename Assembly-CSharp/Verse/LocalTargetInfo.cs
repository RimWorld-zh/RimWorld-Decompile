using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF3 RID: 3827
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x04003CAC RID: 15532
		private Thing thingInt;

		// Token: 0x04003CAD RID: 15533
		private IntVec3 cellInt;

		// Token: 0x06005B3A RID: 23354 RVA: 0x002E9CC0 File Offset: 0x002E80C0
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x002E9CD5 File Offset: 0x002E80D5
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005B3C RID: 23356 RVA: 0x002E9CE8 File Offset: 0x002E80E8
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06005B3D RID: 23357 RVA: 0x002E9D18 File Offset: 0x002E8118
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06005B3E RID: 23358 RVA: 0x002E9D3C File Offset: 0x002E813C
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x002E9D58 File Offset: 0x002E8158
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005B40 RID: 23360 RVA: 0x002E9D88 File Offset: 0x002E8188
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06005B41 RID: 23361 RVA: 0x002E9DA8 File Offset: 0x002E81A8
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

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06005B42 RID: 23362 RVA: 0x002E9DE0 File Offset: 0x002E81E0
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

		// Token: 0x06005B43 RID: 23363 RVA: 0x002E9E90 File Offset: 0x002E8290
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x002E9EAC File Offset: 0x002E82AC
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x002E9EC8 File Offset: 0x002E82C8
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x002E9F0C File Offset: 0x002E830C
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x002E9F5C File Offset: 0x002E835C
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

		// Token: 0x06005B48 RID: 23368 RVA: 0x002E9FB0 File Offset: 0x002E83B0
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

		// Token: 0x06005B49 RID: 23369 RVA: 0x002EA004 File Offset: 0x002E8404
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

		// Token: 0x06005B4A RID: 23370 RVA: 0x002EA084 File Offset: 0x002E8484
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x002EA0A4 File Offset: 0x002E84A4
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x002EA0D8 File Offset: 0x002E84D8
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x002EA0FC File Offset: 0x002E84FC
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

		// Token: 0x06005B4E RID: 23374 RVA: 0x002EA140 File Offset: 0x002E8540
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
	}
}
