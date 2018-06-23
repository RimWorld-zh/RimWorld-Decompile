using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9C RID: 3740
	public class ListableOption
	{
		// Token: 0x04003A71 RID: 14961
		public string label;

		// Token: 0x04003A72 RID: 14962
		public Action action;

		// Token: 0x04003A73 RID: 14963
		private string uiHighlightTag;

		// Token: 0x04003A74 RID: 14964
		public float minHeight = 45f;

		// Token: 0x06005862 RID: 22626 RVA: 0x002D4E7D File Offset: 0x002D327D
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x002D4EA8 File Offset: 0x002D32A8
		public virtual float DrawOption(Vector2 pos, float width)
		{
			float b = Text.CalcHeight(this.label, width);
			float num = Mathf.Max(this.minHeight, b);
			Rect rect = new Rect(pos.x, pos.y, width, num);
			if (Widgets.ButtonText(rect, this.label, true, true, true))
			{
				this.action();
			}
			if (this.uiHighlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.uiHighlightTag);
			}
			return num;
		}
	}
}
