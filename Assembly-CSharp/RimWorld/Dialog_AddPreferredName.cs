using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Dialog_AddPreferredName : Window
	{
		private string searchName = "";

		private string[] searchWords;

		private List<NameTriple> cachedNames;

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		public Dialog_AddPreferredName()
		{
			base.doCloseButton = true;
			base.absorbInputAroundWindow = true;
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList();
		}

		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label("TypeFirstNickOrLastName".Translate(), -1f);
			string text = listing_Standard.TextEntry(this.searchName, 1);
			if (text.Length < 20)
			{
				this.searchName = text;
				this.searchWords = this.searchName.Replace("'", "").Split(' ');
			}
			listing_Standard.Gap(4f);
			if (this.searchName.Length > 1)
			{
				foreach (NameTriple item in this.cachedNames.Where(new Func<NameTriple, bool>(this.FilterMatch)))
				{
					if (listing_Standard.ButtonText(item.ToString(), (string)null))
					{
						this.TryChooseName(item);
					}
					double num = listing_Standard.CurHeight + 30.0;
					float height = inRect.height;
					Vector2 closeButSize = base.CloseButSize;
					if (num > height - (closeButSize.y + 8.0))
						break;
				}
			}
			listing_Standard.End();
		}

		private bool FilterMatch(NameTriple n)
		{
			return (!(n.First == "Tynan") || !(n.Last == "Sylvester")) && this.searchWords.Length != 0 && ((this.searchWords.Length != 1) ? (this.searchWords.Length == 2 && n.First.EqualsIgnoreCase(this.searchWords[0]) && (n.Last.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchWords[1], StringComparison.OrdinalIgnoreCase))) : (n.Last.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.First.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase) || n.Nick.StartsWith(this.searchName, StringComparison.OrdinalIgnoreCase)));
		}

		private void TryChooseName(NameTriple name)
		{
			if (this.AlreadyPreferred(name))
			{
				Messages.Message("MessageAlreadyPreferredName".Translate(), MessageTypeDefOf.RejectInput);
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
	}
}
