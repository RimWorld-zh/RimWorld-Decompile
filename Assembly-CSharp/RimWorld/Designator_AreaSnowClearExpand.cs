using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C4 RID: 1988
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		// Token: 0x06002C0B RID: 11275 RVA: 0x00174B44 File Offset: 0x00172F44
		public Designator_AreaSnowClearExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaSnowClearExpand".Translate();
			this.defaultDesc = "DesignatorAreaSnowClearExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
		}
	}
}
