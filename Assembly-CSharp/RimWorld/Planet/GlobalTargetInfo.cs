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
				Log.Warning("Constructed GlobalTargetInfo with cell=" + cell + " and a null map.", false);
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

		public int Tile
		{
			get
			{
				int result;
				if (this.worldObjectInt != null)
				{
					result = this.worldObjectInt.Tile;
				}
				else if (this.tileInt >= 0)
				{
					result = this.tileInt;
				}
				else if (this.thingInt != null && this.thingInt.Tile >= 0)
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

		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			GlobalTargetInfo result;
			if (target.HasThing)
			{
				result = new GlobalTargetInfo(target.Thing);
			}
			else
			{
				result = new GlobalTargetInfo(target.Cell, target.Map, false);
			}
			return result;
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
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject " + targ.worldObjectInt, 134566, false);
				result = LocalTargetInfo.Invalid;
			}
			else if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt, 7833122, false);
				result = LocalTargetInfo.Invalid;
			}
			else if (!targ.IsValid)
			{
				result = LocalTargetInfo.Invalid;
			}
			else if (targ.thingInt != null)
			{
				result = new LocalTargetInfo(targ.thingInt);
			}
			else
			{
				result = new LocalTargetInfo(targ.cellInt);
			}
			return result;
		}

		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			TargetInfo result;
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had WorldObject " + targ.worldObjectInt, 134566, false);
				result = TargetInfo.Invalid;
			}
			else if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt, 7833122, false);
				result = TargetInfo.Invalid;
			}
			else if (!targ.IsValid)
			{
				result = TargetInfo.Invalid;
			}
			else if (targ.thingInt != null)
			{
				result = new TargetInfo(targ.thingInt);
			}
			else
			{
				result = new TargetInfo(targ.cellInt, targ.mapInt, false);
			}
			return result;
		}

		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had WorldObject " + targ.worldObjectInt, 134566, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.Cell;
		}

		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			if (targ.worldObjectInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had WorldObject " + targ.worldObjectInt, 134566, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.thingInt;
		}

		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had Thing " + targ.thingInt, 6324165, false);
			}
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had cell " + targ.cellInt, 631672, false);
			}
			if (targ.tileInt >= 0)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt, 7833122, false);
			}
			return targ.worldObjectInt;
		}

		public static bool operator ==(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			bool result;
			if (a.Thing != null || b.Thing != null)
			{
				result = (a.Thing == b.Thing);
			}
			else if (a.cellInt.IsValid || b.cellInt.IsValid)
			{
				result = (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
			}
			else if (a.WorldObject != null || b.WorldObject != null)
			{
				result = (a.WorldObject == b.WorldObject);
			}
			else
			{
				result = ((a.tileInt < 0 && b.tileInt < 0) || a.tileInt == b.tileInt);
			}
			return result;
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
			int result;
			if (this.thingInt != null)
			{
				result = this.thingInt.GetHashCode();
			}
			else if (this.cellInt.IsValid)
			{
				result = Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
			}
			else if (this.worldObjectInt != null)
			{
				result = this.worldObjectInt.GetHashCode();
			}
			else if (this.tileInt >= 0)
			{
				result = this.tileInt;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public override string ToString()
		{
			string result;
			if (this.thingInt != null)
			{
				result = this.thingInt.GetUniqueLoadID();
			}
			else if (this.cellInt.IsValid)
			{
				result = this.cellInt.ToString() + ", " + ((this.mapInt == null) ? "null" : this.mapInt.GetUniqueLoadID());
			}
			else if (this.worldObjectInt != null)
			{
				result = '@' + this.worldObjectInt.GetUniqueLoadID();
			}
			else if (this.tileInt >= 0)
			{
				result = this.tileInt.ToString();
			}
			else
			{
				result = "null";
			}
			return result;
		}
	}
}
