using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

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
