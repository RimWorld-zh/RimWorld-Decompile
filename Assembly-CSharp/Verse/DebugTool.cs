using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E25 RID: 3621
	public class DebugTool
	{
		// Token: 0x060054D8 RID: 21720 RVA: 0x002B80CB File Offset: 0x002B64CB
		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x002B80F8 File Offset: 0x002B64F8
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

		// Token: 0x060054DA RID: 21722 RVA: 0x002B8148 File Offset: 0x002B6548
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

		// Token: 0x040037FE RID: 14334
		private string label;

		// Token: 0x040037FF RID: 14335
		private Action clickAction = null;

		// Token: 0x04003800 RID: 14336
		private Action onGUIAction = null;
	}
}
