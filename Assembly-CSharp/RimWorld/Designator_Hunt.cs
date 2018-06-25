using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CF RID: 1999
	public class Designator_Hunt : Designator
	{
		// Token: 0x040017A4 RID: 6052
		private List<Pawn> justDesignated = new List<Pawn>();

		// Token: 0x06002C4E RID: 11342 RVA: 0x00176230 File Offset: 0x00174630
		public Designator_Hunt()
		{
			this.defaultLabel = "DesignatorHunt".Translate();
			this.defaultDesc = "DesignatorHuntDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002C4F RID: 11343 RVA: 0x001762B4 File Offset: 0x001746B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x001762CC File Offset: 0x001746CC
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x001762E8 File Offset: 0x001746E8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.HuntablesInCell(c).Any<Pawn>())
			{
				result = "MessageMustDesignateHuntable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00176348 File Offset: 0x00174748
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.HuntablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x001763A8 File Offset: 0x001747A8
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			AcceptanceReport result;
			if (pawn != null && pawn.AnimalOrWildMan() && pawn.Faction == null && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x00176410 File Offset: 0x00174810
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x00176464 File Offset: 0x00174864
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					HuntUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind));
				}
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0017651C File Offset: 0x0017491C
		private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
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
