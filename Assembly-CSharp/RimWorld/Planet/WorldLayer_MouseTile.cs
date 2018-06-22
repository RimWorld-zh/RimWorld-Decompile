using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058D RID: 1421
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000E985C File Offset: 0x000E7C5C
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
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x000E98CC File Offset: 0x000E7CCC
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
