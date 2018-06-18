using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC8 RID: 3784
	public abstract class EditWindow : Window
	{
		// Token: 0x0600595D RID: 22877 RVA: 0x0029ADFC File Offset: 0x002991FC
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x0600595E RID: 22878 RVA: 0x0029AE4C File Offset: 0x0029924C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x0600595F RID: 22879 RVA: 0x0029AE70 File Offset: 0x00299270
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x0029AE8C File Offset: 0x0029928C
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

		// Token: 0x04003BB6 RID: 15286
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04003BB7 RID: 15287
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x04003BB8 RID: 15288
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
