using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC9 RID: 3785
	public abstract class EditWindow : Window
	{
		// Token: 0x0600595F RID: 22879 RVA: 0x0029AE1C File Offset: 0x0029921C
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06005960 RID: 22880 RVA: 0x0029AE6C File Offset: 0x0029926C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005961 RID: 22881 RVA: 0x0029AE90 File Offset: 0x00299290
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x0029AEAC File Offset: 0x002992AC
		public override void PostOpen()
		{
			while (this.windowRect.x <= (float)UI.screenWidth - 200f && this.windowRect.y <= (float)UI.screenHeight - 200f)
			{
				bool flag = false;
				foreach (EditWindow editWindow in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (editWindow != this)
					{
						if (Mathf.Abs(editWindow.windowRect.x - this.windowRect.x) < 8f && Mathf.Abs(editWindow.windowRect.y - this.windowRect.y) < 8f)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return;
				}
				this.windowRect.x = this.windowRect.x + 16f;
				this.windowRect.y = this.windowRect.y + 16f;
			}
		}

		// Token: 0x04003BB7 RID: 15287
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04003BB8 RID: 15288
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x04003BB9 RID: 15289
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
