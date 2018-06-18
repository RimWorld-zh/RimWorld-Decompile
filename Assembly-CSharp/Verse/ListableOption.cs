using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9D RID: 3741
	public class ListableOption
	{
		// Token: 0x06005842 RID: 22594 RVA: 0x002D326D File Offset: 0x002D166D
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x002D3298 File Offset: 0x002D1698
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

		// Token: 0x04003A61 RID: 14945
		public string label;

		// Token: 0x04003A62 RID: 14946
		public Action action;

		// Token: 0x04003A63 RID: 14947
		private string uiHighlightTag;

		// Token: 0x04003A64 RID: 14948
		public float minHeight = 45f;
	}
}
