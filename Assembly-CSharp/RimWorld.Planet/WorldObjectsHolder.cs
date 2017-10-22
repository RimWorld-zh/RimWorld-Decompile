using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class WorldObjectsHolder : IExposable
	{
		private List<WorldObject> worldObjects = new List<WorldObject>();

		private List<Caravan> caravans = new List<Caravan>();

		private List<FactionBase> factionBases = new List<FactionBase>();

		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		private List<Settlement> settlements = new List<Settlement>();

		private List<DestroyedFactionBase> destroyedFactionBases = new List<DestroyedFactionBase>();

		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		private List<MapParent> mapParents = new List<MapParent>();

		private List<Site> sites = new List<Site>();

		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

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

		public List<FactionBase> FactionBases
		{
			get
			{
				return this.factionBases;
			}
		}

		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		public List<DestroyedFactionBase> DestroyedFactionBases
		{
			get
			{
				return this.destroyedFactionBases;
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
				for (int num = this.worldObjects.Count - 1; num >= 0; num--)
				{
					if (!this.worldObjects[num].def.saved)
					{
						WorldObjectsHolder.tmpUnsavedWorldObjects.Add(this.worldObjects[num]);
						this.worldObjects.RemoveAt(num);
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
				this.worldObjects.RemoveAll((Predicate<WorldObject>)((WorldObject wo) => wo == null));
				for (int i = 0; i < this.worldObjects.Count; i++)
				{
					this.worldObjects[i].SpawnSetup();
				}
				this.Recache();
			}
		}

		public void Add(WorldObject o)
		{
			if (this.worldObjects.Contains(o))
			{
				Log.Error("Tried to add world object " + o + " to world, but it's already here.");
			}
			else
			{
				if (o.Tile < 0)
				{
					Log.Error("Tried to add world object " + o + " but its tile is not set. Setting to 0.");
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
				Log.Error("Tried to remove world object " + o + " from world, but it's not here.");
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
			if (o is Caravan)
			{
				this.caravans.Add((Caravan)o);
			}
			if (o is FactionBase)
			{
				this.factionBases.Add((FactionBase)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Add((TravelingTransportPods)o);
			}
			if (o is Settlement)
			{
				this.settlements.Add((Settlement)o);
			}
			if (o is DestroyedFactionBase)
			{
				this.destroyedFactionBases.Add((DestroyedFactionBase)o);
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
			if (o is Caravan)
			{
				this.caravans.Remove((Caravan)o);
			}
			if (o is FactionBase)
			{
				this.factionBases.Remove((FactionBase)o);
			}
			if (o is TravelingTransportPods)
			{
				this.travelingTransportPods.Remove((TravelingTransportPods)o);
			}
			if (o is Settlement)
			{
				this.settlements.Remove((Settlement)o);
			}
			if (o is DestroyedFactionBase)
			{
				this.destroyedFactionBases.Remove((DestroyedFactionBase)o);
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
			this.caravans.Clear();
			this.factionBases.Clear();
			this.travelingTransportPods.Clear();
			this.settlements.Clear();
			this.destroyedFactionBases.Clear();
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
			return o != null && ((!(o is Caravan)) ? ((!(o is Settlement)) ? this.worldObjects.Contains(o) : this.settlements.Contains((Settlement)o)) : this.caravans.Contains((Caravan)o));
		}

		public IEnumerable<WorldObject> ObjectsAt(int tileID)
		{
			if (tileID >= 0)
			{
				int i = 0;
				while (true)
				{
					if (i < this.worldObjects.Count)
					{
						if (this.worldObjects[i].Tile != tileID)
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return this.worldObjects[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public bool AnyWorldObjectAt(int tile)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.worldObjects.Count)
				{
					if (this.worldObjects[num].Tile == tile)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		public T WorldObjectAt<T>(int tile) where T : WorldObject
		{
			int num = 0;
			T result;
			while (true)
			{
				if (num < this.worldObjects.Count)
				{
					if (this.worldObjects[num].Tile == tile && this.worldObjects[num] is T)
					{
						result = (T)(this.worldObjects[num] as T);
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
		}

		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		public WorldObject WorldObjectAt(int tile, WorldObjectDef def)
		{
			int num = 0;
			WorldObject result;
			while (true)
			{
				if (num < this.worldObjects.Count)
				{
					if (this.worldObjects[num].Tile == tile && this.worldObjects[num].def == def)
					{
						result = this.worldObjects[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool AnyFactionBaseAt(int tile)
		{
			return this.FactionBaseAt(tile) != null;
		}

		public FactionBase FactionBaseAt(int tile)
		{
			int num = 0;
			FactionBase result;
			while (true)
			{
				if (num < this.factionBases.Count)
				{
					if (this.factionBases[num].Tile == tile)
					{
						result = this.factionBases[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		public Settlement SettlementAt(int tile)
		{
			int num = 0;
			Settlement result;
			while (true)
			{
				if (num < this.settlements.Count)
				{
					if (this.settlements[num].Tile == tile)
					{
						result = this.settlements[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool AnyDestroyedFactionBaseAt(int tile)
		{
			return this.DestroyedFactionBaseAt(tile) != null;
		}

		public DestroyedFactionBase DestroyedFactionBaseAt(int tile)
		{
			int num = 0;
			DestroyedFactionBase result;
			while (true)
			{
				if (num < this.destroyedFactionBases.Count)
				{
					if (this.destroyedFactionBases[num].Tile == tile)
					{
						result = this.destroyedFactionBases[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		public MapParent MapParentAt(int tile)
		{
			int num = 0;
			MapParent result;
			while (true)
			{
				if (num < this.mapParents.Count)
				{
					if (this.mapParents[num].Tile == tile)
					{
						result = this.mapParents[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		public WorldObject WorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			int num = 0;
			WorldObject result;
			while (true)
			{
				if (num < this.worldObjects.Count)
				{
					if (this.worldObjects[num].def == def && this.worldObjects[num].Tile == tile)
					{
						result = this.worldObjects[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public Caravan PlayerControlledCaravanAt(int tile)
		{
			int num = 0;
			Caravan result;
			while (true)
			{
				if (num < this.caravans.Count)
				{
					if (this.caravans[num].Tile == tile && this.caravans[num].IsPlayerControlled)
					{
						result = this.caravans[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool AnySettlementAtOrAdjacent(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.settlements.Count)
				{
					if (worldGrid.IsNeighborOrSame(this.settlements[num].Tile, tile))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public RoutePlannerWaypoint RoutePlannerWaypointAt(int tile)
		{
			int num = 0;
			RoutePlannerWaypoint result;
			while (true)
			{
				if (num < this.routePlannerWaypoints.Count)
				{
					if (this.routePlannerWaypoints[num].Tile == tile)
					{
						result = this.routePlannerWaypoints[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
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
	}
}
