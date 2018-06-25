using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B5A RID: 2906
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x04002A2F RID: 10799
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04002A30 RID: 10800
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04002A31 RID: 10801
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);

		// Token: 0x06003F82 RID: 16258 RVA: 0x002173E0 File Offset: 0x002157E0
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

		// Token: 0x06003F83 RID: 16259 RVA: 0x00217440 File Offset: 0x00215840
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

		// Token: 0x06003F84 RID: 16260 RVA: 0x002174A0 File Offset: 0x002158A0
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

		// Token: 0x06003F85 RID: 16261 RVA: 0x00217500 File Offset: 0x00215900
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

		// Token: 0x06003F86 RID: 16262 RVA: 0x00217560 File Offset: 0x00215960
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
