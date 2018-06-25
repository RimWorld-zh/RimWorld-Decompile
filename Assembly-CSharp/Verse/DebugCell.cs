using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C07 RID: 3079
	internal sealed class DebugCell
	{
		// Token: 0x04002E0B RID: 11787
		public IntVec3 c;

		// Token: 0x04002E0C RID: 11788
		public string displayString;

		// Token: 0x04002E0D RID: 11789
		public float colorPct;

		// Token: 0x04002E0E RID: 11790
		public int ticksLeft;

		// Token: 0x04002E0F RID: 11791
		public Material customMat;

		// Token: 0x06004354 RID: 17236 RVA: 0x002399B7 File Offset: 0x00237DB7
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

		// Token: 0x06004355 RID: 17237 RVA: 0x002399F4 File Offset: 0x00237DF4
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
	}
}
