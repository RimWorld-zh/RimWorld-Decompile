using System;
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
			foreach (TrainableDef item in from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td)
			{
				PawnColumnDef d3 = new PawnColumnDef
				{
					defName = "Trainable_" + item.defName,
					trainable = item,
					headerIcon = item.icon,
					workerClass = typeof(PawnColumnWorker_Trainable),
					sortable = true,
					headerTip = item.LabelCap
				};
				animalsTable.columns.Insert(animalsTable.columns.FindIndex((Predicate<PawnColumnDef>)((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox)) - 1, d3);
				yield return d3;
			}
			PawnTableDef workTable = PawnTableDefOf.Work;
			bool moveWorkTypeLabelDown = false;
			foreach (WorkTypeDef item2 in (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where d.visible
			select d).Reverse())
			{
				moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
				PawnColumnDef d2 = new PawnColumnDef
				{
					defName = "WorkPriority_" + item2.defName,
					workType = item2,
					moveWorkTypeLabelDown = moveWorkTypeLabelDown,
					workerClass = typeof(PawnColumnWorker_WorkPriority),
					sortable = true
				};
				workTable.columns.Insert(workTable.columns.FindIndex((Predicate<PawnColumnDef>)((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities)) + 1, d2);
				yield return d2;
			}
		}
	}
}
