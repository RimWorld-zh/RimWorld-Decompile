using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CreditsAssembler
	{
		public static IEnumerable<CreditsEntry> AllCredits()
		{
			yield return (CreditsEntry)new CreditRecord_Space(200f);
			yield return (CreditsEntry)new CreditRecord_Title("Credits_Developers".Translate());
			yield return (CreditsEntry)new CreditRecord_Space(50f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_DirectionVarious", "Tynan Sylvester", "@TynanSylvester");
			yield return (CreditsEntry)new CreditRecord_Space(35f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Code", "Piotr Walczak", "@isonil11");
			yield return (CreditsEntry)new CreditRecord_Space(35f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_MusicAndSound", "Alistair Lindsay", "@AlistairLindsay");
			yield return (CreditsEntry)new CreditRecord_Space(35f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_GameArt", "Rhopunzel", "@Rhopunzel");
			yield return (CreditsEntry)new CreditRecord_Space(35f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_GameArt", "Kay Fedewa", "@TheZebrafox");
			yield return (CreditsEntry)new CreditRecord_Space(35f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_MenuArt", "Ricardo Tome", (string)null);
			yield return (CreditsEntry)new CreditRecord_Space(200f);
			yield return (CreditsEntry)new CreditRecord_Title("Credits_TitleCommunity".Translate());
			yield return (CreditsEntry)new CreditRecord_Space(50f);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_ModDonation", "Zhentar", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_ModDonation", "Haplo", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_ModDonation", "iame6162013", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_ModDonation", "Shinzy", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_WritingDonation", "John Woolley", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Moderator", "ItchyFlea", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Moderator", "Ramsis", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Moderator", "Calahan", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Moderator", "milon", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Moderator", "Evul", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Moderator", "Semmy", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_WikiMaster", "ZestyLemons", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "ItchyFlea", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Haplo", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Semmy", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "drb89", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Skissor", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "MarvinKosh", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "TLHeart", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Sion", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Evul", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "enystrom8734", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "pheanox", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Nasikabatrachus", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "_alphaBeta_", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "letharion", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Laos", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Coenmjc", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Gaesatae", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "skullywag", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "Vas", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role("Credit_Tester", "theDee05", (string)null);
			yield return (CreditsEntry)new CreditRecord_Role(string.Empty, "Many other gracious volunteers!", (string)null);
			yield return (CreditsEntry)new CreditRecord_Space(200f);
			foreach (LoadedLanguage allLoadedLanguage in LanguageDatabase.AllLoadedLanguages)
			{
				if (allLoadedLanguage.info.credits.Count > 0)
				{
					yield return (CreditsEntry)new CreditRecord_Title("Credits_TitleLanguage".Translate(allLoadedLanguage.FriendlyNameEnglish));
				}
				List<CreditsEntry>.Enumerator enumerator2 = allLoadedLanguage.info.credits.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						CreditsEntry langCred = enumerator2.Current;
						yield return langCred;
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
		}
	}
}
