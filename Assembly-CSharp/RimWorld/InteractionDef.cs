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
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00066CF0 File Offset: 0x000650F0
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
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00066D2C File Offset: 0x0006512C
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

		// Token: 0x06000B62 RID: 2914 RVA: 0x00066D6A File Offset: 0x0006516A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x0400064B RID: 1611
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x0400064C RID: 1612
		public ThingDef interactionMote;

		// Token: 0x0400064D RID: 1613
		public float socialFightBaseChance = 0f;

		// Token: 0x0400064E RID: 1614
		public ThoughtDef initiatorThought;

		// Token: 0x0400064F RID: 1615
		public SkillDef initiatorXpGainSkill;

		// Token: 0x04000650 RID: 1616
		public int initiatorXpGainAmount;

		// Token: 0x04000651 RID: 1617
		public ThoughtDef recipientThought;

		// Token: 0x04000652 RID: 1618
		public SkillDef recipientXpGainSkill;

		// Token: 0x04000653 RID: 1619
		public int recipientXpGainAmount;

		// Token: 0x04000654 RID: 1620
		[NoTranslate]
		private string symbol;

		// Token: 0x04000655 RID: 1621
		public RulePack logRulesInitiator;

		// Token: 0x04000656 RID: 1622
		public RulePack logRulesRecipient;

		// Token: 0x04000657 RID: 1623
		[Unsaved]
		private InteractionWorker workerInt;

		// Token: 0x04000658 RID: 1624
		[Unsaved]
		private Texture2D symbolTex;
	}
}
