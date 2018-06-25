using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000355 RID: 853
	public class IncidentWorker_QuestJourneyOffer : IncidentWorker
	{
		// Token: 0x04000908 RID: 2312
		private const int MinTraversalDistance = 180;

		// Token: 0x04000909 RID: 2313
		private const int MaxTraversalDistance = 800;

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0007C70C File Offset: 0x0007AB0C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return this.TryFindRootTile(out num);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0007C72C File Offset: 0x0007AB2C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int rootTile;
			bool result;
			int tile;
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
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(journeyDestination);
				};
				diaOption.resolveTree = true;
				diaNode.options.Add(diaOption);
				DiaOption diaOption2 = new DiaOption("OK".Translate());
				diaOption2.resolveTree = true;
				diaNode.options.Add(diaOption2);
				Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, true, null));
				Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
				result = true;
			}
			return result;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0007C840 File Offset: 0x0007AC40
		private bool TryFindRootTile(out int tile)
		{
			int unused;
			return TileFinder.TryFindRandomPlayerTile(out tile, false, (int x) => this.TryFindDestinationTileActual(x, 180, out unused));
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0007C878 File Offset: 0x0007AC78
		private bool TryFindDestinationTile(int rootTile, out int tile)
		{
			int num = 800;
			int i = 0;
			while (i < 1000)
			{
				num = (int)((float)num * Rand.Range(0.5f, 0.75f));
				if (num <= 180)
				{
					num = 180;
				}
				bool result;
				if (this.TryFindDestinationTileActual(rootTile, num, out tile))
				{
					result = true;
				}
				else
				{
					if (num > 180)
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			tile = -1;
			return false;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0007C8FC File Offset: 0x0007ACFC
		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			return TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].biome.canBuildBase && Find.WorldGrid[x].biome.canAutoChoose, true, true);
		}
	}
}
