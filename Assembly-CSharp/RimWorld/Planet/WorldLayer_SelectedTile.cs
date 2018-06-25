using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000594 RID: 1428
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x000EB08C File Offset: 0x000E948C
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x000EB0AC File Offset: 0x000E94AC
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
