using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E22 RID: 3618
	public class DebugTool
	{
		// Token: 0x0400380C RID: 14348
		private string label;

		// Token: 0x0400380D RID: 14349
		private Action clickAction = null;

		// Token: 0x0400380E RID: 14350
		private Action onGUIAction = null;

		// Token: 0x060054F4 RID: 21748 RVA: 0x002B9C83 File Offset: 0x002B8083
		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x002B9CB0 File Offset: 0x002B80B0
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

		// Token: 0x060054F6 RID: 21750 RVA: 0x002B9D00 File Offset: 0x002B8100
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
