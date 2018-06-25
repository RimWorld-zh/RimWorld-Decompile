using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062A RID: 1578
	public class WorldObjectsHolder : IExposable
	{
		// Token: 0x04001280 RID: 4736
		private List<WorldObject> worldObjects = new List<WorldObject>();

		// Token: 0x04001281 RID: 4737
		private List<Caravan> caravans = new List<Caravan>();

		// Token: 0x04001282 RID: 4738
		private List<FactionBase> factionBases = new List<FactionBase>();

		// Token: 0x04001283 RID: 4739
		private List<TravelingTransportPods> travelingTransportPods = new List<TravelingTransportPods>();

		// Token: 0x04001284 RID: 4740
		private List<Settlement> settlements = new List<Settlement>();

		// Token: 0x04001285 RID: 4741
		private List<DestroyedFactionBase> destroyedFactionBases = new List<DestroyedFactionBase>();

		// Token: 0x04001286 RID: 4742
		private List<RoutePlannerWaypoint> routePlannerWaypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04001287 RID: 4743
		private List<MapParent> mapParents = new List<MapParent>();

		// Token: 0x04001288 RID: 4744
		private List<Site> sites = new List<Site>();

		// Token: 0x04001289 RID: 4745
		private List<PeaceTalks> peaceTalks = new List<PeaceTalks>();

		// Token: 0x0400128A RID: 4746
		private static List<WorldObject> tmpUnsavedWorldObjects = new List<WorldObject>();

		// Token: 0x0400128B RID: 4747
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x00114144 File Offset: 0x00112544
		public List<WorldObject> AllWorldObjects
		{
			get
			{
				return this.worldObjects;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x00114160 File Offset: 0x00112560
		public List<Caravan> Caravans
		{
			get
			{
				return this.caravans;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x0011417C File Offset: 0x0011257C
		public List<FactionBase> FactionBases
		{
			get
			{
				return this.factionBases;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x00114198 File Offset: 0x00112598
		public List<TravelingTransportPods> TravelingTransportPods
		{
			get
			{
				return this.travelingTransportPods;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x001141B4 File Offset: 0x001125B4
		public List<Settlement> Settlements
		{
			get
			{
				return this.settlements;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002034 RID: 8244 RVA: 0x001141D0 File Offset: 0x001125D0
		public List<DestroyedFactionBase> DestroyedFactionBases
		{
			get
			{
				return this.destroyedFactionBases;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x001141EC File Offset: 0x001125EC
		public List<RoutePlannerWaypoint> RoutePlannerWaypoints
		{
			get
			{
				return this.routePlannerWaypoints;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002036 RID: 8246 RVA: 0x00114208 File Offset: 0x00112608
		public List<MapParent> MapParents
		{
			get
			{
				return this.mapParents;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002037 RID: 8247 RVA: 0x00114224 File Offset: 0x00112624
		public List<Site> Sites
		{
			get
			{
				return this.sites;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x00114240 File Offset: 0x00112640
		public List<PeaceTalks> PeaceTalks
		{
			get
			{
				return this.peaceTalks;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x0011425C File Offset: 0x0011265C
		public int WorldObjectsCount
		{
			get
			{
				return this.worldObjects.Count;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x0011427C File Offset: 0x0011267C
		public int CaravansCount
		{
			get
			{
				return this.caravans.Count;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x0011429C File Offset: 0x0011269C
		public int RoutePlannerWaypointsCount
		{
			get
			{
				return this.routePlannerWaypoints.Count;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x001142BC File Offset: 0x001126BC
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

		// Token: 0x0600203D RID: 8253 RVA: 0x0011430C File Offset: 0x0011270C
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

		// Token: 0x0600203E RID: 8254 RVA: 0x0011443C File Offset: 0x0011283C
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

		// Token: 0x0600203F RID: 8255 RVA: 0x001144C4 File Offset: 0x001128C4
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

		// Token: 0x06002040 RID: 8256 RVA: 0x0011451C File Offset: 0x0011291C
		public void WorldObjectsHolderTick()
		{
			WorldObjectsHolder.tmpWorldObjects.Clear();
			WorldObjectsHolder.tmpWorldObjects.AddRange(this.worldObjects);
			for (int i = 0; i < WorldObjectsHolder.tmpWorldObjects.Count; i++)
			{
				WorldObjectsHolder.tmpWorldObjects[i].Tick();
			}
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00114574 File Offset: 0x00112974
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

		// Token: 0x06002042 RID: 8258 RVA: 0x00114680 File Offset: 0x00112A80
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

		// Token: 0x06002043 RID: 8259 RVA: 0x00114794 File Offset: 0x00112B94
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

		// Token: 0x06002044 RID: 8260 RVA: 0x00114838 File Offset: 0x00112C38
		public bool Contains(WorldObject o)
		{
			bool result;
			if (o == null)
			{
				result = false;
			}
			else if (o is Caravan)
			{
				result = this.caravans.Contains((Caravan)o);
			}
			else if (o is Settlement)
			{
				result = this.settlements.Contains((Settlement)o);
			}
			else
			{
				result = this.worldObjects.Contains(o);
			}
			return result;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x001148AC File Offset: 0x00112CAC
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

		// Token: 0x06002046 RID: 8262 RVA: 0x001148E0 File Offset: 0x00112CE0
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

		// Token: 0x06002047 RID: 8263 RVA: 0x00114934 File Offset: 0x00112D34
		public bool AnyWorldObjectAt<T>(int tile) where T : WorldObject
		{
			return this.WorldObjectAt<T>(tile) != null;
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0011495C File Offset: 0x00112D5C
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

		// Token: 0x06002049 RID: 8265 RVA: 0x001149E0 File Offset: 0x00112DE0
		public bool AnyWorldObjectAt(int tile, WorldObjectDef def)
		{
			return this.WorldObjectAt(tile, def) != null;
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00114A04 File Offset: 0x00112E04
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

		// Token: 0x0600204B RID: 8267 RVA: 0x00114A78 File Offset: 0x00112E78
		public bool AnyFactionBaseAt(int tile)
		{
			return this.FactionBaseAt(tile) != null;
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00114A9C File Offset: 0x00112E9C
		public FactionBase FactionBaseAt(int tile)
		{
			for (int i = 0; i < this.factionBases.Count; i++)
			{
				if (this.factionBases[i].Tile == tile)
				{
					return this.factionBases[i];
				}
			}
			return null;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00114AFC File Offset: 0x00112EFC
		public bool AnySettlementAt(int tile)
		{
			return this.SettlementAt(tile) != null;
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00114B20 File Offset: 0x00112F20
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

		// Token: 0x0600204F RID: 8271 RVA: 0x00114B80 File Offset: 0x00112F80
		public bool AnySiteAt(int tile)
		{
			return this.SiteAt(tile) != null;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00114BA4 File Offset: 0x00112FA4
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

		// Token: 0x06002051 RID: 8273 RVA: 0x00114C04 File Offset: 0x00113004
		public bool AnyDestroyedFactionBaseAt(int tile)
		{
			return this.DestroyedFactionBaseAt(tile) != null;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00114C28 File Offset: 0x00113028
		public DestroyedFactionBase DestroyedFactionBaseAt(int tile)
		{
			for (int i = 0; i < this.destroyedFactionBases.Count; i++)
			{
				if (this.destroyedFactionBases[i].Tile == tile)
				{
					return this.destroyedFactionBases[i];
				}
			}
			return null;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00114C88 File Offset: 0x00113088
		public bool AnyMapParentAt(int tile)
		{
			return this.MapParentAt(tile) != null;
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x00114CAC File Offset: 0x001130AC
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

		// Token: 0x06002055 RID: 8277 RVA: 0x00114D0C File Offset: 0x0011310C
		public bool AnyWorldObjectOfDefAt(WorldObjectDef def, int tile)
		{
			return this.WorldObjectOfDefAt(def, tile) != null;
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00114D30 File Offset: 0x00113130
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

		// Token: 0x06002057 RID: 8279 RVA: 0x00114DA4 File Offset: 0x001131A4
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

		// Token: 0x06002058 RID: 8280 RVA: 0x00114E18 File Offset: 0x00113218
		public bool AnySettlementAtOrAdjacent(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < this.settlements.Count; i++)
			{
				if (worldGrid.IsNeighborOrSame(this.settlements[i].Tile, tile))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00114E78 File Offset: 0x00113278
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

		// Token: 0x0600205A RID: 8282 RVA: 0x00114ED8 File Offset: 0x001132D8
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
