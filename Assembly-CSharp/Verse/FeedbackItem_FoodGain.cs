using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5F RID: 3679
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x04003975 RID: 14709
		protected int Amount = 0;

		// Token: 0x060056BE RID: 22206 RVA: 0x002CB908 File Offset: 0x002C9D08
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x002CB920 File Offset: 0x002C9D20
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}
	}
}
