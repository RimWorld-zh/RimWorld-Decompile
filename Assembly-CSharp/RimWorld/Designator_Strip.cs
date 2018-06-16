using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DF RID: 2015
	public class Designator_Strip : Designator
	{
		// Token: 0x06002C9F RID: 11423 RVA: 0x001779DC File Offset: 0x00175DDC
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

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002CA0 RID: 11424 RVA: 0x00177A54 File Offset: 0x00175E54
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x00177A6C File Offset: 0x00175E6C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Strip;
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x00177A88 File Offset: 0x00175E88
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

		// Token: 0x06002CA3 RID: 11427 RVA: 0x00177AE8 File Offset: 0x00175EE8
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StrippablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x00177B48 File Offset: 0x00175F48
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

		// Token: 0x06002CA5 RID: 11429 RVA: 0x00177B90 File Offset: 0x00175F90
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x00177BB4 File Offset: 0x00175FB4
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
