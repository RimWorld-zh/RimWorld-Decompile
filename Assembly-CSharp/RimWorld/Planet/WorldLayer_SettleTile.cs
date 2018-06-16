using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000597 RID: 1431
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001B45 RID: 6981 RVA: 0x000EAC58 File Offset: 0x000E9058
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
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x000EACB0 File Offset: 0x000E90B0
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x000EACCC File Offset: 0x000E90CC
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
