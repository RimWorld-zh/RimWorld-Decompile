using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000596 RID: 1430
	public class WorldLayer_SelectedTile : WorldLayer_SingleTile
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x000EAC80 File Offset: 0x000E9080
		protected override int Tile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x000EACA0 File Offset: 0x000E90A0
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}
	}
}
