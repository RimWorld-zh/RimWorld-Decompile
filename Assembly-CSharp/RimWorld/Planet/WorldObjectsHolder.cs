using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld.Planet
{
	public class WorldObjectsHolder : IExposable
	{
		private List<WorldObject> worldObjects = new List<WorldObject>();

		private HashSet<WorldObject> worldObjectsHashSet = new HashSet<WorldObject>();

		private List<Caravan> caravans = new List<Caravan>();

		private List<Settlement> settlements = new List<Settlement>();

		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		private List<SettlementBase> settlementBases = new List<SettlementBase>();

		private List<DestroyedSettlement> destroyedSettlements = new List<DestroyedSettlement>();

		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		private List<MapParent> mapParents = new List<MapParent>();

		private List<Site> sites = new List<Site>();

		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		[CompilerGenerated]
		private static Predicate<WorldObject> <>f__am$cache0;

		public WorldObjectsHolder()
		{
		}

		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		public List<SettlementBase> SettlementBases
		{
			get
			{
				return this.settlementBases;
			}
		}

		public List<DestroyedSettlement> DestroyedSettlements
		{
			get
			{
				return this.destroyedSettlements;
			}
		}

		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		public int PlayerControlledCaravansCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.caravans.Count; i++)
				{
					if (this.caravans[i].IsPlayerControlled)
					{
						num++;
					}
				}
				return num;
			}
		}

		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
				for (int i = this.worldObjects.Count - 1; i >= 0; i--)
				{
					if (!this.worldObjects[i].def.saved)
					{
						WorldObjectsHolder.tmpUnsavedWorldObjects.Add(this.worldObjects[i]);
						this.worldObjects.RemoveAt(i);
					}
				}
			}
			Scribe_Collections.Look<WorldObject>(ref this.worldObjects, "worldObjects", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.worldObjects.AddRange(WorldObjectsHolder.tmpUnsavedWorldObjects);
				WorldObjectsHolder.tmpUnsavedWorldObjects.Clear();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.worldObjects.RemoveAll((WorldObject wo) => wo == null);
				for (int j = 0; j < this.worldObjects.Count; j++)
				{
					this.worldObjects[j].SpawnSetup();
				}
				this.Recache();
			}
		}

		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				Log.Error("Tried to add world object " + o + " to world, but it's already here.", false);
			}
			else
			{
				if (o.Tile < 0)
				{
					Log.Error("Tried to add world object " + o + " but its tile is not set. Setting to 0.", false);
					o.Tile = 0;
				}
				this.worldObjects.Add(o);
				this.AddToCache(o);
				o.SpawnSetup();
				o.PostAdd();
			}
		}

		public void Remove(WorldObject o)
		{
			if (!this.worldObjects.Contains(o))
			{
				Log.Error("Tried to remove world object " + o + " from world, but it's not here.", false);
			}
			else
			{
				this.worldObjects.Remove(o);
				this.RemoveFromCache(o);
				o.PostRemove();
			}
		}

		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].Tick();
			}
		}

		private void AddToCache(WorldObject o)
		{
			this.worldObjectsHashSet.Add(o);
			if (o is Caravan)
			{
				this.caravans.Add((Caravan)o);
			}
			if (o is Settlement)
			{
				this.settlements.Add((Settlement)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Add((TravelingTransportPods)o);
			}
			if (o is SettlementBase)
			{
				this.settlementBases.Add((SettlementBase)o);
			}
			if (o is DestroyedSettlement)
			{
				this.destroyedSettlements.Add((DestroyedSettlement)o);
			}
			if (o is RoutePlannerWaypoint)
			{
				this.routePlannerWaypoints.Add((RoutePlannerWaypoint)o);
			}
			if (o is MapParent)
			{
				this.mapParents.Add((MapParent)o);
			}
			if (o is Site)
			{
				this.sites.Add((Site)o);
			}
			if (o is PeaceTalks)
			{
				this.peaceTalks.Add((PeaceTalks)o);
			}
		}

		private void RemoveFromCache(WorldObject o)
		{
			this.worldObjectsHashSet.Remove(o);
			if (o is Caravan)
			{
				this.caravans.Remove((Caravan)o);
			}
			if (o is Settlement)
			{
				this.settlements.Remove((Settlement)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Remove((TravelingTransportPods)o);
			}
			if (o is SettlementBase)
			{
				this.settlementBases.Remove((SettlementBase)o);
			}
			if (o is DestroyedSettlement)
			{
				this.destroyedSettlements.Remove((DestroyedSettlement)o);
			}
			if (o is RoutePlannerWaypoint)
			{
				this.routePlannerWaypoints.Remove((RoutePlannerWaypoint)o);
			}
			if (o is MapParent)
			{
				this.mapParents.Remove((MapParent)o);
			}
			if (o is Site)
			{
				this.sites.Remove((Site)o);
			}
			if (o is PeaceTalks)
			{
				this.peaceTalks.Remove((PeaceTalks)o);
			}
		}

		private void Recache()
		{
			this.worldObjectsHashSet.Clear();
			this.caravans.Clear();
			this.settlements.Clear();
			this.travelingTransportPods.Clear();
			this.settlementBases.Clear();
			this.destroyedSettlements.Clear();
			this.routePlannerWaypoints.Clear();
			this.mapParents.Clear();
			this.sites.Clear();
			this.peaceTalks.Clear();
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				this.AddToCache(this.worldObjects[i]);
			}
		}

		public bool Contains(WorldObject o)
		{
			return o != null && this.worldObjectsHashSet.Contains(o);
		}

		public IEnumerable<WorldObject> ObjectsAt(int tileID)
		{
			if (tileID < 0)
			{
				yield break;
			}
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tileID)
				{
					yield return this.worldObjects[i];
				}
			}
			yield break;
		}

		public bool AnyWorldObjectAt(int tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile)
				{
					return true;
				}
			}
			return false;
		}

		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		public T WorldObjectAt<T>(int tile) where T : WorldObject
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i] is T)
				{
					return this.worldObjects[i] as T;
				}
			}
			return (T)((object)null);
		}

		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		public WorldObject WorldObjectAt(int tile, WorldObjectDef def)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].Tile == tile && this.worldObjects[i].def == def)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		public Settlement SettlementAt(int tile)
		{
			for (int i = 0; i < this.settlements.Count; i++)
			{
				if (this.settlements[i].Tile == tile)
				{
					return this.settlements[i];
				}
			}
			return null;
		}

		public bool AnySettlementBaseAt(int tile)
		{
			return this.SettlementBaseAt(tile) != null;
		}

		public SettlementBase SettlementBaseAt(int tile)
		{
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (this.settlementBases[i].Tile == tile)
				{
					return this.settlementBases[i];
				}
			}
			return null;
		}

		public bool AnySiteAt(int tile)
		{
			return this.SiteAt(tile) != null;
		}

		public Site SiteAt(int tile)
		{
			for (int i = 0; i < this.sites.Count; i++)
			{
				if (this.sites[i].Tile == tile)
				{
					return this.sites[i];
				}
			}
			return null;
		}

		public bool AnyDestroyedSettlementAt(int tile)
		{
			return this.DestroyedSettlementAt(tile) != null;
		}

		public DestroyedSettlement DestroyedSettlementAt(int tile)
		{
			for (int i = 0; i < this.destroyedSettlements.Count; i++)
			{
				if (this.destroyedSettlements[i].Tile == tile)
				{
					return this.destroyedSettlements[i];
				}
			}
			return null;
		}

		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		public MapParent MapParentAt(int tile)
		{
			for (int i = 0; i < this.mapParents.Count; i++)
			{
				if (this.mapParents[i].Tile == tile)
				{
					return this.mapParents[i];
				}
			}
			return null;
		}

		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		public WorldObject WorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			for (int i = 0; i < this.worldObjects.Count; i++)
			{
				if (this.worldObjects[i].def == def && this.worldObjects[i].Tile == tile)
				{
					return this.worldObjects[i];
				}
			}
			return null;
		}

		public Caravan PlayerControlledCaravanAt(int tile)
		{
			for (int i = 0; i < this.caravans.Count; i++)
			{
				if (this.caravans[i].Tile == tile && this.caravans[i].IsPlayerControlled)
				{
					return this.caravans[i];
				}
			}
			return null;
		}

		public bool AnySettlementBaseAtOrAdjacent(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlementBases.Count; i++)
			{
				if (worldGrid.IsNeighborOrSame(this.settlementBases[i].Tile, tile))
				{
					return true;
				}
			}
			return false;
		}

		public RoutePlannerWaypoint RoutePlannerWaypointAt(int tile)
		{
			for (int i = 0; i < this.routePlannerWaypoints.Count; i++)
			{
				if (this.routePlannerWaypoints[i].Tile == tile)
				{
					return this.routePlannerWaypoints[i];
				}
			}
			return null;
		}

		public void GetPlayerControlledCaravansAt(int tile, List<Caravan> outCaravans)
		{
			outCaravans.Clear();
			for (int i = 0; i < this.caravans.Count; i++)
			{
				Caravan caravan = this.caravans[i];
				if (caravan.Tile == tile && caravan.IsPlayerControlled)
				{
					outCaravans.Add(caravan);
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WorldObjectsHolder()
		{
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(WorldObject wo)
		{
			return wo == null;
		}

		[CompilerGenerated]
		private sealed class <ObjectsAt>c__Iterator0 : IEnumerable, IEnumerable<WorldObject>, IEnumerator, IDisposable, IEnumerator<WorldObject>
		{
			internal int tileID;

			internal int <i>__1;

			internal WorldObjectsHolder $this;

			internal WorldObject $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ObjectsAt>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (tileID < 0)
					{
						return false;
					}
					i = 0;
					break;
				case 1u:
					IL_96:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.worldObjects.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.worldObjects[i].Tile == tileID)
					{
						this.$current = this.worldObjects[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_96;
				}
				return false;
			}

			WorldObject IEnumerator<WorldObject>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.Planet.WorldObject>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorldObject> IEnumerable<WorldObject>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObjectsHolder.<ObjectsAt>c__Iterator0 <ObjectsAt>c__Iterator = new WorldObjectsHolder.<ObjectsAt>c__Iterator0();
				<ObjectsAt>c__Iterator.$this = this;
				<ObjectsAt>c__Iterator.tileID = tileID;
				return <ObjectsAt>c__Iterator;
			}
		}
	}
}
