using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E62 RID: 3682
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x0400397D RID: 14717
		protected int Amount = 0;

		// Token: 0x060056C2 RID: 22210 RVA: 0x002CBC20 File Offset: 0x002CA020
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x002CBC38 File Offset: 0x002CA038
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}
	}
}
