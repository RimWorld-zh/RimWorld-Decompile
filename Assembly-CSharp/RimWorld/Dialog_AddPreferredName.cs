using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F4 RID: 2036
	public class Dialog_AddPreferredName : Window
	{
		// Token: 0x040017BC RID: 6076
		private string searchName = "";

		// Token: 0x040017BD RID: 6077
		private string[] searchWords;

		// Token: 0x040017BE RID: 6078
		private List<NameTriple> cachedNames;

		// Token: 0x06002D26 RID: 11558 RVA: 0x0017B840 File Offset: 0x00179C40
		public Dialog_AddPreferredName()
		{
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.cachedNames = (from n in (from b in SolidBioDatabase.allBios
			select b.name).Concat(PawnNameDatabaseSolid.AllNames())
			orderby n.Last descending
			select n).ToList<NameTriple>();
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x0017B8CC File Offset: 0x00179CCC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 650f);
			}
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x0017B8F0 File Offset: 0x00179CF0
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

		// Token: 0x06002D29 RID: 11561 RVA: 0x0017BA3C File Offset: 0x00179E3C
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

		// Token: 0x06002D2A RID: 11562 RVA: 0x0017BB50 File Offset: 0x00179F50
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

		// Token: 0x06002D2B RID: 11563 RVA: 0x0017BBA0 File Offset: 0x00179FA0
		private bool AlreadyPreferred(NameTriple name)
		{
			return Prefs.PreferredNames.Contains(name.ToString());
		}
	}
}
