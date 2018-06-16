using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C8 RID: 1992
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		// Token: 0x06002C10 RID: 11280 RVA: 0x001748D8 File Offset: 0x00172CD8
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
