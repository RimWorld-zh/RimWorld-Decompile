using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECA RID: 3786
	public abstract class EditWindow : Window
	{
		// Token: 0x04003BCE RID: 15310
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04003BCF RID: 15311
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x04003BD0 RID: 15312
		private const float SuperimposeAvoidOffsetMinEdge = 200f;

		// Token: 0x06005981 RID: 22913 RVA: 0x0029C7E8 File Offset: 0x0029ABE8
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x0029C838 File Offset: 0x0029AC38
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06005983 RID: 22915 RVA: 0x0029C85C File Offset: 0x0029AC5C
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x0029C878 File Offset: 0x0029AC78
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
	}
}
