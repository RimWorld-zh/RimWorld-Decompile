using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020002A7 RID: 679
	public class InteractionDef : Def
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00066C88 File Offset: 0x00065088
		public InteractionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InteractionWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00066CC4 File Offset: 0x000650C4
		public Texture2D Symbol
		{
			get
			{
				if (this.symbolTex == null)
				{
					this.symbolTex = ContentFinder<Texture2D>.Get(this.symbol, true);
				}
				return this.symbolTex;
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00066D02 File Offset: 0x00065102
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x0400064C RID: 1612
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x0400064D RID: 1613
		public ThingDef interactionMote;

		// Token: 0x0400064E RID: 1614
		public float socialFightBaseChance = 0f;

		// Token: 0x0400064F RID: 1615
		public ThoughtDef initiatorThought;

		// Token: 0x04000650 RID: 1616
		public SkillDef initiatorXpGainSkill;

		// Token: 0x04000651 RID: 1617
		public int initiatorXpGainAmount;

		// Token: 0x04000652 RID: 1618
		public ThoughtDef recipientThought;

		// Token: 0x04000653 RID: 1619
		public SkillDef recipientXpGainSkill;

		// Token: 0x04000654 RID: 1620
		public int recipientXpGainAmount;

		// Token: 0x04000655 RID: 1621
		[NoTranslate]
		private string symbol;

		// Token: 0x04000656 RID: 1622
		public RulePack logRulesInitiator;

		// Token: 0x04000657 RID: 1623
		public RulePack logRulesRecipient;

		// Token: 0x04000658 RID: 1624
		[Unsaved]
		private InteractionWorker workerInt;

		// Token: 0x04000659 RID: 1625
		[Unsaved]
		private Texture2D symbolTex;
	}
}
