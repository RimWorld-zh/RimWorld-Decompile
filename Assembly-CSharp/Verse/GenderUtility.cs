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
			switch (gender)
			{
			case Gender.None:
			{
				result = "NoneLower".Translate();
				break;
			}
			case Gender.Male:
			{
				result = "Male".Translate();
				break;
			}
			case Gender.Female:
			{
				result = "Female".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}

		public static string GetPronoun(this Gender gender)
		{
			string result;
			switch (gender)
			{
			case Gender.None:
			{
				result = "Proit".Translate();
				break;
			}
			case Gender.Male:
			{
				result = "Prohe".Translate();
				break;
			}
			case Gender.Female:
			{
				result = "Proshe".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}

		public static string GetPossessive(this Gender gender)
		{
			string result;
			switch (gender)
			{
			case Gender.None:
			{
				result = "Proits".Translate();
				break;
			}
			case Gender.Male:
			{
				result = "Prohis".Translate();
				break;
			}
			case Gender.Female:
			{
				result = "Proher".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}

		public static string GetObjective(this Gender gender)
		{
			string result;
			switch (gender)
			{
			case Gender.None:
			{
				result = "ProitObj".Translate();
				break;
			}
			case Gender.Male:
			{
				result = "ProhimObj".Translate();
				break;
			}
			case Gender.Female:
			{
				result = "ProherObj".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}

		public static Texture2D GetIcon(this Gender gender)
		{
			Texture2D result;
			switch (gender)
			{
			case Gender.None:
			{
				result = GenderUtility.GenderlessIcon;
				break;
			}
			case Gender.Male:
			{
				result = GenderUtility.MaleIcon;
				break;
			}
			case Gender.Female:
			{
				result = GenderUtility.FemaleIcon;
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}
	}
}
