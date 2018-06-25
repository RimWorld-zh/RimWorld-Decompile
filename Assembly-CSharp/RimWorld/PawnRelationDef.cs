using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B8 RID: 696
	public class PawnRelationDef : Def
	{
		// Token: 0x040006B4 RID: 1716
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x040006B5 RID: 1717
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040006B6 RID: 1718
		public float importance;

		// Token: 0x040006B7 RID: 1719
		public bool implied;

		// Token: 0x040006B8 RID: 1720
		public bool reflexive;

		// Token: 0x040006B9 RID: 1721
		public int opinionOffset;

		// Token: 0x040006BA RID: 1722
		public float generationChanceFactor;

		// Token: 0x040006BB RID: 1723
		public float attractionFactor = 1f;

		// Token: 0x040006BC RID: 1724
		public float incestOpinionOffset;

		// Token: 0x040006BD RID: 1725
		public bool familyByBloodRelation;

		// Token: 0x040006BE RID: 1726
		public ThoughtDef diedThought;

		// Token: 0x040006BF RID: 1727
		public ThoughtDef diedThoughtFemale;

		// Token: 0x040006C0 RID: 1728
		public ThoughtDef soldThought;

		// Token: 0x040006C1 RID: 1729
		public ThoughtDef killedThought;

		// Token: 0x040006C2 RID: 1730
		public ThoughtDef killedThoughtFemale;

		// Token: 0x040006C3 RID: 1731
		[Unsaved]
		private PawnRelationWorker workerInt = null;

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00068D34 File Offset: 0x00067134
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

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00068D80 File Offset: 0x00067180
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

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00068DC4 File Offset: 0x000671C4
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00068DE8 File Offset: 0x000671E8
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

		// Token: 0x06000BAA RID: 2986 RVA: 0x00068E28 File Offset: 0x00067228
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

		// Token: 0x06000BAB RID: 2987 RVA: 0x00068E68 File Offset: 0x00067268
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
	}
}
