using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C3 RID: 1987
	public abstract class Designator_AreaSnowClear : Designator_Area
	{
		// Token: 0x06002C05 RID: 11269 RVA: 0x001749EC File Offset: 0x00172DEC
		public Designator_AreaSnowClear(DesignateMode mode)
		{
			this.mode = mode;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "AreaSnowClear";
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x00174A3C File Offset: 0x00172E3C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002C07 RID: 11271 RVA: 0x00174A54 File Offset: 0x00172E54
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x00174A6C File Offset: 0x00172E6C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else
			{
				bool flag = base.Map.areaManager.SnowClear[c];
				if (this.mode == DesignateMode.Add)
				{
					result = !flag;
				}
				else
				{
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00174AD8 File Offset: 0x00172ED8
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.mode == DesignateMode.Add)
			{
				base.Map.areaManager.SnowClear[c] = true;
			}
			else
			{
				base.Map.areaManager.SnowClear[c] = false;
			}
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x00174B24 File Offset: 0x00172F24
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.SnowClear.MarkForDraw();
		}

		// Token: 0x04001799 RID: 6041
		private DesignateMode mode;
	}
}
