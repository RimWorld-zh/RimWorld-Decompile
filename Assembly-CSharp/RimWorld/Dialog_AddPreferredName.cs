using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Dialog_AddPreferredName : Window
	{
		private string searchName = "";

		private string[] searchWords;

		private List<NameTriple> cachedNames;

		[CompilerGenerated]
		private static Func<PawnBio, NameTriple> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<NameTriple, string> <>f__am$cache1;

		public Dialog_AddPreferredName()
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label("TypeFirstNickOrLastName".Translate(), -1f, null);
			string text = listing_Standard.TextEntry(this.searchName, 1);
			if (text.Length < 20)
			{
				this.searchName = text;
				this.searchWords = this.searchName.Replace("'", "").Split(new char[]
				{
					' '
				});
			}
			listing_Standard.Gap(4f);
			if (this.searchName.Length > 1)
			{
				foreach (NameTriple nameTriple in this.cachedNames.Where(new Func<NameTriple, bool>(this.FilterMatch)))
				{
					if (listing_Standard.ButtonText(nameTriple.ToString(), null))
					{
						this.TryChooseName(nameTriple);
					}
					if (listing_Standard.CurHeight + 30f > inRect.height - (this.CloseButSize.y + 8f))
					{
						break;
					}
				}
			}
			listing_Standard.End();
		}

		private bool FilterMatch(NameTriple n)
		{
			bool result;
			if (n.First == "Tynan" && n.Last == "Sylvester")
			{
				result = false;
			}
			else if (this.searchWords.Length == 0)
			{
				result = false;
			}
			else if (this.searchWords.Length == 1)
			{
				result = (n.Last.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.First.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase));
			}
			else
			{
				result = (this.searchWords.Length == 2 && n.First.EqualsIgnoreCase(this.searchWords[0]) && (n.Last.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase)));
			}
			return result;
		}

		private void TryChooseName(NameTriple name)
		{
			if (this.AlreadyPreferred(name))
			{
				Messages.Message("MessageAlreadyPreferredName".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				Prefs.PreferredNames.Add(name.ToString());
				this.Close(true);
			}
		}

		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}

		[CompilerGenerated]
		private static NameTriple <Dialog_AddPreferredName>m__0(PawnBio b)
		{
			return b.name;
		}

		[CompilerGenerated]
		private static string <Dialog_AddPreferredName>m__1(NameTriple n)
		{
			return n.Last;
		}
	}
}
