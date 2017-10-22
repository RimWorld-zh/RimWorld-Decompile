using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	public class Page_ScenarioEditor : Page
	{
		private Scenario curScen;

		private Vector2 infoScrollPosition = Vector2.zero;

		private string seed;

		private bool seedIsValid = true;

		private bool editMode;

		public override string PageTitle
		{
			get
			{
				return "ScenarioEditor".Translate();
			}
		}

		public Scenario EditingScenario
		{
			get
			{
				return this.curScen;
			}
		}

		public Page_ScenarioEditor(Scenario scen)
		{
			if (scen != null)
			{
				this.curScen = scen;
				this.seedIsValid = false;
			}
			else
			{
				this.RandomizeSeedAndScenario();
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			GUI.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, (float)(mainRect.width * 0.34999999403953552), mainRect.height).Rounded();
			this.DoConfigControls(rect2);
			Rect rect3 = new Rect((float)(rect2.xMax + 17.0), 0f, (float)(mainRect.width - rect2.width - 17.0), mainRect.height).Rounded();
			if (!this.editMode)
			{
				ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			}
			else
			{
				ScenarioUI.DrawScenarioEditInterface(rect3, this.curScen, ref this.infoScrollPosition);
			}
			GUI.EndGroup();
			base.DoBottomButtons(rect, (string)null, (string)null, null, true);
		}

		private void RandomizeSeedAndScenario()
		{
			this.seed = GenText.RandomSeedString();
			this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
		}

		private void DoConfigControls(Rect rect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(rect);
			if (listing_Standard.ButtonText("Load".Translate(), (string)null))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Load((Action<Scenario>)delegate(Scenario loadedScen)
				{
					this.curScen = loadedScen;
					this.seedIsValid = false;
				}));
			}
			if (listing_Standard.ButtonText("Save".Translate(), (string)null) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen));
			}
			if (listing_Standard.ButtonText("RandomizeSeed".Translate(), (string)null))
			{
				SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
				this.RandomizeSeedAndScenario();
				this.seedIsValid = true;
			}
			if (this.seedIsValid)
			{
				listing_Standard.Label("Seed".Translate().CapitalizeFirst(), -1f);
				string a = listing_Standard.TextEntry(this.seed, 1);
				if (a != this.seed)
				{
					this.seed = a;
					this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
				}
			}
			else
			{
				listing_Standard.Gap((float)(Text.LineHeight + Text.LineHeight + 2.0));
			}
			listing_Standard.CheckboxLabeled("EditMode".Translate().CapitalizeFirst(), ref this.editMode, (string)null);
			if (this.editMode)
			{
				this.seedIsValid = false;
				if (listing_Standard.ButtonText("AddPart".Translate(), (string)null))
				{
					this.OpenAddScenPartMenu();
				}
				if (SteamManager.Initialized && (this.curScen.Category == ScenarioCategory.CustomLocal || this.curScen.Category == ScenarioCategory.SteamWorkshop) && listing_Standard.ButtonText(Workshop.UploadButtonLabel(this.curScen.GetPublishedFileId()), (string)null) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
				{
					AcceptanceReport acceptanceReport = this.curScen.TryUploadReport();
					if (!acceptanceReport.Accepted)
					{
						Messages.Message(acceptanceReport.Reason, MessageSound.RejectInput);
					}
					else
					{
						SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), (Action)delegate
						{
							SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), (Action)delegate
							{
								SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
								Workshop.Upload(this.curScen);
							}, true, (string)null));
						}, true, (string)null));
					}
				}
			}
			listing_Standard.End();
		}

		private static bool CheckAllPartsCompatible(Scenario scen)
		{
			foreach (ScenPart allPart in scen.AllParts)
			{
				int num = 0;
				foreach (ScenPart allPart2 in scen.AllParts)
				{
					if (allPart2.def == allPart.def)
					{
						num++;
					}
					if (num > allPart.def.maxUses)
					{
						Messages.Message("TooMany".Translate(allPart.def.maxUses) + ": " + allPart.def.label, MessageSound.RejectInput);
						return false;
					}
					if (allPart != allPart2 && !allPart.CanCoexistWith(allPart2))
					{
						Messages.Message("Incompatible".Translate() + ": " + allPart.def.label + ", " + allPart2.def.label, MessageSound.RejectInput);
						return false;
					}
				}
			}
			return true;
		}

		private void OpenAddScenPartMenu()
		{
			FloatMenuUtility.MakeMenu(from p in ScenarioMaker.AddableParts(this.curScen)
			where p.category != ScenPartCategory.Fixed
			orderby p.label
			select p, (Func<ScenPartDef, string>)((ScenPartDef p) => p.LabelCap), (Func<ScenPartDef, Action>)((ScenPartDef p) => (Action)delegate()
			{
				this.AddScenPart(p);
			}));
		}

		private void AddScenPart(ScenPartDef def)
		{
			ScenPart scenPart = ScenarioMaker.MakeScenPart(def);
			scenPart.Randomize();
			this.curScen.parts.Add(scenPart);
		}

		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			if (!Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				return false;
			}
			Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
			return true;
		}
	}
}
