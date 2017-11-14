using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class PawnColumnDefgenerator
	{
		public static IEnumerable<PawnColumnDef> ImpliedPawnColumnDefs()
		{
			PawnTableDef animalsTable = PawnTableDefOf.Animals;
			using (IEnumerator<TrainableDef> enumerator = (from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					TrainableDef sourceDef = enumerator.Current;
					PawnColumnDef d3 = new PawnColumnDef
					{
						defName = "Trainable_" + sourceDef.defName,
						trainable = sourceDef,
						headerIcon = sourceDef.icon,
						workerClass = typeof(PawnColumnWorker_Trainable),
						sortable = true,
						headerTip = sourceDef.LabelCap
					};
					animalsTable.columns.Insert(animalsTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, d3);
					yield return d3;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			PawnTableDef workTable = PawnTableDefOf.Work;
			bool moveWorkTypeLabelDown2 = false;
			using (IEnumerator<WorkTypeDef> enumerator2 = (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where d.visible
			select d).Reverse().GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					WorkTypeDef def = enumerator2.Current;
					moveWorkTypeLabelDown2 = !moveWorkTypeLabelDown2;
					PawnColumnDef d2 = new PawnColumnDef
					{
						defName = "WorkPriority_" + def.defName,
						workType = def,
						moveWorkTypeLabelDown = moveWorkTypeLabelDown2,
						workerClass = typeof(PawnColumnWorker_WorkPriority),
						sortable = true
					};
					workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, d2);
					yield return d2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0334:
			/*Error near IL_0335: Unexpected return in MoveNext()*/;
		}
	}
}
