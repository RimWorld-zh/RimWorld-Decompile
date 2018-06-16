using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9E RID: 3742
	public class ListableOption
	{
		// Token: 0x06005844 RID: 22596 RVA: 0x002D326D File Offset: 0x002D166D
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x002D3298 File Offset: 0x002D1698
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

		// Token: 0x04003A63 RID: 14947
		public string label;

		// Token: 0x04003A64 RID: 14948
		public Action action;

		// Token: 0x04003A65 RID: 14949
		private string uiHighlightTag;

		// Token: 0x04003A66 RID: 14950
		public float minHeight = 45f;
	}
}
