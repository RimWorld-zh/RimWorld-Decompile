using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BB RID: 1979
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x06002BE4 RID: 11236 RVA: 0x00174108 File Offset: 0x00172508
		public Designator_AreaAllowedClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			this.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaDelete;
			this.hotKey = KeyBindingDefOf.Misc9;
			this.tutorTag = "AreaAllowedClear";
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x00174184 File Offset: 0x00172584
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x001741C7 File Offset: 0x001725C7
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
