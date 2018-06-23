using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000353 RID: 851
	public class IncidentWorker_QuestJourneyOffer : IncidentWorker
	{
		// Token: 0x04000908 RID: 2312
		private const int MinTraversalDistance = 180;

		// Token: 0x04000909 RID: 2313
		private const int MaxTraversalDistance = 800;

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0007C5BC File Offset: 0x0007A9BC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return this.TryFindRootTile(out num);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0007C5DC File Offset: 0x0007A9DC
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

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0007C6F0 File Offset: 0x0007AAF0
		private bool TryFindRootTile(out int tile)
		{
			int unused;
			return TileFinder.TryFindRandomPlayerTile(out tile, false, (int x) => this.TryFindDestinationTileActual(x, 180, out unused));
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0007C728 File Offset: 0x0007AB28
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

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0007C7AC File Offset: 0x0007ABAC
		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			return TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].biome.canBuildBase && Find.WorldGrid[x].biome.canAutoChoose, true, true);
		}
	}
}
