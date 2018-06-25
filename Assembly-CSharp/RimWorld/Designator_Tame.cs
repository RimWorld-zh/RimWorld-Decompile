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
		// Token: 0x040017A5 RID: 6053
		private List<Pawn> justDesignated = new List<Pawn>();

		// Token: 0x06002CA6 RID: 11430 RVA: 0x00178154 File Offset: 0x00176554
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
		// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x001781E4 File Offset: 0x001765E4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x001781FC File Offset: 0x001765FC
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x00178218 File Offset: 0x00176618
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

		// Token: 0x06002CAA RID: 11434 RVA: 0x00178278 File Offset: 0x00176678
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.TameablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x001782D8 File Offset: 0x001766D8
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

		// Token: 0x06002CAC RID: 11436 RVA: 0x00178360 File Offset: 0x00176760
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

		// Token: 0x06002CAD RID: 11437 RVA: 0x00178420 File Offset: 0x00176820
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x00178474 File Offset: 0x00176874
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
