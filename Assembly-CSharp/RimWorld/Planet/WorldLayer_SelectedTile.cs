using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000594 RID: 1428
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x000EAE24 File Offset: 0x000E9224
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001B3F RID: 6975 RVA: 0x000EAE44 File Offset: 0x000E9244
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
