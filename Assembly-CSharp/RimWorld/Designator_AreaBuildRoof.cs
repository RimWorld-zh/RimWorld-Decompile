using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C0 RID: 1984
	public class Designator_AreaBuildRoof : Designator_Area
	{
		// Token: 0x06002BEC RID: 11244 RVA: 0x00173F6C File Offset: 0x0017236C
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

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002BED RID: 11245 RVA: 0x00173FEC File Offset: 0x001723EC
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002BEE RID: 11246 RVA: 0x00174004 File Offset: 0x00172404
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x0017401C File Offset: 0x0017241C
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

		// Token: 0x06002BF0 RID: 11248 RVA: 0x0017408B File Offset: 0x0017248B
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = true;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x001740BC File Offset: 0x001724BC
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

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00174188 File Offset: 0x00172588
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
