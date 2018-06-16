using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000896 RID: 2198
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06003220 RID: 12832 RVA: 0x001AFC04 File Offset: 0x001AE004
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x001AFC1C File Offset: 0x001AE01C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x001AFC40 File Offset: 0x001AE040
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x001AFC70 File Offset: 0x001AE070
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (this.CanAssignMaster(pawn))
			{
				Rect rect2 = rect.ContractedBy(2f);
				TrainableUtility.MasterSelectButton(rect2, pawn, true);
			}
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x001AFCA4 File Offset: 0x001AE0A4
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

		// Token: 0x06003225 RID: 12837 RVA: 0x001AFCF4 File Offset: 0x001AE0F4
		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x001AFD50 File Offset: 0x001AE150
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

		// Token: 0x06003227 RID: 12839 RVA: 0x001AFD90 File Offset: 0x001AE190
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
