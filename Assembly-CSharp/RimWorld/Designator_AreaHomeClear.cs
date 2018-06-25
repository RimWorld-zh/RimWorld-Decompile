using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C2 RID: 1986
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x06002BFA RID: 11258 RVA: 0x0017473C File Offset: 0x00172B3C
		public Designator_AreaHomeClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaHomeClear".Translate();
			this.defaultDesc = "DesignatorAreaHomeClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaDelete;
		}
	}
}
