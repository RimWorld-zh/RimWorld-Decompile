using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public abstract class EditWindow : Window
	{
		private const float SuperimposeAvoidThreshold = 8f;

		private const float SuperimposeAvoidOffset = 16f;

		private const float SuperimposeAvoidOffsetMinEdge = 200f;

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

		public EditWindow()
		{
			base.resizeable = true;
			base.draggable = true;
			base.preventCameraMotion = false;
			base.doCloseX = true;
			base.windowRect.x = 5f;
			base.windowRect.y = 5f;
		}

		public override void PostOpen()
		{
			while (!(base.windowRect.x > (float)UI.screenWidth - 200.0) && !(base.windowRect.y > (float)UI.screenHeight - 200.0))
			{
				bool flag = false;
				foreach (EditWindow item in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (item != this && Mathf.Abs(item.windowRect.x - base.windowRect.x) < 8.0 && Mathf.Abs(item.windowRect.y - base.windowRect.y) < 8.0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					base.windowRect.x += 16f;
					base.windowRect.y += 16f;
					continue;
				}
				break;
			}
		}
	}
}
