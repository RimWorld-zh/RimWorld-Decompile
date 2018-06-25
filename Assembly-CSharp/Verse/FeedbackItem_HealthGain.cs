using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E63 RID: 3683
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x0400397E RID: 14718
		protected Pawn Healer;

		// Token: 0x0400397F RID: 14719
		protected int Amount = 0;

		// Token: 0x060056C4 RID: 22212 RVA: 0x002CBC68 File Offset: 0x002CA068
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x002CBC88 File Offset: 0x002CA088
		public override void FeedbackOnGUI()
		{
			string text;
			if (this.Amount >= 0)
			{
				text = "+";
			}
			else
			{
				text = "-";
			}
			text += this.Amount;
			base.DrawFloatingText(text, Color.red);
		}
	}
}
