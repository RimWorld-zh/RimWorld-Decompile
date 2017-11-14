using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		protected override int Tile
		{
			get
			{
				if (!(InspectGizmoGrid.mouseoverGizmo is Command_Settle))
				{
					return -1;
				}
				Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
				if (caravan == null)
				{
					return -1;
				}
				return caravan.Tile;
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
				return Mathf.Abs((float)(Time.time % 2.0 - 1.0));
			}
		}
	}
}
