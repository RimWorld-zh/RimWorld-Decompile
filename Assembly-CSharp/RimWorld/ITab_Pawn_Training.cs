using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085A RID: 2138
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x06003068 RID: 12392 RVA: 0x001A5540 File Offset: 0x001A3940
		public ITab_Pawn_Training()
		{
			this.size = new Vector2(300f, 130f + 28f * (float)DefDatabase<TrainableDef>.DefCount);
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x001A558C File Offset: 0x001A398C
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x001A55C8 File Offset: 0x001A39C8
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}
	}
}
