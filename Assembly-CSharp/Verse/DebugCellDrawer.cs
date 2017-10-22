using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public sealed class DebugCellDrawer
	{
		private const int DefaultLifespanTicks = 50;

		private List<DebugCell> debugCells = new List<DebugCell>();

		private List<DebugLine> debugLines = new List<DebugLine>();

		public void FlashCell(IntVec3 c, float colorPct = 0, string text = null)
		{
			DebugCell debugCell = new DebugCell();
			debugCell.c = c;
			debugCell.displayString = text;
			debugCell.colorPct = colorPct;
			debugCell.ticksLeft = 50;
			this.debugCells.Add(debugCell);
		}

		public void FlashCell(IntVec3 c, Material mat, string text = null)
		{
			DebugCell debugCell = new DebugCell();
			debugCell.c = c;
			debugCell.displayString = text;
			debugCell.customMat = mat;
			debugCell.ticksLeft = 50;
			this.debugCells.Add(debugCell);
		}

		public void FlashLine(IntVec3 a, IntVec3 b)
		{
			DebugLine item = new DebugLine(a.ToVector3Shifted(), b.ToVector3Shifted())
			{
				TicksLeft = 50
			};
			this.debugLines.Add(item);
		}

		public void DebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugCells.Count; i++)
			{
				this.debugCells[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		public void DebugDrawerTick()
		{
			for (int num = this.debugCells.Count - 1; num >= 0; num--)
			{
				DebugCell debugCell = this.debugCells[num];
				debugCell.ticksLeft--;
				if (debugCell.ticksLeft <= 0)
				{
					this.debugCells.RemoveAt(num);
				}
			}
			for (int num2 = this.debugLines.Count - 1; num2 >= 0; num2--)
			{
				List<DebugLine> obj = this.debugLines;
				int index = num2;
				DebugLine debugLine = this.debugLines[num2];
				Vector3 a = debugLine.a;
				DebugLine debugLine2 = this.debugLines[num2];
				obj[index] = new DebugLine(a, debugLine2.b, this.debugLines[num2].TicksLeft - 1);
				if (this.debugLines[num2].TicksLeft <= 0)
				{
					this.debugLines.RemoveAt(num2);
				}
			}
		}

		public void DebugDrawerOnGUI()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				for (int i = 0; i < this.debugCells.Count; i++)
				{
					this.debugCells[i].OnGUI();
				}
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}
	}
}
