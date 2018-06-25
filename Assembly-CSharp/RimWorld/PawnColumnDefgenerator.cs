using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000236 RID: 566
	public class PawnColumnDefgenerator
	{
		// Token: 0x06000A3A RID: 2618 RVA: 0x0005A458 File Offset: 0x00058858
		public static IEnumerable<PawnColumnDef> ImpliedPawnColumnDefs()
		{
			PawnTableDef animalsTable = PawnTableDefOf.Animals;
			foreach (TrainableDef sourceDef in from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td)
			{
				PawnColumnDef d3 = new PawnColumnDef();
				d3.defName = "Trainable_" + sourceDef.defName;
				d3.trainable = sourceDef;
				d3.headerIcon = sourceDef.icon;
				d3.workerClass = typeof(PawnColumnWorker_Trainable);
				d3.sortable = true;
				d3.headerTip = sourceDef.LabelCap;
				d3.paintable = true;
				d3.modContentPack = sourceDef.modContentPack;
				animalsTable.columns.Insert(animalsTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, d3);
				yield return d3;
			}
			PawnTableDef workTable = PawnTableDefOf.Work;
			bool moveWorkTypeLabelDown = false;
			foreach (WorkTypeDef def in (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where d.visible
			select d).Reverse<WorkTypeDef>())
			{
				moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
				PawnColumnDef d2 = new PawnColumnDef();
				d2.defName = "WorkPriority_" + def.defName;
				d2.workType = def;
				d2.moveWorkTypeLabelDown = moveWorkTypeLabelDown;
				d2.workerClass = typeof(PawnColumnWorker_WorkPriority);
				d2.sortable = true;
				d2.modContentPack = def.modContentPack;
				workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, d2);
				yield return d2;
			}
			yield break;
		}
	}
}
