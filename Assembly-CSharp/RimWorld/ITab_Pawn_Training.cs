using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085C RID: 2140
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x0600306B RID: 12395 RVA: 0x001A5210 File Offset: 0x001A3610
		public ITab_Pawn_Training()
		{
			this.size = new Vector2(300f, 130f + 28f * (float)DefDatabase<TrainableDef>.DefCount);
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x001A525C File Offset: 0x001A365C
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x001A5298 File Offset: 0x001A3698
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}
	}
}
