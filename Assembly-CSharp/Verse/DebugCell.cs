using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C06 RID: 3078
	internal sealed class DebugCell
	{
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

		// Token: 0x06004354 RID: 17236 RVA: 0x002396D7 File Offset: 0x00237AD7
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

		// Token: 0x06004355 RID: 17237 RVA: 0x00239714 File Offset: 0x00237B14
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
