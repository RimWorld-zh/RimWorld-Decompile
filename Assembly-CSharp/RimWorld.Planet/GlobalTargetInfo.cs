using System;
using Verse;

namespace RimWorld.Planet
{
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		private Thing thingInt;

		private IntVec3 cellInt;

		private Map mapInt;

		private WorldObject worldObjectInt;

		private int tileInt;

		public const char WorldObjectLoadIDMarker = '@';

		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid || this.worldObjectInt != null || this.tileInt >= 0;
			}
		}

		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt >= 0;
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

		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		public IntVec3 Cell
		{
			get
			{
				return (this.thingInt == null) ? this.cellInt : this.thingInt.PositionHeld;
			}
		}

		public Map Map
		{
			get
			{
				return (this.thingInt == null) ? this.mapInt : this.thingInt.MapHeld;
			}
		}

		public int Tile
		{
			get
			{
				return (this.worldObjectInt == null) ? ((this.tileInt < 0) ? ((this.thingInt == null || this.thingInt.Tile < 0) ? ((!this.cellInt.IsValid || this.mapInt == null) ? (-1) : this.mapInt.Tile) : this.thingInt.Tile) : this.tileInt) : this.worldObjectInt.Tile;
			}
		}

		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		public GlobalTargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				Log.Warning("Constructed GlobalTargetInfo with cell=" + cell + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = -1;
		}

		public GlobalTargetInfo(int tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			return (!target.HasThing) ? new GlobalTargetInfo(target.Cell, target.Map, false) : new GlobalTargetInfo(target.Thing);
		}

		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		public static explicit operator LocalTargetInfo(GlobalTargetInfo targ)
		{
			LocalTargetInfo result;
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject " + targ.worldObjectInt, 134566);
				result = LocalTargetInfo.Invalid;
			}
			else if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt, 7833122);
				result = LocalTargetInfo.Invalid;
			}
			else
			{
				result = (targ.IsValid ? ((targ.thingInt == null) ? new LocalTargetInfo(targ.cellInt) : new LocalTargetInfo(targ.thingInt)) : LocalTargetInfo.Invalid);
			}
			return result;
		}

		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			TargetInfo result;
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had WorldObject " + targ.worldObjectInt, 134566);
				result = TargetInfo.Invalid;
			}
			else if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt, 7833122);
				result = TargetInfo.Invalid;
			}
			else
			{
				result = (targ.IsValid ? ((targ.thingInt == null) ? new TargetInfo(targ.cellInt, targ.mapInt, false) : new TargetInfo(targ.thingInt)) : TargetInfo.Invalid);
			}
			return result;
		}

		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had WorldObject " + targ.worldObjectInt, 134566);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt, 7833122);
			}
			return targ.Cell;
		}

		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had cell " + targ.cellInt, 631672);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had WorldObject " + targ.worldObjectInt, 134566);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt, 7833122);
			}
			return targ.thingInt;
		}

		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had Thing " + targ.thingInt, 6324165);
			}
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had cell " + targ.cellInt, 631672);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt, 7833122);
			}
			return targ.worldObjectInt;
		}

		public static bool operator ==(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return (a.Thing != null || b.Thing != null) ? (a.Thing == b.Thing) : ((a.cellInt.IsValid || b.cellInt.IsValid) ? (a.cellInt == b.cellInt && a.mapInt == b.mapInt) : ((a.WorldObject != null || b.WorldObject != null) ? (a.WorldObject == b.WorldObject) : ((a.tileInt < 0 && b.tileInt < 0) || a.tileInt == b.tileInt)));
		}

		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return obj is GlobalTargetInfo && this.Equals((GlobalTargetInfo)obj);
		}

		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return (this.thingInt == null) ? ((!this.cellInt.IsValid) ? ((this.worldObjectInt == null) ? ((this.tileInt < 0) ? (-1) : this.tileInt) : this.worldObjectInt.GetHashCode()) : Gen.HashCombine(this.cellInt.GetHashCode(), this.mapInt)) : this.thingInt.GetHashCode();
		}

		public override string ToString()
		{
			return (this.thingInt == null) ? ((!this.cellInt.IsValid) ? ((this.worldObjectInt == null) ? ((this.tileInt < 0) ? "null" : this.tileInt.ToString()) : ('@' + this.worldObjectInt.GetUniqueLoadID())) : (this.cellInt.ToString() + ", " + ((this.mapInt == null) ? "null" : this.mapInt.GetUniqueLoadID()))) : this.thingInt.GetUniqueLoadID();
		}
	}
}
