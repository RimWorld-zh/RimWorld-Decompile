using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B6 RID: 694
	public class PawnRelationDef : Def
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00068B80 File Offset: 0x00066F80
		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00068BCC File Offset: 0x00066FCC
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			string label;
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				label = this.labelFemale;
			}
			else
			{
				label = this.label;
			}
			return label;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00068C10 File Offset: 0x00067010
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00068C34 File Offset: 0x00067034
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			ThoughtDef result;
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				result = this.diedThoughtFemale;
			}
			else
			{
				result = this.diedThought;
			}
			return result;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00068C74 File Offset: 0x00067074
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			ThoughtDef result;
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				result = this.killedThoughtFemale;
			}
			else
			{
				result = this.killedThought;
			}
			return result;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00068CB4 File Offset: 0x000670B4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string c in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.implied && this.reflexive)
			{
				yield return this.defName + ": implied relations can't use the \"reflexive\" option.";
				this.reflexive = false;
			}
			yield break;
		}

		// Token: 0x040006B3 RID: 1715
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x040006B4 RID: 1716
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040006B5 RID: 1717
		public float importance;

		// Token: 0x040006B6 RID: 1718
		public bool implied;

		// Token: 0x040006B7 RID: 1719
		public bool reflexive;

		// Token: 0x040006B8 RID: 1720
		public int opinionOffset;

		// Token: 0x040006B9 RID: 1721
		public float generationChanceFactor;

		// Token: 0x040006BA RID: 1722
		public float attractionFactor = 1f;

		// Token: 0x040006BB RID: 1723
		public float incestOpinionOffset;

		// Token: 0x040006BC RID: 1724
		public bool familyByBloodRelation;

		// Token: 0x040006BD RID: 1725
		public ThoughtDef diedThought;

		// Token: 0x040006BE RID: 1726
		public ThoughtDef diedThoughtFemale;

		// Token: 0x040006BF RID: 1727
		public ThoughtDef soldThought;

		// Token: 0x040006C0 RID: 1728
		public ThoughtDef killedThought;

		// Token: 0x040006C1 RID: 1729
		public ThoughtDef killedThoughtFemale;

		// Token: 0x040006C2 RID: 1730
		[Unsaved]
		private PawnRelationWorker workerInt = null;
	}
}
