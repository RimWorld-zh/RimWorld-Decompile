using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D9 RID: 2009
	public class Designator_Slaughter : Designator
	{
		// Token: 0x040017A7 RID: 6055
		private List<Pawn> justDesignated = new List<Pawn>();

		// Token: 0x06002C88 RID: 11400 RVA: 0x00177738 File Offset: 0x00175B38
		public Designator_Slaughter()
		{
			this.defaultLabel = "DesignatorSlaughter".Translate();
			this.defaultDesc = "DesignatorSlaughterDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Slaughter", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x001777BC File Offset: 0x00175BBC
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002C8A RID: 11402 RVA: 0x001777D4 File Offset: 0x00175BD4
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x001777F0 File Offset: 0x00175BF0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.SlaughterablesInCell(c).Any<Pawn>())
			{
				result = "MessageMustDesignateSlaughterable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x00177850 File Offset: 0x00175C50
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x001778B0 File Offset: 0x00175CB0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			AcceptanceReport result;
			if (pawn != null && pawn.def.race.Animal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null && !pawn.InAggroMentalState)
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x00177930 File Offset: 0x00175D30
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x00177968 File Offset: 0x00175D68
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x001779B8 File Offset: 0x00175DB8
		private IEnumerable<Pawn> SlaughterablesInCell(IntVec3 c)
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
					yield return (Pawn)thingList[i];
				}
			}
			yield break;
		}
	}
}
