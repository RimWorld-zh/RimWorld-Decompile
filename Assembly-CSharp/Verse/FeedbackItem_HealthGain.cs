using UnityEngine;

namespace Verse
{
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		protected Pawn Healer;

		protected int Amount = 0;

		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		public override void FeedbackOnGUI()
		{
			string text = "";
			text = ((this.Amount < 0) ? "-" : "+");
			text += this.Amount;
			base.DrawFloatingText(text, Color.red);
		}
	}
}
