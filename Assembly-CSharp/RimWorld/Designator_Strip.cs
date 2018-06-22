using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DB RID: 2011
	public class Designator_Strip : Designator
	{
		// Token: 0x06002C9A RID: 11418 RVA: 0x00177C48 File Offset: 0x00176048
		public Designator_Strip()
		{
			this.defaultLabel = "DesignatorStrip".Translate();
			this.defaultDesc = "DesignatorStripDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x00177CC0 File Offset: 0x001760C0
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002C9C RID: 11420 RVA: 0x00177CD8 File Offset: 0x001760D8
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Strip;
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x00177CF4 File Offset: 0x001760F4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.StrippablesInCell(c).Any<Thing>())
			{
				result = "MessageMustDesignateStrippable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x00177D54 File Offset: 0x00176154
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StrippablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x00177DB4 File Offset: 0x001761B4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result;
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				result = false;
			}
			else
			{
				result = StrippableUtility.CanBeStrippedByColony(t);
			}
			return result;
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x00177DFC File Offset: 0x001761FC
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x00177E20 File Offset: 0x00176220
		private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
			}
			yield break;
		}
	}
}
