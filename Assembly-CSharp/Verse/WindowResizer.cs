using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECD RID: 3789
	public class WindowResizer
	{
		// Token: 0x0600597C RID: 22908 RVA: 0x002DC2B0 File Offset: 0x002DA6B0
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

		// Token: 0x04003BE0 RID: 15328
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x04003BE1 RID: 15329
		private bool isResizing = false;

		// Token: 0x04003BE2 RID: 15330
		private Rect resizeStart = default(Rect);

		// Token: 0x04003BE3 RID: 15331
		private const float ResizeButtonSize = 24f;
	}
}
