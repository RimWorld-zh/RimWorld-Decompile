using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E2 RID: 1250
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x04000CFE RID: 3326
		public GenderPossibility gender;

		// Token: 0x04000CFF RID: 3327
		public NameTriple name;

		// Token: 0x04000D00 RID: 3328
		public Backstory childhood;

		// Token: 0x04000D01 RID: 3329
		public Backstory adulthood;

		// Token: 0x04000D02 RID: 3330
		public bool pirateKing = false;

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x000C5DD4 File Offset: 0x000C41D4
		public PawnBioType BioType
		{
			get
			{
				PawnBioType result;
				if (this.pirateKing)
				{
					result = PawnBioType.PirateKing;
				}
				else if (this.adulthood != null)
				{
					result = PawnBioType.BackstoryInGame;
				}
				else
				{
					result = PawnBioType.Undefined;
				}
				return result;
			}
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000C5E0E File Offset: 0x000C420E
		public void PostLoad()
		{
			if (this.childhood != null)
			{
				this.childhood.PostLoad();
			}
			if (this.adulthood != null)
			{
				this.adulthood.PostLoad();
			}
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000C5E40 File Offset: 0x000C4240
		public void ResolveReferences()
		{
			if (this.adulthood.spawnCategories.Count == 1 && this.adulthood.spawnCategories[0] == "Trader")
			{
				this.adulthood.spawnCategories.Add("Civil");
			}
			if (this.childhood != null)
			{
				this.childhood.ResolveReferences();
			}
			if (this.adulthood != null)
			{
				this.adulthood.ResolveReferences();
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000C5EC8 File Offset: 0x000C42C8
		public IEnumerable<string> ConfigErrors()
		{
			if (this.childhood != null)
			{
				foreach (string error in this.childhood.ConfigErrors(true))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.childhood.title,
						": ",
						error
					});
				}
			}
			if (this.adulthood != null)
			{
				foreach (string error2 in this.adulthood.ConfigErrors(false))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.adulthood.title,
						": ",
						error2
					});
				}
			}
			yield break;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000C5EF4 File Offset: 0x000C42F4
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}
	}
}
