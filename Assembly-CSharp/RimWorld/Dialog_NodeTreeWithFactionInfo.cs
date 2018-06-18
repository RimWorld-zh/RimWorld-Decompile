using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x06002DF6 RID: 11766 RVA: 0x00182DC8 File Offset: 0x001811C8
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x00182DF0 File Offset: 0x001811F0
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				this.DrawFactionInfo(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - 7f), this.faction);
			}
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x00182E44 File Offset: 0x00181244
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
