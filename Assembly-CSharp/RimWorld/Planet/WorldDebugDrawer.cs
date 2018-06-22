using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000584 RID: 1412
	public class WorldDebugDrawer
	{
		// Token: 0x06001AF6 RID: 6902 RVA: 0x000E7F48 File Offset: 0x000E6348
		public void FlashTile(int tile, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.colorPct = colorPct;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000E7F88 File Offset: 0x000E6388
		public void FlashTile(int tile, Material mat, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.customMat = mat;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000E7FC8 File Offset: 0x000E63C8
		public void FlashLine(Vector3 a, Vector3 b, bool onPlanetSurface = false, int duration = 50)
		{
			DebugWorldLine debugWorldLine = new DebugWorldLine(a, b, onPlanetSurface);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000E7FF4 File Offset: 0x000E63F4
		public void FlashLine(int tileA, int tileB, int duration = 50)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(tileA);
			Vector3 tileCenter2 = worldGrid.GetTileCenter(tileB);
			DebugWorldLine debugWorldLine = new DebugWorldLine(tileCenter, tileCenter2, true);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000E8034 File Offset: 0x000E6434
		public void WorldDebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				this.debugTiles[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x000E80A0 File Offset: 0x000E64A0
		public void WorldDebugDrawerTick()
		{
			for (int i = this.debugTiles.Count - 1; i >= 0; i--)
			{
				DebugTile debugTile = this.debugTiles[i];
				debugTile.ticksLeft--;
				if (debugTile.ticksLeft <= 0)
				{
					this.debugTiles.RemoveAt(i);
				}
			}
			for (int j = this.debugLines.Count - 1; j >= 0; j--)
			{
				DebugWorldLine debugWorldLine = this.debugLines[j];
				debugWorldLine.ticksLeft--;
				if (debugWorldLine.ticksLeft <= 0)
				{
					this.debugLines.RemoveAt(j);
				}
			}
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x000E8154 File Offset: 0x000E6554
		public void WorldDebugDrawerOnGUI()
		{
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				if (this.debugTiles[i].DistanceToCamera <= 39f)
				{
					this.debugTiles[i].OnGUI();
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04000FE0 RID: 4064
		private List<DebugTile> debugTiles = new List<DebugTile>();

		// Token: 0x04000FE1 RID: 4065
		private List<DebugWorldLine> debugLines = new List<DebugWorldLine>();

		// Token: 0x04000FE2 RID: 4066
		private const int DefaultLifespanTicks = 50;

		// Token: 0x04000FE3 RID: 4067
		private const float MaxDistToCameraToDisplayLabel = 39f;
	}
}
