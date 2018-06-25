using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		public WorldLayer_MouseTile()
		{
		}

		protected override int Tile
		{
			get
			{
				WorldDragBox dragBox = Find.World.UI.selector.dragBox;
				int result;
				if (dragBox.IsValidAndActive)
				{
					result = -1;
				}
				else if (Find.WorldTargeter.IsTargeting)
				{
					result = -1;
				}
				else if (Find.ScreenshotModeHandler.Active)
				{
					result = -1;
				}
				else
				{
					result = GenWorld.MouseTile(false);
				}
				return result;
			}
		}

		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
