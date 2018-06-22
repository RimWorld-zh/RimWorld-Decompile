using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC7 RID: 3783
	public abstract class EditWindow : Window
	{
		// Token: 0x0600597E RID: 22910 RVA: 0x0029C3DC File Offset: 0x0029A7DC
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x0600597F RID: 22911 RVA: 0x0029C42C File Offset: 0x0029A82C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06005980 RID: 22912 RVA: 0x0029C450 File Offset: 0x0029A850
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x0029C46C File Offset: 0x0029A86C
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

		// Token: 0x04003BC6 RID: 15302
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04003BC7 RID: 15303
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x04003BC8 RID: 15304
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
