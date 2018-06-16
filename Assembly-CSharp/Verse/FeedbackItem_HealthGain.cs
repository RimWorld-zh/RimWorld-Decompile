using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E62 RID: 3682
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x060056A2 RID: 22178 RVA: 0x002C9D40 File Offset: 0x002C8140
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x002C9D60 File Offset: 0x002C8160
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

		// Token: 0x04003969 RID: 14697
		protected Pawn Healer;

		// Token: 0x0400396A RID: 14698
		protected int Amount = 0;
	}
}
