using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E61 RID: 3681
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x060056A0 RID: 22176 RVA: 0x002C9D40 File Offset: 0x002C8140
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x002C9D60 File Offset: 0x002C8160
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

		// Token: 0x04003967 RID: 14695
		protected Pawn Healer;

		// Token: 0x04003968 RID: 14696
		protected int Amount = 0;
	}
}
