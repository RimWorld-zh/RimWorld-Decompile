using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000591 RID: 1425
	public class WorldLayer_MouseTile : WorldLayer_SingleTile
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x000E9808 File Offset: 0x000E7C08
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
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x000E9878 File Offset: 0x000E7C78
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}
	}
}
