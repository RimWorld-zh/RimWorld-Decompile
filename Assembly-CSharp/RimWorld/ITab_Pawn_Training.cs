using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085A RID: 2138
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x06003067 RID: 12391 RVA: 0x001A57A8 File Offset: 0x001A3BA8
		public ITab_Pawn_Training()
		{
			this.size = new Vector2(300f, 130f + 28f * (float)DefDatabase<TrainableDef>.DefCount);
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003068 RID: 12392 RVA: 0x001A57F4 File Offset: 0x001A3BF4
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x001A5830 File Offset: 0x001A3C30
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}
	}
}
