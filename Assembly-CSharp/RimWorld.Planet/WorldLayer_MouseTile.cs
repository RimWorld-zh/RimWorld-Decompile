using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		protected override int Tile
		{
			get
			{
				WorldDragBox dragBox = Find.World.UI.selector.dragBox;
				return (!dragBox.IsValidAndActive) ? ((!Find.WorldTargeter.IsTargeting) ? GenWorld.MouseTile(false) : (-1)) : (-1);
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
