using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058C RID: 1420
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x000E9474 File Offset: 0x000E7874
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

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000E94B8 File Offset: 0x000E78B8
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
