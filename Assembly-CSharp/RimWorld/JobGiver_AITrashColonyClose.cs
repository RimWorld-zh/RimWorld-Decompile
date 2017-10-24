using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_AITrashColonyClose : ThinkNode_JobGiver
	{
		private const int CloseSearchRadius = 5;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			IntVec3 randomCell;
			Building edifice;
			Plant plant;
			if (!pawn.HostileTo(Faction.OfPlayer))
			{
				result = null;
			}
			else
			{
				bool flag = pawn.natives.IgniteVerb != null && pawn.HostileTo(Faction.OfPlayer);
				CellRect cellRect = CellRect.CenteredOn(pawn.Position, 5);
				for (int i = 0; i < 35; i++)
				{
					randomCell = cellRect.RandomCell;
					if (randomCell.InBounds(pawn.Map))
					{
						edifice = randomCell.GetEdifice(pawn.Map);
						if (edifice != null && TrashUtility.ShouldTrashBuilding(pawn, edifice, false) && GenSight.LineOfSight(pawn.Position, randomCell, pawn.Map, false, null, 0, 0))
							goto IL_00ac;
						if (flag)
						{
							plant = randomCell.GetPlant(pawn.Map);
							if (plant != null && TrashUtility.ShouldTrashPlant(pawn, plant) && GenSight.LineOfSight(pawn.Position, randomCell, pawn.Map, false, null, 0, 0))
								goto IL_0138;
						}
						if (DebugViewSettings.drawDestSearch && Find.VisibleMap == pawn.Map)
						{
							Find.VisibleMap.debugDrawer.FlashCell(randomCell, 0f, "trash no", 50);
						}
					}
				}
				result = null;
			}
			goto IL_01ca;
			IL_01ca:
			return result;
			IL_00ac:
			if (DebugViewSettings.drawDestSearch && Find.VisibleMap == pawn.Map)
			{
				Find.VisibleMap.debugDrawer.FlashCell(randomCell, 1f, "trash bld", 50);
			}
			result = TrashUtility.TrashJob(pawn, edifice);
			goto IL_01ca;
			IL_0138:
			if (DebugViewSettings.drawDestSearch && Find.VisibleMap == pawn.Map)
			{
				Find.VisibleMap.debugDrawer.FlashCell(randomCell, 0.5f, "trash plant", 50);
			}
			result = TrashUtility.TrashJob(pawn, plant);
			goto IL_01ca;
		}
	}
}
