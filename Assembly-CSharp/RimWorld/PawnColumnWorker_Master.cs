using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
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
				string label = TrainableUtility.MasterString(pawn).Truncate(rect2.width, null);
				if (Widgets.ButtonText(rect2, label, true, false, true))
				{
					TrainableUtility.OpenMasterSelectMenu(pawn);
				}
			}
		}

		public override int Compare(Pawn a, Pawn b)
		{
			int valueToCompare = this.GetValueToCompare1(a);
			int valueToCompare2 = this.GetValueToCompare1(b);
			return (valueToCompare == valueToCompare2) ? this.GetValueToCompare2(a).CompareTo(this.GetValueToCompare2(b)) : valueToCompare.CompareTo(valueToCompare2);
		}

		private bool CanAssignMaster(Pawn pawn)
		{
			return (byte)((pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer) ? (pawn.training.IsCompleted(TrainableDefOf.Obedience) ? 1 : 0) : 0) != 0;
		}

		private int GetValueToCompare1(Pawn pawn)
		{
			return this.CanAssignMaster(pawn) ? ((pawn.playerSettings.master == null) ? 1 : 2) : 0;
		}

		private string GetValueToCompare2(Pawn pawn)
		{
			return (pawn.playerSettings == null || pawn.playerSettings.master == null) ? "" : pawn.playerSettings.master.Label;
		}
	}
}
