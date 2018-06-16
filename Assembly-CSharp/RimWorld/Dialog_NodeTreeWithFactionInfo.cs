using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x06002DF4 RID: 11764 RVA: 0x00182D34 File Offset: 0x00181134
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x00182D5C File Offset: 0x0018115C
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				this.DrawFactionInfo(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - 7f), this.faction);
			}
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x00182DB0 File Offset: 0x001811B0
		private void DrawFactionInfo(Rect rect, Faction faction)
		{
			Text.Anchor = TextAnchor.LowerRight;
			FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
			GUI.color = playerRelationKind.GetColor();
			Widgets.Label(rect, playerRelationKind.GetLabel());
			rect.height -= Text.CalcHeight(playerRelationKind.GetLabel(), rect.width) + Text.SpaceBetweenLines;
			GUI.color = Color.gray;
			Widgets.Label(rect, string.Concat(new string[]
			{
				faction.Name,
				"\n",
				"goodwill".Translate().CapitalizeFirst(),
				": ",
				faction.PlayerGoodwill.ToStringWithSign()
			}));
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x04001862 RID: 6242
		private Faction faction;
	}
}
