using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000661 RID: 1633
	public class TaleReference : IExposable
	{
		// Token: 0x04001372 RID: 4978
		private Tale tale;

		// Token: 0x04001373 RID: 4979
		private int seed;

		// Token: 0x06002237 RID: 8759 RVA: 0x00122966 File Offset: 0x00120D66
		public TaleReference()
		{
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x0012296F File Offset: 0x00120D6F
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x00122990 File Offset: 0x00120D90
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x001229AB File Offset: 0x00120DAB
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x001229D1 File Offset: 0x00120DD1
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x001229F4 File Offset: 0x00120DF4
		public string GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00122A1C File Offset: 0x00120E1C
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
