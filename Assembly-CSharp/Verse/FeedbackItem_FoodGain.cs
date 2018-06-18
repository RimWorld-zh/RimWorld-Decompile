using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E60 RID: 3680
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x0600569E RID: 22174 RVA: 0x002C9CF8 File Offset: 0x002C80F8
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x002C9D10 File Offset: 0x002C8110
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}

		// Token: 0x04003966 RID: 14694
		protected int Amount = 0;
	}
}
