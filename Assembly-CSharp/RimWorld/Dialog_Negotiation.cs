using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080A RID: 2058
	public class Dialog_Negotiation : Dialog_NodeTree
	{
		// Token: 0x0400185C RID: 6236
		protected Pawn negotiator;

		// Token: 0x0400185D RID: 6237
		protected ICommunicable commTarget;

		// Token: 0x0400185E RID: 6238
		private const float TitleHeight = 70f;

		// Token: 0x0400185F RID: 6239
		private const float InfoHeight = 60f;

		// Token: 0x06002DF0 RID: 11760 RVA: 0x00182EA4 File Offset: 0x001812A4
		public Dialog_Negotiation(Pawn negotiator, ICommunicable commTarget, DiaNode startNode, bool radioMode) : base(startNode, radioMode, false, null)
		{
			this.negotiator = negotiator;
			this.commTarget = commTarget;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x00182EC0 File Offset: 0x001812C0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(720f, 600f);
			}
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x00182EE4 File Offset: 0x001812E4
		public override void DoWindowContents(Rect inRect)
		{
			GUI.BeginGroup(inRect);
			Rect rect = new Rect(0f, 0f, inRect.width / 2f, 70f);
			Rect rect2 = new Rect(0f, rect.yMax, rect.width, 60f);
			Rect rect3 = new Rect(inRect.width / 2f, 0f, inRect.width / 2f, 70f);
			Rect rect4 = new Rect(inRect.width / 2f, rect.yMax, rect.width, 60f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.negotiator.LabelCap);
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect3, this.commTarget.GetCallLabel());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.7f);
			Widgets.Label(rect2, "SocialSkillIs".Translate(new object[]
			{
				this.negotiator.skills.GetSkill(SkillDefOf.Social).Level
			}));
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect4, this.commTarget.GetInfoText());
			Faction faction = this.commTarget.GetFaction();
			if (faction != null)
			{
				FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
				GUI.color = playerRelationKind.GetColor();
				Rect rect5 = new Rect(rect4.x, rect4.y + Text.CalcHeight(this.commTarget.GetInfoText(), rect4.width) + Text.SpaceBetweenLines, rect4.width, 30f);
				Widgets.Label(rect5, playerRelationKind.GetLabel());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			GUI.EndGroup();
			float num = 147f;
			Rect rect6 = new Rect(0f, num, inRect.width, inRect.height - num);
			base.DrawNode(rect6);
		}
	}
}
