using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E61 RID: 3681
	public class FeedbackItem_FoodGain : FeedbackItem
	{
		// Token: 0x04003975 RID: 14709
		protected int Amount = 0;

		// Token: 0x060056C2 RID: 22210 RVA: 0x002CBA34 File Offset: 0x002C9E34
		public FeedbackItem_FoodGain(Vector2 ScreenPos, int Amount) : base(ScreenPos)
		{
			this.Amount = Amount;
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x002CBA4C File Offset: 0x002C9E4C
		public override void FeedbackOnGUI()
		{
			string str = this.Amount + " food";
			base.DrawFloatingText(str, Color.yellow);
		}
	}
}
