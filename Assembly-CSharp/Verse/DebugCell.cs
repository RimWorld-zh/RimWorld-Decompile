using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C04 RID: 3076
	internal sealed class DebugCell
	{
		// Token: 0x06004351 RID: 17233 RVA: 0x002395FB File Offset: 0x002379FB
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
			}
			else
			{
				CellRenderer.RenderCell(this.c, this.colorPct);
			}
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x00239638 File Offset: 0x00237A38
		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
				Rect rect2 = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
				if (rect2.Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x04002E04 RID: 11780
		public IntVec3 c;

		// Token: 0x04002E05 RID: 11781
		public string displayString;

		// Token: 0x04002E06 RID: 11782
		public float colorPct;

		// Token: 0x04002E07 RID: 11783
		public int ticksLeft;

		// Token: 0x04002E08 RID: 11784
		public Material customMat;
	}
}
