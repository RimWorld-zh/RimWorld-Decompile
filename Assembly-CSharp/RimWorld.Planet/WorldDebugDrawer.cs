using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldDebugDrawer
	{
		private const int DefaultLifespanTicks = 50;

		private const float MaxDistToCameraToDisplayLabel = 39f;

		private List<DebugTile> debugTiles = new List<DebugTile>();

		private List<DebugWorldLine> debugLines = new List<DebugWorldLine>();

		public void FlashTile(int tile, float colorPct = 0, string text = null)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.colorPct = colorPct;
			debugTile.ticksLeft = 50;
			this.debugTiles.Add(debugTile);
		}

		public void FlashTile(int tile, Material mat, string text = null)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.customMat = mat;
			debugTile.ticksLeft = 50;
			this.debugTiles.Add(debugTile);
		}

		public void FlashLine(Vector3 a, Vector3 b, bool onPlanetSurface = false)
		{
			DebugWorldLine debugWorldLine = new DebugWorldLine(a, b, onPlanetSurface);
			debugWorldLine.TicksLeft = 50;
			this.debugLines.Add(debugWorldLine);
		}

		public void FlashLine(int tileA, int tileB)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(tileA);
			Vector3 tileCenter2 = worldGrid.GetTileCenter(tileB);
			DebugWorldLine debugWorldLine = new DebugWorldLine(tileCenter, tileCenter2, true);
			debugWorldLine.TicksLeft = 50;
			this.debugLines.Add(debugWorldLine);
		}

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

		public void WorldDebugDrawerTick()
		{
			for (int num = this.debugTiles.Count - 1; num >= 0; num--)
			{
				DebugTile debugTile = this.debugTiles[num];
				debugTile.ticksLeft--;
				if (debugTile.ticksLeft <= 0)
				{
					this.debugTiles.RemoveAt(num);
				}
			}
			for (int num2 = this.debugLines.Count - 1; num2 >= 0; num2--)
			{
				DebugWorldLine debugWorldLine = this.debugLines[num2];
				debugWorldLine.ticksLeft--;
				if (debugWorldLine.ticksLeft <= 0)
				{
					this.debugLines.RemoveAt(num2);
				}
			}
		}

		public void WorldDebugDrawerOnGUI()
		{
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				if (!(this.debugTiles[i].DistanceToCamera > 39.0))
				{
					this.debugTiles[i].OnGUI();
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
