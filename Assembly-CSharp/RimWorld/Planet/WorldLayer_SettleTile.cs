using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000595 RID: 1429
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x000EAE68 File Offset: 0x000E9268
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
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x000EAEC0 File Offset: 0x000E92C0
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x000EAEDC File Offset: 0x000E92DC
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
