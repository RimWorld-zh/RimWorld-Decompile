using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080B RID: 2059
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x04001864 RID: 6244
		private Faction faction;

		// Token: 0x06002DF2 RID: 11762 RVA: 0x00183354 File Offset: 0x00181754
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x0018337C File Offset: 0x0018177C
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				this.DrawFactionInfo(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - 7f), this.faction);
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x001833D0 File Offset: 0x001817D0
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
	}
}
