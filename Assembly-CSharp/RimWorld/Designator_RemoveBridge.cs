using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D5 RID: 2005
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x06002C7C RID: 11388 RVA: 0x001772E4 File Offset: 0x001756E4
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x00177334 File Offset: 0x00175734
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (c.InBounds(base.Map) && c.GetTerrain(base.Map) != TerrainDefOf.Bridge)
			{
				result = false;
			}
			else
			{
				result = base.CanDesignateCell(c);
			}
			return result;
		}
	}
}
