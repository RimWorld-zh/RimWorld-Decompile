using RimWorld.Planet;
using System;
using UnityEngine;

namespace Verse
{
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		private Thing thingInt;

		private IntVec3 cellInt;

		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		public IntVec3 Cell
		{
			get
			{
				return (this.thingInt == null) ? this.cellInt : this.thingInt.PositionHeld;
			}
		}

		public Vector3 CenterVector3
		{
			get
			{
				return (this.thingInt == null) ? this.cellInt.ToVector3Shifted() : this.thingInt.DrawPos;
			}
		}

		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165);
			}
			return targ.Cell;
		}

		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672);
			}
			return targ.thingInt;
		}

		public TargetInfo ToTargetInfo(Map map)
		{
			return this.IsValid ? ((this.Thing == null) ? new TargetInfo(this.Cell, map, false) : new TargetInfo(this.Thing)) : TargetInfo.Invalid;
		}

		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			return this.IsValid ? ((this.Thing == null) ? new GlobalTargetInfo(this.Cell, map, false) : new GlobalTargetInfo(this.Thing)) : GlobalTargetInfo.Invalid;
		}

		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			return (a.Thing != null || b.Thing != null) ? (a.Thing == b.Thing) : ((!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt);
		}

		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return (this.thingInt == null) ? this.cellInt.GetHashCode() : this.thingInt.GetHashCode();
		}

		public override string ToString()
		{
			return (this.Thing == null) ? ((!this.Cell.IsValid) ? "null" : this.Cell.ToString()) : this.Thing.GetUniqueLoadID();
		}
	}
}
