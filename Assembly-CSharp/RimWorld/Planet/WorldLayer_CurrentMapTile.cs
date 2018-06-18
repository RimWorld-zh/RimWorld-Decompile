using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058E RID: 1422
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x000E9068 File Offset: 0x000E7468
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
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000E90AC File Offset: 0x000E74AC
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
