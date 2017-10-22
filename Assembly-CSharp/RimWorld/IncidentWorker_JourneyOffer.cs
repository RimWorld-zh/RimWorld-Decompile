using RimWorld.Planet;
using System;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_JourneyOffer : IncidentWorker
	{
		private const int MinTraversalDistance = 200;

		private const int MaxTraversalDistance = 800;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num = default(int);
			return this.TryFindRootTile(out num);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int rootTile = default(int);
			bool result;
			int tile = default(int);
			if (!this.TryFindRootTile(out rootTile))
			{
				result = false;
			}
			else if (!this.TryFindDestinationTile(rootTile, out tile))
			{
				result = false;
			}
			else
			{
				WorldObject journeyDestination = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.EscapeShip);
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
				result = true;
			}
			return result;
		}

		private bool TryFindRootTile(out int tile)
		{
			int unused;
			return TileFinder.TryFindRandomPlayerTile(out tile, false, (Predicate<int>)((int x) => this.TryFindDestinationTileActual(x, 200, out unused)));
		}

		private bool TryFindDestinationTile(int rootTile, out int tile)
		{
			int num = 800;
			int num2 = 0;
			bool result;
			while (true)
			{
				if (num2 < 1000)
				{
					num = (int)((float)num * Rand.Range(0.5f, 0.75f));
					if (num <= 200)
					{
						num = 200;
					}
					if (this.TryFindDestinationTileActual(rootTile, num, out tile))
					{
						result = true;
						break;
					}
					if (num <= 200)
					{
						result = false;
						break;
					}
					num2++;
					continue;
				}
				tile = -1;
				result = false;
				break;
			}
			return result;
		}

		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			return TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (Predicate<int>)((int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].biome.canBuildBase && Find.WorldGrid[x].biome.canAutoChoose), true, true);
		}
	}
}
