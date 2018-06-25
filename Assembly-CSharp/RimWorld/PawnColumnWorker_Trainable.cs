using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000898 RID: 2200
	public class PawnColumnWorker_Trainable : PawnColumnWorker
	{
		// Token: 0x06003242 RID: 12866 RVA: 0x001B0CAC File Offset: 0x001AF0AC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training != null)
			{
				bool flag;
				AcceptanceReport canTrain = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
				if (flag && canTrain.Accepted)
				{
					int num = (int)((rect.width - 24f) / 2f);
					int num2 = Mathf.Max(3, 0);
					Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f);
					TrainingCardUtility.DoTrainableCheckbox(rect2, pawn, this.def.trainable, canTrain, false, true);
				}
			}
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x001B0D54 File Offset: 0x001AF154
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x001B0D78 File Offset: 0x001AF178
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x001B0DA0 File Offset: 0x001AF1A0
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x001B0DC4 File Offset: 0x001AF1C4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x001B0DF0 File Offset: 0x001AF1F0
		private int GetValueToCompare(Pawn pawn)
		{
			int result;
			if (pawn.training == null)
			{
				result = int.MinValue;
			}
			else if (pawn.training.HasLearned(this.def.trainable))
			{
				result = 4;
			}
			else
			{
				bool flag;
				AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(this.def.trainable, out flag);
				if (!flag)
				{
					result = 0;
				}
				else if (!acceptanceReport.Accepted)
				{
					result = 1;
				}
				else if (!pawn.training.GetWanted(this.def.trainable))
				{
					result = 2;
				}
				else
				{
					result = 3;
				}
			}
			return result;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x001B0E9C File Offset: 0x001AF29C
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].training != null && !pawnsListForReading[i].training.HasLearned(this.def.trainable))
					{
						bool flag;
						AcceptanceReport acceptanceReport = pawnsListForReading[i].training.CanAssignToTrain(this.def.trainable, out flag);
						if (flag && acceptanceReport.Accepted)
						{
							bool wanted = pawnsListForReading[i].training.GetWanted(this.def.trainable);
							if (Event.current.button == 0)
							{
								if (!wanted)
								{
									pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, true);
								}
							}
							else if (Event.current.button == 1)
							{
								if (wanted)
								{
									pawnsListForReading[i].training.SetWantedRecursive(this.def.trainable, false);
								}
							}
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x001B1018 File Offset: 0x001AF418
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}
	}
}
