using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DE RID: 2014
	public class Designator_Tame : Designator
	{
		// Token: 0x040017A9 RID: 6057
		private List<Pawn> justDesignated = new List<Pawn>();

		// Token: 0x06002CA5 RID: 11429 RVA: 0x001783B8 File Offset: 0x001767B8
		public Designator_Tame()
		{
			this.defaultLabel = "DesignatorTame".Translate();
			this.defaultDesc = "DesignatorTameDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.tutorTag = "Tame";
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x00178448 File Offset: 0x00176848
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x00178460 File Offset: 0x00176860
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x0017847C File Offset: 0x0017687C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!this.TameablesInCell(c).Any<Pawn>())
			{
				result = "MessageMustDesignateTameable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x001784DC File Offset: 0x001768DC
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.TameablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x0017853C File Offset: 0x0017693C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			AcceptanceReport result;
			if (pawn != null && pawn.AnimalOrWildMan() && pawn.Faction == null && pawn.RaceProps.wildness < 1f && !pawn.HostileTo(t) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x001785C4 File Offset: 0x001769C4
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					TameUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind));
				}
			}
			this.justDesignated.Clear();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTaming, KnowledgeAmount.Total);
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x00178684 File Offset: 0x00176A84
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x001786D8 File Offset: 0x00176AD8
		private IEnumerable<Pawn> TameablesInCell(IntVec3 c)
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
