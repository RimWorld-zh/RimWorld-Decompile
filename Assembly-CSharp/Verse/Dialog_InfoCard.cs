using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBB RID: 3771
	public class Dialog_InfoCard : Window
	{
		// Token: 0x0600591A RID: 22810 RVA: 0x002DA9A7 File Offset: 0x002D8DA7
		public Dialog_InfoCard(Thing thing)
		{
			this.thing = thing;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x002DA9C4 File Offset: 0x002D8DC4
		public Dialog_InfoCard(Def onlyDef)
		{
			this.def = onlyDef;
			this.Setup();
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x002DA9DA File Offset: 0x002D8DDA
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.Setup();
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x002DA9F7 File Offset: 0x002D8DF7
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x0600591E RID: 22814 RVA: 0x002DAA10 File Offset: 0x002D8E10
		private Def Def
		{
			get
			{
				Def result;
				if (this.thing != null)
				{
					result = this.thing.def;
				}
				else if (this.worldObject != null)
				{
					result = this.worldObject.def;
				}
				else
				{
					result = this.def;
				}
				return result;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x002DAA64 File Offset: 0x002D8E64
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06005920 RID: 22816 RVA: 0x002DAA84 File Offset: 0x002D8E84
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06005921 RID: 22817 RVA: 0x002DAAA8 File Offset: 0x002D8EA8
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x002DAAC4 File Offset: 0x002D8EC4
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.soundAppear = SoundDefOf.InfoCard_Open;
			this.soundClose = SoundDefOf.InfoCard_Close;
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x002DAB14 File Offset: 0x002D8F14
		public override void WindowUpdate()
		{
			base.WindowUpdate();
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x002DAB20 File Offset: 0x002D8F20
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect);
			rect2.yMin = rect.yMax;
			rect2.yMax -= 38f;
			Rect rect3 = rect2;
			rect3.yMin += 45f;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), delegate()
			{
				this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			}, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Character;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Health;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Health);
				list.Add(item3);
				TabRecord item4 = new TabRecord("TabRecords".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Records;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Records);
				list.Add(item4);
			}
			TabDrawer.DrawTabs(rect3, list, 200f);
			this.FillCard(rect3.ContractedBy(18f));
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x002DACA0 File Offset: 0x002D90A0
		protected void FillCard(Rect cardRect)
		{
			if (this.tab == Dialog_InfoCard.InfoCardTab.Stats)
			{
				if (this.thing != null)
				{
					Thing innerThing = this.thing;
					MinifiedThing minifiedThing = this.thing as MinifiedThing;
					if (minifiedThing != null)
					{
						innerThing = minifiedThing.InnerThing;
					}
					StatsReportUtility.DrawStatsReport(cardRect, innerThing);
				}
				else if (this.worldObject != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.worldObject);
				}
				else
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.def, this.stuff);
				}
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect));
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Health)
			{
				cardRect.yMin += 8f;
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, false, null);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x002DADB0 File Offset: 0x002D91B0
		private string GetTitle()
		{
			string result;
			if (this.thing != null)
			{
				result = this.thing.LabelCapNoCount;
			}
			else if (this.worldObject != null)
			{
				result = this.worldObject.LabelCap;
			}
			else
			{
				ThingDef thingDef = this.Def as ThingDef;
				if (thingDef != null)
				{
					result = GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
				}
				else
				{
					result = this.Def.LabelCap;
				}
			}
			return result;
		}

		// Token: 0x04003B69 RID: 15209
		private Thing thing;

		// Token: 0x04003B6A RID: 15210
		private ThingDef stuff;

		// Token: 0x04003B6B RID: 15211
		private Def def;

		// Token: 0x04003B6C RID: 15212
		private WorldObject worldObject;

		// Token: 0x04003B6D RID: 15213
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x02000EBC RID: 3772
		private enum InfoCardTab : byte
		{
			// Token: 0x04003B6F RID: 15215
			Stats,
			// Token: 0x04003B70 RID: 15216
			Character,
			// Token: 0x04003B71 RID: 15217
			Health,
			// Token: 0x04003B72 RID: 15218
			Records
		}
	}
}
