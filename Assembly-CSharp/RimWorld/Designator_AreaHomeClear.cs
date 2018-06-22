using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C0 RID: 1984
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x06002BF6 RID: 11254 RVA: 0x001745EC File Offset: 0x001729EC
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
