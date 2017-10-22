using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_JourneyOffer : IncidentWorker
	{
		private const int MinTraversalDistance = 200;

		private const int MaxTraversalDistance = 800;

		private List<int> possibleRootTiles = new List<int>();

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			this.FindPossibleRootTiles();
			return this.possibleRootTiles.Any();
		}

		public override bool TryExecute(IncidentParms parms)
		{
			this.FindPossibleRootTiles();
			if (!this.possibleRootTiles.Any())
			{
				return false;
			}
			int tile = default(int);
			if (!this.TryFindDestinationTile(this.possibleRootTiles.RandomElement(), out tile))
			{
				return false;
			}
			WorldObject journeyDestination = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.JourneyDestination);
			journeyDestination.Tile = tile;
			Find.WorldObjects.Add(journeyDestination);
			DiaNode diaNode = new DiaNode("JourneyOffer".Translate());
			DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
			diaOption.action = (Action)delegate
			{
				CameraJumper.TryJumpAndSelect(journeyDestination);
			};
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			DiaOption diaOption2 = new DiaOption("OK".Translate());
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, true, (string)null));
			return true;
		}

		private void FindPossibleRootTiles()
		{
			this.possibleRootTiles.Clear();
			List<Map> maps = Find.Maps;
			int num = default(int);
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && this.TryFindDestinationTileActual(maps[i].Tile, 200, out num))
				{
					this.possibleRootTiles.Add(maps[i].Tile);
				}
			}
			if (!this.possibleRootTiles.Any())
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled && this.TryFindDestinationTileActual(caravans[j].Tile, 200, out num))
					{
						this.possibleRootTiles.Add(caravans[j].Tile);
					}
				}
			}
		}

		private bool TryFindDestinationTile(int rootTile, out int tile)
		{
			int num = 800;
			for (int i = 0; i < 1000; i++)
			{
				num = (int)((float)num * Rand.Range(0.5f, 0.75f));
				if (num <= 200)
				{
					num = 200;
				}
				if (this.TryFindDestinationTileActual(rootTile, num, out tile))
				{
					return true;
				}
				if ((float)num <= 200.00100708007812)
				{
					return false;
				}
			}
			tile = -1;
			return false;
		}

		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			return TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (Predicate<int>)((int x) => !Find.WorldObjects.AnyWorldObjectAt(x)), true);
		}
	}
}
