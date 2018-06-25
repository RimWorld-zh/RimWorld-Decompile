using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECD RID: 3789
	public class WindowResizer
	{
		// Token: 0x04003BEF RID: 15343
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x04003BF0 RID: 15344
		private bool isResizing = false;

		// Token: 0x04003BF1 RID: 15345
		private Rect resizeStart = default(Rect);

		// Token: 0x04003BF2 RID: 15346
		private const float ResizeButtonSize = 24f;

		// Token: 0x0600599E RID: 22942 RVA: 0x002DE054 File Offset: 0x002DC454
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
