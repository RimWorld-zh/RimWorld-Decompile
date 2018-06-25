using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000663 RID: 1635
	public class TaleReference : IExposable
	{
		// Token: 0x04001372 RID: 4978
		private Tale tale;

		// Token: 0x04001373 RID: 4979
		private int seed;

		// Token: 0x0600223B RID: 8763 RVA: 0x00122AB6 File Offset: 0x00120EB6
		public TaleReference()
		{
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x00122ABF File Offset: 0x00120EBF
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x00122AE0 File Offset: 0x00120EE0
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00122AFB File Offset: 0x00120EFB
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00122B21 File Offset: 0x00120F21
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00122B44 File Offset: 0x00120F44
		public string GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00122B6C File Offset: 0x00120F6C
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
