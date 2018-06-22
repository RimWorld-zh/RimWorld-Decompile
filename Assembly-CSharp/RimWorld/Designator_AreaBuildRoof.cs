using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BC RID: 1980
	public class Designator_AreaBuildRoof : Designator_Area
	{
		// Token: 0x06002BE7 RID: 11239 RVA: 0x001741D8 File Offset: 0x001725D8
		public Designator_AreaBuildRoof()
		{
			this.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc10;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
			this.useMouseIcon = true;
			this.tutorTag = "AreaBuildRoofExpand";
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x00174258 File Offset: 0x00172658
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x00174270 File Offset: 0x00172670
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00174288 File Offset: 0x00172688
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.Fogged(base.Map))
			{
				result = false;
			}
			else
			{
				bool flag = base.Map.areaManager.BuildRoof[c];
				result = !flag;
			}
			return result;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x001742F7 File Offset: 0x001726F7
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = true;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00174328 File Offset: 0x00172728
		public override bool ShowWarningForCell(IntVec3 c)
		{
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
			{
				if (thing.def.plant != null && thing.def.plant.interferesWithRoof)
				{
					Messages.Message("MessageRoofIncompatibleWithPlant".Translate(new object[]
					{
						Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(thing.def.label)
					}), MessageTypeDefOf.CautionInput, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x001743F4 File Offset: 0x001727F4
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
