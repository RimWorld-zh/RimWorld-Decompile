using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private static Func<Backstory, float> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<PawnBio, float> _003C_003Ef__mg_0024cache1;

		public static void GiveAppropriateBioAndNameTo(Pawn pawn, string requiredLastName)
		{
			if ((Rand.Value < 0.25 || pawn.kindDef.factionLeader) && PawnBioAndNameGenerator.TryGiveSolidBioTo(pawn, requiredLastName))
				return;
			PawnBioAndNameGenerator.GiveShuffledBioTo(pawn, pawn.Faction.def, requiredLastName);
		}

		private static void GiveShuffledBioTo(Pawn pawn, FactionDef factionType, string requiredLastName)
		{
			pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, requiredLastName);
			PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Childhood, ref pawn.story.childhood, factionType);
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20.0)
			{
				PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, BackstorySlot.Adulthood, ref pawn.story.adulthood, factionType);
			}
		}

		private static void FillBackstorySlotShuffled(Pawn pawn, BackstorySlot slot, ref Backstory backstory, FactionDef factionType)
		{
			if (!(from kvp in BackstoryDatabase.allBackstories
			where (byte)((kvp.Value.shuffleable && kvp.Value.spawnCategories.Contains(factionType.backstoryCategory) && kvp.Value.slot == slot) ? ((slot != BackstorySlot.Adulthood || !kvp.Value.requiredWorkTags.OverlapsWithOnAnyWorkType(pawn.story.childhood.workDisables)) ? 1 : 0) : 0) != 0
			select kvp.Value).TryRandomElementByWeight<Backstory>(new Func<Backstory, float>(PawnBioAndNameGenerator.BackstorySelectionWeight), out backstory))
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
			bool result;
			if (pawnBio == null)
			{
				result = false;
			}
			else
			{
				if (pawnBio.name.First == "Tynan" && pawnBio.name.Last == "Sylvester" && Rand.Value < 0.5)
				{
					pawnBio = PawnBioAndNameGenerator.TryGetRandomUnusedSolidBioFor(pawn.Faction.def.backstoryCategory, pawn.kindDef, pawn.gender, requiredLastName);
				}
				if (pawnBio == null)
				{
					result = false;
				}
				else
				{
					pawn.Name = pawnBio.name;
					pawn.story.childhood = pawnBio.childhood;
					if (pawn.ageTracker.AgeBiologicalYearsFloat >= 20.0)
					{
						pawn.story.adulthood = pawnBio.adulthood;
					}
					result = true;
				}
			}
			return result;
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
			PawnBio result;
			while (true)
			{
				result = null;
				if (SolidBioDatabase.allBios.Where((Func<PawnBio, bool>)delegate(PawnBio bio)
				{
					bool result2;
					if (bio.gender != GenderPossibility.Either)
					{
						if (((gender == Gender.Male) ? bio.gender : GenderPossibility.Male) != 0)
						{
							result2 = false;
							goto IL_0108;
						}
						if (gender == Gender.Female && bio.gender != GenderPossibility.Female)
						{
							result2 = false;
							goto IL_0108;
						}
					}
					result2 = ((byte)((requiredLastName.NullOrEmpty() || !(bio.name.Last != requiredLastName)) ? ((prefName == null || bio.name.Equals(prefName)) ? ((!kind.factionLeader || bio.pirateKing) ? (bio.adulthood.spawnCategories.Contains(backstoryCategory) ? ((!bio.name.UsedThisGame) ? 1 : 0) : 0) : 0) : 0) : 0) != 0);
					goto IL_0108;
					IL_0108:
					return result2;
				}).TryRandomElementByWeight<PawnBio>(new Func<PawnBio, float>(PawnBioAndNameGenerator.BioSelectionWeight), out result))
					break;
				if (prefName == null)
					break;
				prefName = null;
			}
			return result;
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
			NameTriple result;
			if (list2.Count == 0)
			{
				Log.Error("Empty solid pawn name list for gender: " + gender + ".");
				result = null;
			}
			else if (nameTriple != null && list2.Contains(nameTriple))
			{
				result = nameTriple;
			}
			else
			{
				list2.Shuffle();
				NameTriple nameTriple2 = result = (from name in list2
				where (byte)((requiredLastName == null || !(name.Last != requiredLastName)) ? ((!name.UsedThisGame) ? 1 : 0) : 0) != 0
				select name).FirstOrDefault();
			}
			return result;
		}

		public static Name GeneratePawnName(Pawn pawn, NameStyle style = NameStyle.Full, string forcedLastName = null)
		{
			Name result;
			switch (style)
			{
			case NameStyle.Full:
			{
				RulePackDef nameGenerator = pawn.RaceProps.GetNameGenerator(pawn.gender);
				if (nameGenerator != null)
				{
					string name = NameGenerator.GenerateName(nameGenerator, (Predicate<string>)((string x) => !new NameSingle(x, false).UsedThisGame), false, (string)null);
					result = new NameSingle(name, false);
				}
				else if (pawn.Faction != null && pawn.Faction.def.pawnNameMaker != null)
				{
					string rawName = NameGenerator.GenerateName(pawn.Faction.def.pawnNameMaker, (Predicate<string>)delegate(string x)
					{
						NameTriple nameTriple4 = NameTriple.FromString(x);
						nameTriple4.ResolveMissingPieces(forcedLastName);
						return !nameTriple4.UsedThisGame;
					}, false, (string)null);
					NameTriple nameTriple = NameTriple.FromString(rawName);
					nameTriple.CapitalizeNick();
					nameTriple.ResolveMissingPieces(forcedLastName);
					result = nameTriple;
				}
				else if (pawn.RaceProps.nameCategory != 0)
				{
					if (Rand.Value < 0.5)
					{
						NameTriple nameTriple2 = PawnBioAndNameGenerator.TryGetRandomUnusedSolidName(pawn.gender, forcedLastName);
						if (nameTriple2 != null)
						{
							result = nameTriple2;
							goto IL_01b4;
						}
					}
					result = PawnBioAndNameGenerator.GeneratePawnName_Shuffled(pawn, forcedLastName);
				}
				else
				{
					Log.Error("No name making method for " + pawn);
					NameTriple nameTriple3 = NameTriple.FromString(pawn.def.label);
					nameTriple3.ResolveMissingPieces((string)null);
					result = nameTriple3;
				}
				break;
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
				result = new NameSingle(text, true);
				break;
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
			goto IL_01b4;
			IL_01b4:
			return result;
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

		private static float BackstorySelectionWeight(Backstory bs)
		{
			return PawnBioAndNameGenerator.WorkDisablesSelectionWeight(bs.workDisables);
		}

		private static float BioSelectionWeight(PawnBio bio)
		{
			return PawnBioAndNameGenerator.WorkDisablesSelectionWeight(bio.adulthood.workDisables & bio.childhood.workDisables);
		}

		private static float WorkDisablesSelectionWeight(WorkTags wt)
		{
			return (float)((((int)wt & 2) == 0) ? ((((int)wt & 4096) == 0) ? ((((int)wt & 32768) == 0) ? ((((int)wt & 128) == 0) ? 1.0 : 3.0) : 3.0) : 0.75) : 0.44999998807907104);
		}
	}
}
