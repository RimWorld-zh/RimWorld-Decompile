using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ITab_Pawn_Training : ITab
	{
		public ITab_Pawn_Training()
		{
			this.size = new Vector2(300f, 130f + 28f * (float)DefDatabase<TrainableDef>.DefCount);
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}
	}
}
