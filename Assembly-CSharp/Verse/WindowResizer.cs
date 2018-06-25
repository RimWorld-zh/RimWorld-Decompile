using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECE RID: 3790
	public class WindowResizer
	{
		// Token: 0x04003BF7 RID: 15351
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x04003BF8 RID: 15352
		private bool isResizing = false;

		// Token: 0x04003BF9 RID: 15353
		private Rect resizeStart = default(Rect);

		// Token: 0x04003BFA RID: 15354
		private const float ResizeButtonSize = 24f;

		// Token: 0x0600599E RID: 22942 RVA: 0x002DE240 File Offset: 0x002DC640
		public Rect DoResizeControl(Rect winRect)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(winRect.width - 24f, winRect.height - 24f, 24f, 24f);
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.isResizing = true;
				this.resizeStart = new Rect(mousePosition.x, mousePosition.y, winRect.width, winRect.height);
			}
			if (this.isResizing)
			{
				winRect.width = this.resizeStart.width + (mousePosition.x - this.resizeStart.x);
				winRect.height = this.resizeStart.height + (mousePosition.y - this.resizeStart.y);
				if (winRect.width < this.minWindowSize.x)
				{
					winRect.width = this.minWindowSize.x;
				}
				if (winRect.height < this.minWindowSize.y)
				{
					winRect.height = this.minWindowSize.y;
				}
				winRect.xMax = Mathf.Min((float)UI.screenWidth, winRect.xMax);
				winRect.yMax = Mathf.Min((float)UI.screenHeight, winRect.yMax);
				if (Event.current.type == EventType.MouseUp)
				{
					this.isResizing = false;
				}
			}
			Widgets.ButtonImage(rect, TexUI.WinExpandWidget);
			return new Rect(winRect.x, winRect.y, (float)((int)winRect.width), (float)((int)winRect.height));
		}
	}
}
