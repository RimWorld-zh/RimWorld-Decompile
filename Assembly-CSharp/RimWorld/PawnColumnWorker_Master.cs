using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		public PawnColumnWorker_Master()
		{
		}

		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (this.CanAssignMaster(pawn))
			{
				Rect rect2 = rect.ContractedBy(2f);
				TrainableUtility.MasterSelectButton(rect2, pawn, true);
			}
		}

		public override int Compare(Pawn a, Pawn b)
		{
			int valueToCompare = this.GetValueToCompare1(a);
			int valueToCompare2 = this.GetValueToCompare1(b);
			int result;
			if (valueToCompare != valueToCompare2)
			{
				result = valueToCompare.CompareTo(valueToCompare2);
			}
			else
			{
				result = this.GetValueToCompare2(a).CompareTo(this.GetValueToCompare2(b));
			}
			return result;
		}

		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		private int GetValueToCompare1(Pawn pawn)
		{
			int result;
			if (!this.CanAssignMaster(pawn))
			{
				result = 0;
			}
			else if (pawn.playerSettings.Master == null)
			{
				result = 1;
			}
			else
			{
				result = 2;
			}
			return result;
		}

		private string GetValueToCompare2(Pawn pawn)
		{
			string result;
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				result = pawn.playerSettings.Master.Label;
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
}
