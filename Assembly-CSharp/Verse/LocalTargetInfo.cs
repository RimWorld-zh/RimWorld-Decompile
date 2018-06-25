using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		private Thing thingInt;

		private IntVec3 cellInt;

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
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165, false);
			}
			return targ.Cell;
		}

		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672, false);
			}
			return targ.thingInt;
		}

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
