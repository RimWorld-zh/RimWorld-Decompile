using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		public WorldLayer_CurrentMapTile()
		{
		}

		protected override int Tile
		{
			get
			{
				int result;
				if (Current.ProgramState != ProgramState.Playing)
				{
					result = -1;
				}
				else if (Find.CurrentMap == null)
				{
					result = -1;
				}
				else
				{
					result = Find.CurrentMap.Tile;
				}
				return result;
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
