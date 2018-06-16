using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D9 RID: 2009
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x06002C81 RID: 11393 RVA: 0x00177078 File Offset: 0x00175478
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x001770C8 File Offset: 0x001754C8
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
