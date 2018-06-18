using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F0 RID: 2032
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x06002D13 RID: 11539 RVA: 0x0017ABAC File Offset: 0x00178FAC
		public Designator_ZoneDelete()
		{
			this.defaultLabel = "DesignatorZoneDelete".Translate();
			this.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			this.hotKey = KeyBindingDefOf.Misc4;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x0017AC2C File Offset: 0x0017902C
		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			AcceptanceReport result;
			if (!sq.InBounds(base.Map))
			{
				result = false;
			}
			else if (sq.Fogged(base.Map))
			{
				result = false;
			}
			else if (base.Map.zoneManager.ZoneAt(sq) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x0017ACA4 File Offset: 0x001790A4
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x0017ACE8 File Offset: 0x001790E8
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x040017B8 RID: 6072
		private List<Zone> justDesignated = new List<Zone>();
	}
}
