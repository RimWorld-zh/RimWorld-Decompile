using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C08 RID: 3080
	internal sealed class DebugCell
	{
		// Token: 0x0600434A RID: 17226 RVA: 0x0023825B File Offset: 0x0023665B
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

		// Token: 0x0600434B RID: 17227 RVA: 0x00238298 File Offset: 0x00236698
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

		// Token: 0x04002DFC RID: 11772
		public IntVec3 c;

		// Token: 0x04002DFD RID: 11773
		public string displayString;

		// Token: 0x04002DFE RID: 11774
		public float colorPct;

		// Token: 0x04002DFF RID: 11775
		public int ticksLeft;

		// Token: 0x04002E00 RID: 11776
		public Material customMat;
	}
}
