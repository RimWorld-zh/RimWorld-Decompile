using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EA6 RID: 3750
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x06005858 RID: 22616 RVA: 0x002D3F42 File Offset: 0x002D2342
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x002D3F79 File Offset: 0x002D2379
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x0600585A RID: 22618 RVA: 0x002D3FB0 File Offset: 0x002D23B0
		public bool Selected
		{
			get
			{
				return (this.selectedGetter == null) ? this.selected : this.selectedGetter();
			}
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x002D3FE8 File Offset: 0x002D23E8
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

		// Token: 0x04003A83 RID: 14979
		public string label = "Tab";

		// Token: 0x04003A84 RID: 14980
		public Action clickedAction = null;

		// Token: 0x04003A85 RID: 14981
		public bool selected = false;

		// Token: 0x04003A86 RID: 14982
		public Func<bool> selectedGetter;

		// Token: 0x04003A87 RID: 14983
		private const float TabEndWidth = 30f;

		// Token: 0x04003A88 RID: 14984
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x04003A89 RID: 14985
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);
	}
}
