using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9F RID: 3743
	public class ListableOption
	{
		// Token: 0x04003A79 RID: 14969
		public string label;

		// Token: 0x04003A7A RID: 14970
		public Action action;

		// Token: 0x04003A7B RID: 14971
		private string uiHighlightTag;

		// Token: 0x04003A7C RID: 14972
		public float minHeight = 45f;

		// Token: 0x06005866 RID: 22630 RVA: 0x002D5195 File Offset: 0x002D3595
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x002D51C0 File Offset: 0x002D35C0
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
