using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ITab_Pawn_Training : ITab
	{
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		public ITab_Pawn_Training()
		{
			base.size = new Vector2(300f, 450f);
			base.labelKey = "TabTraining";
			base.tutorTag = "Training";
		}

		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, base.size.x, base.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}
	}
}
