using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E61 RID: 3681
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x060056A0 RID: 22176 RVA: 0x002C9CF8 File Offset: 0x002C80F8
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x002C9D10 File Offset: 0x002C8110
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x04003968 RID: 14696
		protected int Amount = 0;
	}
}
