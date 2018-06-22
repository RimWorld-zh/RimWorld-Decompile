using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BF RID: 1983
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		// Token: 0x06002BF5 RID: 11253 RVA: 0x0017457C File Offset: 0x0017297C
		public Designator_AreaHomeExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaHomeExpand".Translate();
			this.defaultDesc = "DesignatorAreaHomeExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
			this.tutorTag = "AreaHomeExpand";
		}
	}
}
