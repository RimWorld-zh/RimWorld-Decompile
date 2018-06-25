using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000595 RID: 1429
	public class WorldLayer_SettleTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x000EB0D0 File Offset: 0x000E94D0
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
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x000EB128 File Offset: 0x000E9528
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x000EB144 File Offset: 0x000E9544
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
