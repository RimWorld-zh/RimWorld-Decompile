using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DB RID: 2011
	public class Designator_Slaughter : Designator
	{
		// Token: 0x06002C8A RID: 11402 RVA: 0x00177118 File Offset: 0x00175518
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

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002C8B RID: 11403 RVA: 0x0017719C File Offset: 0x0017559C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x001771B4 File Offset: 0x001755B4
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x001771D0 File Offset: 0x001755D0
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

		// Token: 0x06002C8E RID: 11406 RVA: 0x00177230 File Offset: 0x00175630
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x00177290 File Offset: 0x00175690
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

		// Token: 0x06002C90 RID: 11408 RVA: 0x00177310 File Offset: 0x00175710
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x00177348 File Offset: 0x00175748
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x00177398 File Offset: 0x00175798
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

		// Token: 0x040017A5 RID: 6053
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
