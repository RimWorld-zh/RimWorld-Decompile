using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnBioAndNameGenerator
	{
		private const float MinAgeForAdulthood = 20f;

		private const float SolidBioChance = 0.25f;

		private const float SolidNameChance = 0.5f;

		private const float TryPreferredNameChance_Bio = 0.5f;

		private const float TryPreferredNameChance_Name = 0.5f;

		private const float ShuffledNicknameChance = 0.15f;

		public static void GiveAppropriateBioAndNameTo(Pawn pawn, string requiredLastName)
		{
			if ((Rand.Value < 0.25 || pawn.kindDef.factionLeader) && PawnBioAndNameGenerator.TryGiveSolidBioTo(pawn, requiredLastName))
				return;
			PawnBioAndNameGenerator.GiveShuffledBioTo(pawn, pawn.Faction.def, requiredLastName);
		}

		private static void GiveShuffledBioTo(Pawn pawn, FactionDef factionType, string requiredLastName)
		{
			pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, requiredLastName);
			PawnBioAndNameGenerator.SetBackstoryInSlot(pawn, BackstorySlot.Childhood, ref pawn.story.childhood, factionType);
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20.0)
			{
				PawnBioAndNameGenerator.SetBackstoryInSlot(pawn, BackstorySlot.Adulthood, ref pawn.story.adulthood, factionType);
			}
		}

		private static void SetBackstoryInSlot(Pawn pawn, BackstorySlot slot, ref Backstory backstory, FactionDef factionType)
		{
			if (!(from kvp in BackstoryDatabase.allBackstories.Where((Func<KeyValuePair<string, Backstory>, bool>)delegate(KeyValuePair<string, Backstory> kvp)
			{
				if (kvp.Value.shuffleable && kvp.Value.spawnCategories.Contains(factionType.backstoryCategory) && kvp.Value.slot == slot)
				{
					if (slot == BackstorySlot.Adulthood && kvp.Value.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.childhood.workDisables))
					{
						return false;
					}
					return true;
				}
				return false;
			})
			select kvp.Value).TryRandomElement<Backstory>(out backstory))
			{
				Log.Error("No shuffled " + slot + " found for " + pawn + " of " + factionType + ". Defaulting.");
				backstory = (from kvp in BackstoryDatabase.allBackstories
				where kvp.Value.slot == slot
				select kvp).RandomElement().Value;
			}
		}

		private static bool TryGiveSolidBioTo(Pawn pawn, string requiredLastName)
		{
			PawnBio pawnBio = PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(pawn.Faction.def.backstoryCategory, pawn.kindDef, pawn.gender, requiredLastName);
			if (pawnBio == null)
			{
				return false;
			}
			if (pawnBio.name.First == "Tynan" && pawnBio.name.Last == "Sylvester" && Rand.Value < 0.5)
			{
				pawnBio = PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(pawn.Faction.def.backstoryCategory, pawn.kindDef, pawn.gender, requiredLastName);
			}
			if (pawnBio == null)
			{
				return false;
			}
			pawn.Name = pawnBio.name;
			pawn.story.childhood = pawnBio.childhood;
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20.0)
			{
				pawn.story.adulthood = pawnBio.adulthood;
			}
			return true;
		}

		private static PawnBio TryGetRandomUnusedSolidBioFor(string backstoryCategory, PawnKindDef kind, Gender gender, string requiredLastName)
		{
			NameTriple prefName = null;
			if (Rand.Value < 0.5)
			{
				prefName = Prefs.RandomPreferredName();
				if (prefName != null && (prefName.UsedThisGame || (requiredLastName != null && prefName.Last != requiredLastName)))
				{
					prefName = null;
				}
			}
			SolidBioDatabase.allBios.Shuffle();
			PawnBio pawnBio;
			while (true)
			{
				pawnBio = SolidBioDatabase.allBios.FirstOrDefault((Func<PawnBio, bool>)delegate(PawnBio bio)
				{
					if (bio.gender != GenderPossibility.Either)
					{
						if (((gender == Gender.Male) ? bio.gender : GenderPossibility.Male) != 0)
						{
							return false;
						}
						if (gender == Gender.Female && bio.gender != GenderPossibility.Female)
						{
							return false;
						}
					}
					if (!requiredLastName.NullOrEmpty() && bio.name.Last != requiredLastName)
					{
						return false;
					}
					if (prefName != null && !bio.name.Equals(prefName))
					{
						return false;
					}
					if (kind.factionLeader && !bio.pirateKing)
					{
						return false;
					}
					if (!bio.adulthood.spawnCategories.Contains(backstoryCategory))
					{
						return false;
					}
					if (bio.name.UsedThisGame)
					{
						return false;
					}
					return true;
				});
				if (pawnBio != null)
					break;
				if (prefName == null)
					break;
				prefName = null;
			}
			return pawnBio;
		}

		public static NameTriple TryGetRandomUnusedSolidName(Gender gender, string requiredLastName = null)
		{
			NameTriple nameTriple = null;
			if (Rand.Value < 0.5)
			{
				nameTriple = Prefs.RandomPreferredName();
				if (nameTriple != null && (nameTriple.UsedThisGame || (requiredLastName != null && nameTriple.Last != requiredLastName)))
				{
					nameTriple = null;
				}
			}
			List<NameTriple> listForGender = PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Either);
			List<NameTriple> list = (gender != Gender.Male) ? PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Female) : PawnNameDatabaseSolid.GetListForGender(GenderPossibility.Male);
			float num = (float)(((float)listForGender.Count + 0.10000000149011612) / ((float)(listForGender.Count + list.Count) + 0.10000000149011612));
			List<NameTriple> list2 = (!(Rand.Value < num)) ? list : listForGender;
			if (list2.Count == 0)
			{
				Log.Error("Empty solid pawn name list for gender: " + gender + ".");
				return null;
			}
			if (nameTriple != null && list2.Contains(nameTriple))
			{
				return nameTriple;
			}
			list2.Shuffle();
			return list2.Where((Func<NameTriple, bool>)delegate(NameTriple name)
			{
				if (requiredLastName != null && name.Last != requiredLastName)
				{
					return false;
				}
				if (name.UsedThisGame)
				{
					return false;
				}
				return true;
			}).FirstOrDefault();
		}

		public static Name GeneratePawnName(Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null)
		{
			switch (style)
			{
			case NameStyle.Full:
			{
				RulePackDef nameGenerator = pawn.RaceProps.GetNameGenerator(pawn.gender);
				if (nameGenerator != null)
				{
					string name = NameGenerator.GenerateName(nameGenerator, (Predicate<string>)((string x) => !new NameSingle(x, false).UsedThisGame), false);
					return new NameSingle(name, false);
				}
				if (pawn.Faction != null && pawn.Faction.def.pawnNameMaker != null)
				{
					string rawName = NameGenerator.GenerateName(pawn.Faction.def.pawnNameMaker, (Predicate<string>)delegate(string x)
					{
						NameTriple nameTriple4 = NameTriple.FromString(x);
						nameTriple4.ResolveMissingPieces(forcedLastName);
						return !nameTriple4.UsedThisGame;
					}, false);
					NameTriple nameTriple = NameTriple.FromString(rawName);
					nameTriple.CapitalizeNick();
					nameTriple.ResolveMissingPieces(forcedLastName);
					return nameTriple;
				}
				if (pawn.RaceProps.nameCategory != 0)
				{
					if (Rand.Value < 0.5)
					{
						NameTriple nameTriple2 = PawnBioAndNameGenerator.TryGetRandomUnusedSolidName(pawn.gender, forcedLastName);
						if (nameTriple2 != null)
						{
							return nameTriple2;
						}
					}
					return PawnBioAndNameGenerator.GeneratePawnName_Shuffled(pawn, forcedLastName);
				}
				Log.Error("No name making method for " + pawn);
				NameTriple nameTriple3 = NameTriple.FromString(pawn.def.label);
				nameTriple3.ResolveMissingPieces((string)null);
				return nameTriple3;
			}
			case NameStyle.Numeric:
			{
				int num = 1;
				string text;
				while (true)
				{
					text = pawn.KindLabel + " " + num.ToString();
					if (!NameUseChecker.NameSingleIsUsed(text))
						break;
					num++;
				}
				return new NameSingle(text, true);
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
		}

		private static NameTriple GeneratePawnName_Shuffled(Pawn pawn, string forcedLastName = null)
		{
			PawnNameCategory pawnNameCategory = pawn.RaceProps.nameCategory;
			if (pawnNameCategory == PawnNameCategory.NoName)
			{
				Log.Message("Can't create a name of type NoName. Defaulting to HumanStandard.");
				pawnNameCategory = PawnNameCategory.HumanStandard;
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(pawnNameCategory);
			string name = nameBank.GetName(PawnNameSlot.First, pawn.gender);
			string text = (forcedLastName == null) ? nameBank.GetName(PawnNameSlot.Last, Gender.None) : forcedLastName;
			int num = 0;
			string nick;
			while (true)
			{
				num++;
				if (Rand.Value < 0.15000000596046448)
				{
					Gender gender = pawn.gender;
					if (Rand.Value < 0.5)
					{
						gender = Gender.None;
					}
					nick = nameBank.GetName(PawnNameSlot.Nick, gender);
				}
				else if (Rand.Value < 0.5)
				{
					nick = name;
				}
				else
				{
					nick = text;
				}
				if (num >= 50)
					break;
				if (!NameUseChecker.AllPawnsNamesEverUsed.Any((Func<Name, bool>)delegate(Name x)
				{
					NameTriple nameTriple = x as NameTriple;
					return nameTriple != null && nameTriple.Nick == nick;
				}))
					break;
			}
			return new NameTriple(name, nick, text);
		}
	}
}
