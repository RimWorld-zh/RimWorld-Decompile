using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class Dialog_InfoCard : Window
	{
		private enum InfoCardTab : byte
		{
			Stats = 0,
			Character = 1,
			Health = 2,
			Records = 3
		}

		private Thing thing;

		private ThingDef stuff;

		private Def def;

		private WorldObject worldObject;

		private InfoCardTab tab;

		private Def Def
		{
			get
			{
				return (this.thing == null) ? ((this.worldObject == null) ? this.def : this.worldObject.def) : this.thing.def;
			}
		}

		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		public Dialog_InfoCard(Thing thing)
		{
			this.thing = thing;
			this.tab = InfoCardTab.Stats;
			this.Setup();
		}

		public Dialog_InfoCard(Def onlyDef)
		{
			this.def = onlyDef;
			this.Setup();
		}

		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.Setup();
		}

		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		private void Setup()
		{
			base.forcePause = true;
			base.closeOnEscapeKey = true;
			base.doCloseButton = true;
			base.doCloseX = true;
			base.absorbInputAroundWindow = true;
			base.soundAppear = SoundDef.Named("InfoCard_Open");
			base.soundClose = SoundDef.Named("InfoCard_Close");
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		public override void WindowUpdate()
		{
			base.WindowUpdate();
		}

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect)
			{
				yMin = rect.yMax
			};
			rect2.yMax -= 38f;
			Rect rect3 = rect2;
			rect3.yMin += 45f;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), (Action)delegate
			{
				this.tab = InfoCardTab.Stats;
			}, this.tab == InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), (Action)delegate
					{
						this.tab = InfoCardTab.Character;
					}, this.tab == InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), (Action)delegate
				{
					this.tab = InfoCardTab.Health;
				}, this.tab == InfoCardTab.Health);
				list.Add(item3);
				TabRecord item4 = new TabRecord("TabRecords".Translate(), (Action)delegate
				{
					this.tab = InfoCardTab.Records;
				}, this.tab == InfoCardTab.Records);
				list.Add(item4);
			}
			TabDrawer.DrawTabs(rect3, list);
			this.FillCard(rect3.ContractedBy(18f));
		}

		protected void FillCard(Rect cardRect)
		{
			if (this.tab == InfoCardTab.Stats)
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
			else if (this.tab == InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect));
			}
			else if (this.tab == InfoCardTab.Health)
			{
				cardRect.yMin += 8f;
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, false, null);
			}
			else if (this.tab == InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
		}

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
				result = ((thingDef == null) ? this.Def.LabelCap : GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst());
			}
			return result;
		}
	}
}
