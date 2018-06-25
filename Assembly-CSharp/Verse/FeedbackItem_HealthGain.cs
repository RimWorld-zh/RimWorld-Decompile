using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E62 RID: 3682
	public class FeedbackItem_HealthGain : FeedbackItem
	{
		// Token: 0x04003976 RID: 14710
		protected Pawn Healer;

		// Token: 0x04003977 RID: 14711
		protected int Amount = 0;

		// Token: 0x060056C4 RID: 22212 RVA: 0x002CBA7C File Offset: 0x002C9E7C
		public FeedbackItem_HealthGain(Vector2 ScreenPos, int Amount, Pawn Healer) : base(ScreenPos)
		{
			this.Amount = Amount;
			this.Healer = Healer;
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x002CBA9C File Offset: 0x002C9E9C
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
