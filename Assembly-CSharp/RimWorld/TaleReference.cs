using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000663 RID: 1635
	public class TaleReference : IExposable
	{
		// Token: 0x04001376 RID: 4982
		private Tale tale;

		// Token: 0x04001377 RID: 4983
		private int seed;

		// Token: 0x0600223A RID: 8762 RVA: 0x00122D1E File Offset: 0x0012111E
		public TaleReference()
		{
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00122D27 File Offset: 0x00121127
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x00122D48 File Offset: 0x00121148
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00122D63 File Offset: 0x00121163
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00122D89 File Offset: 0x00121189
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00122DAC File Offset: 0x001211AC
		public string GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00122DD4 File Offset: 0x001211D4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"TaleReference(tale=",
				(this.tale != null) ? this.tale.ToString() : "null",
				", seed=",
				this.seed,
				")"
			});
		}
	}
}
