using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x06002BFE RID: 11262 RVA: 0x00174478 File Offset: 0x00172878
		public Designator_AreaIgnoreRoof()
		{
			this.defaultLabel = "DesignatorAreaIgnoreRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaIgnoreRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/IgnoreRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002BFF RID: 11263 RVA: 0x001744EC File Offset: 0x001728EC
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x00174504 File Offset: 0x00172904
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x0017451C File Offset: 0x0017291C
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
				result = (base.Map.areaManager.BuildRoof[c] || base.Map.areaManager.NoRoof[c]);
			}
			return result;
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x001745A4 File Offset: 0x001729A4
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x001745D5 File Offset: 0x001729D5
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
