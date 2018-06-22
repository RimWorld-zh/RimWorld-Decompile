using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000592 RID: 1426
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x000EACD4 File Offset: 0x000E90D4
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x000EACF4 File Offset: 0x000E90F4
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
