using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBA RID: 3770
	public class Dialog_InfoCard : Window
	{
		// Token: 0x0600593B RID: 22843 RVA: 0x002DC5F3 File Offset: 0x002DA9F3
		public Dialog_InfoCard(Thing thing)
		{
			this.thing = thing;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x002DC610 File Offset: 0x002DAA10
		public Dialog_InfoCard(Def onlyDef)
		{
			this.def = onlyDef;
			this.Setup();
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x002DC626 File Offset: 0x002DAA26
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.Setup();
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x002DC643 File Offset: 0x002DAA43
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x0600593F RID: 22847 RVA: 0x002DC65C File Offset: 0x002DAA5C
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

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06005940 RID: 22848 RVA: 0x002DC6B0 File Offset: 0x002DAAB0
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005941 RID: 22849 RVA: 0x002DC6D0 File Offset: 0x002DAAD0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06005942 RID: 22850 RVA: 0x002DC6F4 File Offset: 0x002DAAF4
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005943 RID: 22851 RVA: 0x002DC710 File Offset: 0x002DAB10
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

		// Token: 0x06005944 RID: 22852 RVA: 0x002DC760 File Offset: 0x002DAB60
		public override void WindowUpdate()
		{
			base.WindowUpdate();
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x002DC76C File Offset: 0x002DAB6C
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

		// Token: 0x06005946 RID: 22854 RVA: 0x002DC8EC File Offset: 0x002DACEC
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

		// Token: 0x06005947 RID: 22855 RVA: 0x002DC9FC File Offset: 0x002DADFC
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

		// Token: 0x04003B79 RID: 15225
		private Thing thing;

		// Token: 0x04003B7A RID: 15226
		private ThingDef stuff;

		// Token: 0x04003B7B RID: 15227
		private Def def;

		// Token: 0x04003B7C RID: 15228
		private WorldObject worldObject;

		// Token: 0x04003B7D RID: 15229
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x02000EBB RID: 3771
		private enum InfoCardTab : byte
		{
			// Token: 0x04003B7F RID: 15231
			Stats,
			// Token: 0x04003B80 RID: 15232
			Character,
			// Token: 0x04003B81 RID: 15233
			Health,
			// Token: 0x04003B82 RID: 15234
			Records
		}
	}
}
