using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000858 RID: 2136
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x06003064 RID: 12388 RVA: 0x001A53F0 File Offset: 0x001A37F0
		public ITab_Pawn_Training()
		{
			this.size = new Vector2(300f, 130f + 28f * (float)DefDatabase<TrainableDef>.DefCount);
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x001A543C File Offset: 0x001A383C
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x001A5478 File Offset: 0x001A3878
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}
	}
}
