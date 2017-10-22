using UnityEngine;

namespace Verse
{
	internal sealed class DebugCell
	{
		public IntVec3 c;

		public string displayString;

		public float colorPct;

		public int ticksLeft;

		public Material customMat;

		public void Draw()
		{
			if ((Object)this.customMat != (Object)null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
			}
			else
			{
				CellRenderer.RenderCell(this.c, this.colorPct);
			}
		}

		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect((float)(vector.x - 20.0), (float)(vector.y - 20.0), 40f, 40f);
				if (new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}
	}
}
