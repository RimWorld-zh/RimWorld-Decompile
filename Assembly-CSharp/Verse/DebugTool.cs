using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E25 RID: 3621
	public class DebugTool
	{
		// Token: 0x04003813 RID: 14355
		private string label;

		// Token: 0x04003814 RID: 14356
		private Action clickAction = null;

		// Token: 0x04003815 RID: 14357
		private Action onGUIAction = null;

		// Token: 0x060054F8 RID: 21752 RVA: 0x002BA0A3 File Offset: 0x002B84A3
		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x002BA0D0 File Offset: 0x002B84D0
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

		// Token: 0x060054FA RID: 21754 RVA: 0x002BA120 File Offset: 0x002B8520
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
	}
}
