using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class CreditsAssembler
	{
		public CreditsAssembler()
		{
		}

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
			yield return new CreditRecord_Role("Credit_Moderator", "MarvinKosh", null);
			yield return new CreditRecord_Role("Credit_WikiMaster", "ZestyLemons", null);
			yield return new CreditRecord_Role("Credit_Tester", "ItchyFlea", null);
			yield return new CreditRecord_Role("Credit_Tester", "Haplo", null);
			yield return new CreditRecord_Role("Credit_Tester", "Mehni", null);
			yield return new CreditRecord_Role("Credit_Tester", "Vas", null);
			yield return new CreditRecord_Role("Credit_Tester", "XeoNovaDan", null);
			yield return new CreditRecord_Role("Credit_Tester", "JimmyAgnt007", null);
			yield return new CreditRecord_Role("Credit_Tester", "Goldenpotatoes", null);
			yield return new CreditRecord_Role("Credit_Tester", "_alphaBeta_", null);
			yield return new CreditRecord_Role("Credit_Tester", "TheDee05", null);
			yield return new CreditRecord_Role("Credit_Tester", "Drb89", null);
			yield return new CreditRecord_Role("Credit_Tester", "Skissor", null);
			yield return new CreditRecord_Role("Credit_Tester", "MarvinKosh", null);
			yield return new CreditRecord_Role("Credit_Tester", "Evul", null);
			yield return new CreditRecord_Role("Credit_Tester", "Jimyoda", null);
			yield return new CreditRecord_Role("Credit_Tester", "Pheanox", null);
			yield return new CreditRecord_Role("Credit_Tester", "Semmy", null);
			yield return new CreditRecord_Role("Credit_Tester", "Letharion", null);
			yield return new CreditRecord_Role("Credit_Tester", "Laos", null);
			yield return new CreditRecord_Role("Credit_Tester", "Coenmjc", null);
			yield return new CreditRecord_Role("Credit_Tester", "Gaesatae", null);
			yield return new CreditRecord_Role("Credit_Tester", "Skullywag", null);
			yield return new CreditRecord_Role("Credit_Tester", "Enystrom8734", null);
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

		[CompilerGenerated]
		private sealed class <AllCredits>c__Iterator0 : IEnumerable, IEnumerable<CreditsEntry>, IEnumerator, IDisposable, IEnumerator<CreditsEntry>
		{
			internal IEnumerator<LoadedLanguage> $locvar0;

			internal LoadedLanguage <lang>__1;

			internal List<CreditsEntry>.Enumerator $locvar1;

			internal CreditsEntry <langCred>__2;

			internal CreditsEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllCredits>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.$current = new CreditRecord_Space(200f);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = new CreditRecord_Title("Credits_Developers".Translate());
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = new CreditRecord_Role("", "Tynan Sylvester", null);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = new CreditRecord_Role("", "Piotr Walczak", null);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = new CreditRecord_Role("", "Ben Rog-Wilhelm", null);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = new CreditRecord_Space(50f);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = new CreditRecord_Title("Credit_MusicAndSound".Translate());
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = new CreditRecord_Role("", "Alistair Lindsay", null);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = new CreditRecord_Space(50f);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = new CreditRecord_Title("Credit_GameArt".Translate());
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = new CreditRecord_Role("", "Rhopunzel", null);
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = new CreditRecord_Role("", "Ricardo Tome", null);
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					this.$current = new CreditRecord_Role("", "Kay Fedewa", null);
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				case 13u:
					this.$current = new CreditRecord_Role("", "Jon Larson", null);
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				case 14u:
					this.$current = new CreditRecord_Space(50f);
					if (!this.$disposing)
					{
						this.$PC = 15;
					}
					return true;
				case 15u:
					this.$current = new CreditRecord_Title("Credits_AdditionalDevelopment".Translate());
					if (!this.$disposing)
					{
						this.$PC = 16;
					}
					return true;
				case 16u:
					this.$current = new CreditRecord_Role("", "Gavan Woolery", null);
					if (!this.$disposing)
					{
						this.$PC = 17;
					}
					return true;
				case 17u:
					this.$current = new CreditRecord_Role("", "David 'Rez' Graham", null);
					if (!this.$disposing)
					{
						this.$PC = 18;
					}
					return true;
				case 18u:
					this.$current = new CreditRecord_Role("", "Ben Grob", null);
					if (!this.$disposing)
					{
						this.$PC = 19;
					}
					return true;
				case 19u:
					this.$current = new CreditRecord_Space(50f);
					if (!this.$disposing)
					{
						this.$PC = 20;
					}
					return true;
				case 20u:
					this.$current = new CreditRecord_Title("Credits_TitleCommunity".Translate());
					if (!this.$disposing)
					{
						this.$PC = 21;
					}
					return true;
				case 21u:
					this.$current = new CreditRecord_Role("Credit_ModDonation", "Zhentar", null);
					if (!this.$disposing)
					{
						this.$PC = 22;
					}
					return true;
				case 22u:
					this.$current = new CreditRecord_Role("Credit_ModDonation", "Haplo", null);
					if (!this.$disposing)
					{
						this.$PC = 23;
					}
					return true;
				case 23u:
					this.$current = new CreditRecord_Role("Credit_ModDonation", "iame6162013", null);
					if (!this.$disposing)
					{
						this.$PC = 24;
					}
					return true;
				case 24u:
					this.$current = new CreditRecord_Role("Credit_ModDonation", "Shinzy", null);
					if (!this.$disposing)
					{
						this.$PC = 25;
					}
					return true;
				case 25u:
					this.$current = new CreditRecord_Role("Credit_WritingDonation", "John Woolley", null);
					if (!this.$disposing)
					{
						this.$PC = 26;
					}
					return true;
				case 26u:
					this.$current = new CreditRecord_Role("Credit_Moderator", "ItchyFlea", null);
					if (!this.$disposing)
					{
						this.$PC = 27;
					}
					return true;
				case 27u:
					this.$current = new CreditRecord_Role("Credit_Moderator", "Ramsis", null);
					if (!this.$disposing)
					{
						this.$PC = 28;
					}
					return true;
				case 28u:
					this.$current = new CreditRecord_Role("Credit_Moderator", "Calahan", null);
					if (!this.$disposing)
					{
						this.$PC = 29;
					}
					return true;
				case 29u:
					this.$current = new CreditRecord_Role("Credit_Moderator", "milon", null);
					if (!this.$disposing)
					{
						this.$PC = 30;
					}
					return true;
				case 30u:
					this.$current = new CreditRecord_Role("Credit_Moderator", "Evul", null);
					if (!this.$disposing)
					{
						this.$PC = 31;
					}
					return true;
				case 31u:
					this.$current = new CreditRecord_Role("Credit_Moderator", "MarvinKosh", null);
					if (!this.$disposing)
					{
						this.$PC = 32;
					}
					return true;
				case 32u:
					this.$current = new CreditRecord_Role("Credit_WikiMaster", "ZestyLemons", null);
					if (!this.$disposing)
					{
						this.$PC = 33;
					}
					return true;
				case 33u:
					this.$current = new CreditRecord_Role("Credit_Tester", "ItchyFlea", null);
					if (!this.$disposing)
					{
						this.$PC = 34;
					}
					return true;
				case 34u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Haplo", null);
					if (!this.$disposing)
					{
						this.$PC = 35;
					}
					return true;
				case 35u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Mehni", null);
					if (!this.$disposing)
					{
						this.$PC = 36;
					}
					return true;
				case 36u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Vas", null);
					if (!this.$disposing)
					{
						this.$PC = 37;
					}
					return true;
				case 37u:
					this.$current = new CreditRecord_Role("Credit_Tester", "XeoNovaDan", null);
					if (!this.$disposing)
					{
						this.$PC = 38;
					}
					return true;
				case 38u:
					this.$current = new CreditRecord_Role("Credit_Tester", "JimmyAgnt007", null);
					if (!this.$disposing)
					{
						this.$PC = 39;
					}
					return true;
				case 39u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Goldenpotatoes", null);
					if (!this.$disposing)
					{
						this.$PC = 40;
					}
					return true;
				case 40u:
					this.$current = new CreditRecord_Role("Credit_Tester", "_alphaBeta_", null);
					if (!this.$disposing)
					{
						this.$PC = 41;
					}
					return true;
				case 41u:
					this.$current = new CreditRecord_Role("Credit_Tester", "TheDee05", null);
					if (!this.$disposing)
					{
						this.$PC = 42;
					}
					return true;
				case 42u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Drb89", null);
					if (!this.$disposing)
					{
						this.$PC = 43;
					}
					return true;
				case 43u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Skissor", null);
					if (!this.$disposing)
					{
						this.$PC = 44;
					}
					return true;
				case 44u:
					this.$current = new CreditRecord_Role("Credit_Tester", "MarvinKosh", null);
					if (!this.$disposing)
					{
						this.$PC = 45;
					}
					return true;
				case 45u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Evul", null);
					if (!this.$disposing)
					{
						this.$PC = 46;
					}
					return true;
				case 46u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Jimyoda", null);
					if (!this.$disposing)
					{
						this.$PC = 47;
					}
					return true;
				case 47u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Pheanox", null);
					if (!this.$disposing)
					{
						this.$PC = 48;
					}
					return true;
				case 48u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Semmy", null);
					if (!this.$disposing)
					{
						this.$PC = 49;
					}
					return true;
				case 49u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Letharion", null);
					if (!this.$disposing)
					{
						this.$PC = 50;
					}
					return true;
				case 50u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Laos", null);
					if (!this.$disposing)
					{
						this.$PC = 51;
					}
					return true;
				case 51u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Coenmjc", null);
					if (!this.$disposing)
					{
						this.$PC = 52;
					}
					return true;
				case 52u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Gaesatae", null);
					if (!this.$disposing)
					{
						this.$PC = 53;
					}
					return true;
				case 53u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Skullywag", null);
					if (!this.$disposing)
					{
						this.$PC = 54;
					}
					return true;
				case 54u:
					this.$current = new CreditRecord_Role("Credit_Tester", "Enystrom8734", null);
					if (!this.$disposing)
					{
						this.$PC = 55;
					}
					return true;
				case 55u:
					this.$current = new CreditRecord_Role("", "Many other gracious volunteers!", null);
					if (!this.$disposing)
					{
						this.$PC = 56;
					}
					return true;
				case 56u:
					this.$current = new CreditRecord_Space(200f);
					if (!this.$disposing)
					{
						this.$PC = 57;
					}
					return true;
				case 57u:
					enumerator = LanguageDatabase.AllLoadedLanguages.GetEnumerator();
					num = 4294967293u;
					break;
				case 58u:
				case 59u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 58u:
						IL_B05:
						enumerator2 = lang.info.credits.GetEnumerator();
						num = 4294967293u;
						break;
					case 59u:
						break;
					default:
						goto IL_B98;
					}
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							langCred = enumerator2.Current;
							this.$current = langCred;
							if (!this.$disposing)
							{
								this.$PC = 59;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					IL_B98:
					if (enumerator.MoveNext())
					{
						lang = enumerator.Current;
						if (lang.info.credits.Count > 0)
						{
							this.$current = new CreditRecord_Title("Credits_TitleLanguage".Translate(new object[]
							{
								lang.FriendlyNameEnglish
							}));
							if (!this.$disposing)
							{
								this.$PC = 58;
							}
							flag = true;
							return true;
						}
						goto IL_B05;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			CreditsEntry IEnumerator<CreditsEntry>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 58u:
				case 59u:
					try
					{
						switch (num)
						{
						case 59u:
							try
							{
							}
							finally
							{
								((IDisposable)enumerator2).Dispose();
							}
							break;
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.CreditsEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<CreditsEntry> IEnumerable<CreditsEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new CreditsAssembler.<AllCredits>c__Iterator0();
			}
		}
	}
}
