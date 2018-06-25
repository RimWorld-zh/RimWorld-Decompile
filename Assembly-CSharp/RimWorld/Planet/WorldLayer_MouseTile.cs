using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058F RID: 1423
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x000E99AC File Offset: 0x000E7DAC
		protected override int Tile
		{
			get
			{
				WorldDragBox dragBox = Find.World.UI.selector.dragBox;
				int result;
				if (dragBox.IsValidAndActive)
				{
					result = -1;
				}
				else if (Find.WorldTargeter.IsTargeting)
				{
					result = -1;
				}
				else if (Find.ScreenshotModeHandler.Active)
				{
					result = -1;
				}
				else
				{
					result = GenWorld.MouseTile(false);
				}
				return result;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x000E9A1C File Offset: 0x000E7E1C
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
