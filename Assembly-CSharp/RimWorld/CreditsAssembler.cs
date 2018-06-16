using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083D RID: 2109
	public class CreditsAssembler
	{
		// Token: 0x06002FB6 RID: 12214 RVA: 0x00198420 File Offset: 0x00196820
		public static IEnumerable<CreditsEntry> AllCredits()
		{
			yield return new CreditRecord_Space(200f);
			yield return new CreditRecord_Title("Credits_Developers".Translate());
			yield return new CreditRecord_Role("", "Tynan Sylvester", null);
			yield return new CreditRecord_Role("", "Piotr Walczak", null);
			yield return new CreditRecord_Role("", "Ben Rog-Wilhelm", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_MusicAndSound".Translate());
			yield return new CreditRecord_Role("", "Alistair Lindsay", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_GameArt".Translate());
			yield return new CreditRecord_Role("", "Rhopunzel", null);
			yield return new CreditRecord_Role("", "Ricardo Tome", null);
			yield return new CreditRecord_Role("", "Kay Fedewa", null);
			yield return new CreditRecord_Role("", "Jon Larson", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credits_AdditionalDevelopment".Translate());
			yield return new CreditRecord_Role("", "Gavan Woolery", null);
			yield return new CreditRecord_Role("", "David 'Rez' Graham", null);
			yield return new CreditRecord_Role("", "Ben Grob", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credits_TitleCommunity".Translate());
			yield return new CreditRecord_Role("Credit_ModDonation", "Zhentar", null);
			yield return new CreditRecord_Role("Credit_ModDonation", "Haplo", null);
			yield return new CreditRecord_Role("Credit_ModDonation", "iame6162013", null);
			yield return new CreditRecord_Role("Credit_ModDonation", "Shinzy", null);
			yield return new CreditRecord_Role("Credit_WritingDonation", "John Woolley", null);
			yield return new CreditRecord_Role("Credit_Moderator", "ItchyFlea", null);
			yield return new CreditRecord_Role("Credit_Moderator", "Ramsis", null);
			yield return new CreditRecord_Role("Credit_Moderator", "Calahan", null);
			yield return new CreditRecord_Role("Credit_Moderator", "milon", null);
			yield return new CreditRecord_Role("Credit_Moderator", "Evul", null);
			yield return new CreditRecord_Role("Credit_Moderator", "Semmy", null);
			yield return new CreditRecord_Role("Credit_WikiMaster", "ZestyLemons", null);
			yield return new CreditRecord_Role("Credit_Tester", "ItchyFlea", null);
			yield return new CreditRecord_Role("Credit_Tester", "Haplo", null);
			yield return new CreditRecord_Role("Credit_Tester", "Mehni", null);
			yield return new CreditRecord_Role("Credit_Tester", "Semmy", null);
			yield return new CreditRecord_Role("Credit_Tester", "drb89", null);
			yield return new CreditRecord_Role("Credit_Tester", "Skissor", null);
			yield return new CreditRecord_Role("Credit_Tester", "MarvinKosh", null);
			yield return new CreditRecord_Role("Credit_Tester", "TLHeart", null);
			yield return new CreditRecord_Role("Credit_Tester", "Sion", null);
			yield return new CreditRecord_Role("Credit_Tester", "Evul", null);
			yield return new CreditRecord_Role("Credit_Tester", "enystrom8734", null);
			yield return new CreditRecord_Role("Credit_Tester", "pheanox", null);
			yield return new CreditRecord_Role("Credit_Tester", "Nasikabatrachus", null);
			yield return new CreditRecord_Role("Credit_Tester", "_alphaBeta_", null);
			yield return new CreditRecord_Role("Credit_Tester", "letharion", null);
			yield return new CreditRecord_Role("Credit_Tester", "Laos", null);
			yield return new CreditRecord_Role("Credit_Tester", "Coenmjc", null);
			yield return new CreditRecord_Role("Credit_Tester", "Gaesatae", null);
			yield return new CreditRecord_Role("Credit_Tester", "skullywag", null);
			yield return new CreditRecord_Role("Credit_Tester", "Vas", null);
			yield return new CreditRecord_Role("Credit_Tester", "theDee05", null);
			yield return new CreditRecord_Role("", "Many other gracious volunteers!", null);
			yield return new CreditRecord_Space(200f);
			foreach (LoadedLanguage lang in LanguageDatabase.AllLoadedLanguages)
			{
				if (lang.info.credits.Count > 0)
				{
					yield return new CreditRecord_Title("Credits_TitleLanguage".Translate(new object[]
					{
						lang.FriendlyNameEnglish
					}));
				}
				foreach (CreditsEntry langCred in lang.info.credits)
				{
					yield return langCred;
				}
			}
			yield break;
		}
	}
}
