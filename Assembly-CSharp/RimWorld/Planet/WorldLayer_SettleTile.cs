using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000593 RID: 1427
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x000EAD18 File Offset: 0x000E9118
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

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x000EAD70 File Offset: 0x000E9170
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001B3F RID: 6975 RVA: 0x000EAD8C File Offset: 0x000E918C
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
