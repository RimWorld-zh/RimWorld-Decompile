using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		protected override int Tile
		{
			get
			{
				return (Current.ProgramState == ProgramState.Playing) ? ((Find.VisibleMap != null) ? Find.VisibleMap.Tile : (-1)) : (-1);
			}
		}

		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
