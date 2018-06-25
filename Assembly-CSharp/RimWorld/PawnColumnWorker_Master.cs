using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000894 RID: 2196
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x001B025C File Offset: 0x001AE65C
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x001B0274 File Offset: 0x001AE674
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x001B0298 File Offset: 0x001AE698
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x001B02C8 File Offset: 0x001AE6C8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (this.CanAssignMaster(pawn))
			{
				Rect rect2 = rect.ContractedBy(2f);
				TrainableUtility.MasterSelectButton(rect2, pawn, true);
			}
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x001B02FC File Offset: 0x001AE6FC
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

		// Token: 0x06003223 RID: 12835 RVA: 0x001B034C File Offset: 0x001AE74C
		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x001B03A8 File Offset: 0x001AE7A8
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

		// Token: 0x06003225 RID: 12837 RVA: 0x001B03E8 File Offset: 0x001AE7E8
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
