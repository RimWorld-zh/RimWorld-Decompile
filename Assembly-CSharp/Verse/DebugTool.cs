using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class DebugTool
	{
		private string label;

		private Action clickAction = null;

		private Action onGUIAction = null;

		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		public DebugTool(string label, Action clickAction, IntVec3 firstRectCorner)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = delegate()
			{
				IntVec3 intVec = UI.MouseCell();
				Vector3 v = firstRectCorner.ToVector3Shifted();
				Vector3 v2 = intVec.ToVector3Shifted();
				if (v.x < v2.x)
				{
					v.x -= 0.5f;
					v2.x += 0.5f;
				}
				else
				{
					v.x += 0.5f;
					v2.x -= 0.5f;
				}
				if (v.z < v2.z)
				{
					v.z -= 0.5f;
					v2.z += 0.5f;
				}
				else
				{
					v.z += 0.5f;
					v2.z -= 0.5f;
				}
				Vector2 vector = v.MapToUIPosition();
				Vector2 vector2 = v2.MapToUIPosition();
				Widgets.DrawBox(new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y), 3);
			};
		}

		public void DebugToolOnGUI()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					this.clickAction();
				}
				if (Event.current.button == 1)
				{
					DebugTools.curTool = null;
				}
				Event.current.Use();
			}
			Vector2 vector = Event.current.mousePosition + new Vector2(15f, 15f);
			Rect rect = new Rect(vector.x, vector.y, 999f, 999f);
			Text.Font = GameFont.Small;
			Widgets.Label(rect, this.label);
			if (this.onGUIAction != null)
			{
				this.onGUIAction();
			}
		}

		[CompilerGenerated]
		private sealed class <DebugTool>c__AnonStorey0
		{
			internal IntVec3 firstRectCorner;

			public <DebugTool>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				IntVec3 intVec = UI.MouseCell();
				Vector3 v = this.firstRectCorner.ToVector3Shifted();
				Vector3 v2 = intVec.ToVector3Shifted();
				if (v.x < v2.x)
				{
					v.x -= 0.5f;
					v2.x += 0.5f;
				}
				else
				{
					v.x += 0.5f;
					v2.x -= 0.5f;
				}
				if (v.z < v2.z)
				{
					v.z -= 0.5f;
					v2.z += 0.5f;
				}
				else
				{
					v.z += 0.5f;
					v2.z -= 0.5f;
				}
				Vector2 vector = v.MapToUIPosition();
				Vector2 vector2 = v2.MapToUIPosition();
				Widgets.DrawBox(new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y), 3);
			}
		}
	}
}
