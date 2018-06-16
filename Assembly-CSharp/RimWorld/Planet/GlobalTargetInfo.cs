using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000579 RID: 1401
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		// Token: 0x06001AB7 RID: 6839 RVA: 0x000E5AD8 File Offset: 0x000E3ED8
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = -1;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000E5B04 File Offset: 0x000E3F04
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

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000E5B68 File Offset: 0x000E3F68
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = -1;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000E5B92 File Offset: 0x000E3F92
		public GlobalTargetInfo(int tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x000E5BBC File Offset: 0x000E3FBC
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid || this.worldObjectInt != null || this.tileInt >= 0;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x000E5C08 File Offset: 0x000E4008
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x000E5C38 File Offset: 0x000E4038
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt >= 0;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x000E5C68 File Offset: 0x000E4068
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x000E5C8C File Offset: 0x000E408C
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x000E5CA8 File Offset: 0x000E40A8
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000E5CD8 File Offset: 0x000E40D8
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x000E5CFC File Offset: 0x000E40FC
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x000E5D18 File Offset: 0x000E4118
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x000E5D3C File Offset: 0x000E413C
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
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x000E5D74 File Offset: 0x000E4174
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
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x000E5DAC File Offset: 0x000E41AC
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

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000E5E50 File Offset: 0x000E4250
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

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000E5E98 File Offset: 0x000E4298
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000E5EB4 File Offset: 0x000E42B4
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x000E5ED0 File Offset: 0x000E42D0
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

		// Token: 0x06001ACB RID: 6859 RVA: 0x000E5F94 File Offset: 0x000E4394
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

		// Token: 0x06001ACC RID: 6860 RVA: 0x000E6060 File Offset: 0x000E4460
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

		// Token: 0x06001ACD RID: 6861 RVA: 0x000E60FC File Offset: 0x000E44FC
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

		// Token: 0x06001ACE RID: 6862 RVA: 0x000E61A0 File Offset: 0x000E45A0
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

		// Token: 0x06001ACF RID: 6863 RVA: 0x000E6244 File Offset: 0x000E4644
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

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000E633C File Offset: 0x000E473C
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000E635C File Offset: 0x000E475C
		public override bool Equals(object obj)
		{
			return obj is GlobalTargetInfo && this.Equals((GlobalTargetInfo)obj);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000E6390 File Offset: 0x000E4790
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000E63B4 File Offset: 0x000E47B4
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

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000E644C File Offset: 0x000E484C
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

		// Token: 0x04000F88 RID: 3976
		private Thing thingInt;

		// Token: 0x04000F89 RID: 3977
		private IntVec3 cellInt;

		// Token: 0x04000F8A RID: 3978
		private Map mapInt;

		// Token: 0x04000F8B RID: 3979
		private WorldObject worldObjectInt;

		// Token: 0x04000F8C RID: 3980
		private int tileInt;

		// Token: 0x04000F8D RID: 3981
		public const char WorldObjectLoadIDMarker = '@';
	}
}
