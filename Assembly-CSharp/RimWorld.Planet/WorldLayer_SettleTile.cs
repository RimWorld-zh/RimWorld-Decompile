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
				int result;
				if (!(InspectGizmoGrid.mouseoverGizmo is Command_Settle))
				{
					result = -1;
				}
				else
				{
					Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
					result = ((caravan != null) ? caravan.Tile : (-1));
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
				return Mathf.Abs((float)(Time.time % 2.0 - 1.0));
			}
		}
	}
}
