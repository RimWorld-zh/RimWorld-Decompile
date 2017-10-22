using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_ZoneDelete : Designator_Zone
	{
		private List<Zone> justDesignated = new List<Zone>();

		public Designator_ZoneDelete()
		{
			base.defaultLabel = "DesignatorZoneDelete".Translate();
			base.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			base.soundDragSustain = SoundDefOf.DesignateDragAreaDelete;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaDeleteChanged;
			base.soundSucceeded = SoundDefOf.DesignateZoneDelete;
			base.useMouseIcon = true;
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			base.hotKey = KeyBindingDefOf.Misc4;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			return sq.InBounds(base.Map) ? ((!sq.Fogged(base.Map)) ? ((base.Map.zoneManager.ZoneAt(sq) != null) ? true : false) : false) : false;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}
	}
}
