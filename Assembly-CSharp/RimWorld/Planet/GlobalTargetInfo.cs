using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000577 RID: 1399
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		// Token: 0x04000F89 RID: 3977
		private Thing thingInt;

		// Token: 0x04000F8A RID: 3978
		private IntVec3 cellInt;

		// Token: 0x04000F8B RID: 3979
		private Map mapInt;

		// Token: 0x04000F8C RID: 3980
		private WorldObject worldObjectInt;

		// Token: 0x04000F8D RID: 3981
		private int tileInt;

		// Token: 0x04000F8E RID: 3982
		public const char WorldObjectLoadIDMarker = '@';

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000E5F50 File Offset: 0x000E4350
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000E5F7C File Offset: 0x000E437C
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

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000E5FE0 File Offset: 0x000E43E0
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = -1;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000E600A File Offset: 0x000E440A
		public GlobalTargetInfo(int tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001AB6 RID: 6838 RVA: 0x000E6034 File Offset: 0x000E4434
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid || this.worldObjectInt != null || this.tileInt >= 0;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x000E6080 File Offset: 0x000E4480
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x000E60B0 File Offset: 0x000E44B0
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt >= 0;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x000E60E0 File Offset: 0x000E44E0
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x000E6104 File Offset: 0x000E4504
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x000E6120 File Offset: 0x000E4520
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x000E6150 File Offset: 0x000E4550
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x000E6174 File Offset: 0x000E4574
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x000E6190 File Offset: 0x000E4590
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x000E61B4 File Offset: 0x000E45B4
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

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x000E61EC File Offset: 0x000E45EC
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

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000E6224 File Offset: 0x000E4624
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

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000E62C8 File Offset: 0x000E46C8
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

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000E6310 File Offset: 0x000E4710
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000E632C File Offset: 0x000E472C
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000E6348 File Offset: 0x000E4748
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

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000E640C File Offset: 0x000E480C
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

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000E64D8 File Offset: 0x000E48D8
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

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000E6574 File Offset: 0x000E4974
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

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000E6618 File Offset: 0x000E4A18
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

		// Token: 0x06001ACA RID: 6858 RVA: 0x000E66BC File Offset: 0x000E4ABC
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

		// Token: 0x06001ACB RID: 6859 RVA: 0x000E67B4 File Offset: 0x000E4BB4
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000E67D4 File Offset: 0x000E4BD4
		public override bool Equals(object obj)
		{
			return obj is GlobalTargetInfo && this.Equals((GlobalTargetInfo)obj);
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000E6808 File Offset: 0x000E4C08
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000E682C File Offset: 0x000E4C2C
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

		// Token: 0x06001ACF RID: 6863 RVA: 0x000E68C4 File Offset: 0x000E4CC4
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
