using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public abstract class EditWindow : Window
	{
		private const float SuperimposeAvoidThreshold = 8f;

		private const float SuperimposeAvoidOffset = 16f;

		private const float SuperimposeAvoidOffsetMinEdge = 200f;

		[CompilerGenerated]
		private static Func<Window, bool> <>f__am$cache0;

		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

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

		[CompilerGenerated]
		private static bool <PostOpen>m__0(Window di)
		{
			return di is EditWindow;
		}
	}
}
