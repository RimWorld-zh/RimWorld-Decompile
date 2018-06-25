using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EA7 RID: 3751
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x04003A93 RID: 14995
		public string label = "Tab";

		// Token: 0x04003A94 RID: 14996
		public Action clickedAction = null;

		// Token: 0x04003A95 RID: 14997
		public bool selected = false;

		// Token: 0x04003A96 RID: 14998
		public Func<bool> selectedGetter;

		// Token: 0x04003A97 RID: 14999
		private const float TabEndWidth = 30f;

		// Token: 0x04003A98 RID: 15000
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x04003A99 RID: 15001
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);

		// Token: 0x0600587C RID: 22652 RVA: 0x002D5C7E File Offset: 0x002D407E
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x002D5CB5 File Offset: 0x002D40B5
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x0600587E RID: 22654 RVA: 0x002D5CEC File Offset: 0x002D40EC
		public bool Selected
		{
			get
			{
				return (this.selectedGetter == null) ? this.selected : this.selectedGetter();
			}
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x002D5D24 File Offset: 0x002D4124
		public void Draw(Rect rect)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect);
			drawRect2.width = 30f;
			drawRect2.x = rect.x + rect.width - 30f;
			Rect uvRect = new Rect(0.53125f, 0f, 0.46875f, 1f);
			Rect drawRect3 = new Rect(rect);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			Rect uvRect2 = new Rect(30f, 0f, 4f, (float)TabRecord.TabAtlas.height).ToUVRect(new Vector2((float)TabRecord.TabAtlas.width, (float)TabRecord.TabAtlas.height));
			Widgets.DrawTexturePart(drawRect, new Rect(0f, 0f, 0.46875f, 1f), TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect3, uvRect2, TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect2, uvRect, TabRecord.TabAtlas);
			Rect rect2 = rect;
			rect2.width -= 10f;
			if (Mouse.IsOver(rect2))
			{
				GUI.color = Color.yellow;
				rect2.x += 2f;
				rect2.y -= 2f;
			}
			Text.WordWrap = false;
			Widgets.Label(rect2, this.label);
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (!this.Selected)
			{
				Rect drawRect4 = new Rect(rect);
				drawRect4.y += rect.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				Rect uvRect3 = new Rect(0.5f, 0.01f, 0.01f, 0.01f);
				Widgets.DrawTexturePart(drawRect4, uvRect3, TabRecord.TabAtlas);
			}
		}
	}
}
