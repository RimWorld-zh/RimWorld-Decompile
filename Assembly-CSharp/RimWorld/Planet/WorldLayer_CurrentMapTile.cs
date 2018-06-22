using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058A RID: 1418
	public class WorldLayer_CurrentMapTile : WorldLayer_SingleTile
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x000E90BC File Offset: 0x000E74BC
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
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x000E9100 File Offset: 0x000E7500
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}
	}
}
