using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_Trainable : PawnColumnWorker
	{
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training == null)
			{
				return;
			}
			bool flag;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag || !canTrain.Accepted)
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f);
			TrainingCardUtility.DoTrainableCheckbox(rect2, pawn, this.def.trainable, canTrain, false, true);
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.training.IsCompleted(this.def.trainable))
			{
				return 4;
			}
			bool flag;
			AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
			if (!flag)
			{
				return 0;
			}
			if (!acceptanceReport.Accepted)
			{
				return 1;
			}
			if (!pawn.training.GetWanted(this.def.trainable))
			{
				return 2;
			}
			return 3;
		}
	}
}
