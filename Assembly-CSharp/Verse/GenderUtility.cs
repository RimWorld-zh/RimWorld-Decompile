using System;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);

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

		// Note: this type is marked as 'beforefieldinit'.
		static GenderUtility()
		{
		}
	}
}
