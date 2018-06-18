using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C07 RID: 3079
	internal sealed class DebugCell
	{
		// Token: 0x06004348 RID: 17224 RVA: 0x00238233 File Offset: 0x00236633
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

		// Token: 0x06004349 RID: 17225 RVA: 0x00238270 File Offset: 0x00236670
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

		// Token: 0x04002DFA RID: 11770
		public IntVec3 c;

		// Token: 0x04002DFB RID: 11771
		public string displayString;

		// Token: 0x04002DFC RID: 11772
		public float colorPct;

		// Token: 0x04002DFD RID: 11773
		public int ticksLeft;

		// Token: 0x04002DFE RID: 11774
		public Material customMat;
	}
}
