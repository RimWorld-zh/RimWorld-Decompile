using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		public WorldLayer_SettleTile()
		{
		}

		protected override int Tile
		{
			get
			{
				int result;
				if (!(Find.WorldInterface.inspectPane.mouseoverGizmo is Command_Settle))
				{
					result = -1;
				}
				else
				{
					Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
					if (caravan == null)
					{
						result = -1;
					}
					else
					{
						result = caravan.Tile;
					}
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

		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
