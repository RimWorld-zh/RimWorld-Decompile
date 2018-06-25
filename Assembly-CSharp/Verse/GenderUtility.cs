using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B5B RID: 2907
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x04002A36 RID: 10806
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04002A37 RID: 10807
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04002A38 RID: 10808
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);

		// Token: 0x06003F82 RID: 16258 RVA: 0x002176C0 File Offset: 0x00215AC0
		public static string GetLabel(this Gender gender)
		{
			string result;
			if (gender != Gender.None)
			{
				if (gender != Gender.Male)
				{
					if (gender != Gender.Female)
					{
						throw new ArgumentException();
					}
					result = "Female".Translate();
				}
				else
				{
					result = "Male".Translate();
				}
			}
			else
			{
				result = "NoneLower".Translate();
			}
			return result;
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x00217720 File Offset: 0x00215B20
		public static string GetPronoun(this Gender gender)
		{
			string result;
			if (gender != Gender.None)
			{
				if (gender != Gender.Male)
				{
					if (gender != Gender.Female)
					{
						throw new ArgumentException();
					}
					result = "Proshe".Translate();
				}
				else
				{
					result = "Prohe".Translate();
				}
			}
			else
			{
				result = "Proit".Translate();
			}
			return result;
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x00217780 File Offset: 0x00215B80
		public static string GetPossessive(this Gender gender)
		{
			string result;
			if (gender != Gender.None)
			{
				if (gender != Gender.Male)
				{
					if (gender != Gender.Female)
					{
						throw new ArgumentException();
					}
					result = "Proher".Translate();
				}
				else
				{
					result = "Prohis".Translate();
				}
			}
			else
			{
				result = "Proits".Translate();
			}
			return result;
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x002177E0 File Offset: 0x00215BE0
		public static string GetObjective(this Gender gender)
		{
			string result;
			if (gender != Gender.None)
			{
				if (gender != Gender.Male)
				{
					if (gender != Gender.Female)
					{
						throw new ArgumentException();
					}
					result = "ProherObj".Translate();
				}
				else
				{
					result = "ProhimObj".Translate();
				}
			}
			else
			{
				result = "ProitObj".Translate();
			}
			return result;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00217840 File Offset: 0x00215C40
		public static Texture2D GetIcon(this Gender gender)
		{
			Texture2D result;
			if (gender != Gender.None)
			{
				if (gender != Gender.Male)
				{
					if (gender != Gender.Female)
					{
						throw new ArgumentException();
					}
					result = GenderUtility.FemaleIcon;
				}
				else
				{
					result = GenderUtility.MaleIcon;
				}
			}
			else
			{
				result = GenderUtility.GenderlessIcon;
			}
			return result;
		}
	}
}
