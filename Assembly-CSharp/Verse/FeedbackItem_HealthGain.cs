using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E60 RID: 3680
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x060056C0 RID: 22208 RVA: 0x002CB950 File Offset: 0x002C9D50
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x002CB970 File Offset: 0x002C9D70
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

		// Token: 0x04003976 RID: 14710
		protected Pawn Healer;

		// Token: 0x04003977 RID: 14711
		protected int Amount = 0;
	}
}
