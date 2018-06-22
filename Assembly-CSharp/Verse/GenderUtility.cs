using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B58 RID: 2904
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x06003F7F RID: 16255 RVA: 0x00217304 File Offset: 0x00215704
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

		// Token: 0x06003F80 RID: 16256 RVA: 0x00217364 File Offset: 0x00215764
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

		// Token: 0x06003F81 RID: 16257 RVA: 0x002173C4 File Offset: 0x002157C4
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

		// Token: 0x06003F82 RID: 16258 RVA: 0x00217424 File Offset: 0x00215824
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

		// Token: 0x06003F83 RID: 16259 RVA: 0x00217484 File Offset: 0x00215884
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

		// Token: 0x04002A2F RID: 10799
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04002A30 RID: 10800
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04002A31 RID: 10801
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);
	}
}
