using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	public class Page_ScenarioEditor : Page
	{
		private Scenario curScen = null;

		private Vector2 infoScrollPosition = Vector2.zero;

		private string seed;

		private bool seedIsValid = true;

		private bool editMode = false;

		[CompilerGenerated]
		private static Func<ScenPartDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ScenPartDef, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ScenPartDef, string> <>f__am$cache2;

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
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoConfigControls(rect2);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded();
			if (!this.editMode)
			{
				ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			}
			else
			{
				ScenarioUI.DrawScenarioEditInterface(rect3, this.curScen, ref this.infoScrollPosition);
			}
			GUI.EndGroup();
			base.DoBottomButtons(rect, null, null, null, true);
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
			if (listing_Standard.ButtonText("Load".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Load(delegate(Scenario loadedScen)
				{
					this.curScen = loadedScen;
					this.seedIsValid = false;
				}));
			}
			if (listing_Standard.ButtonText("Save".Translate(), null))
			{
				if (Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
				{
					Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen));
				}
			}
			if (listing_Standard.ButtonText("RandomizeSeed".Translate(), null))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.RandomizeSeedAndScenario();
				this.seedIsValid = true;
			}
			if (this.seedIsValid)
			{
				listing_Standard.Label("Seed".Translate().CapitalizeFirst(), -1f, null);
				string a = listing_Standard.TextEntry(this.seed, 1);
				if (a != this.seed)
				{
					this.seed = a;
					this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
				}
			}
			else
			{
				listing_Standard.Gap(Text.LineHeight + Text.LineHeight + 2f);
			}
			listing_Standard.CheckboxLabeled("EditMode".Translate().CapitalizeFirst(), ref this.editMode, null);
			if (this.editMode)
			{
				this.seedIsValid = false;
				if (listing_Standard.ButtonText("AddPart".Translate(), null))
				{
					this.OpenAddScenPartMenu();
				}
				if (SteamManager.Initialized && (this.curScen.Category == ScenarioCategory.CustomLocal || this.curScen.Category == ScenarioCategory.SteamWorkshop))
				{
					if (listing_Standard.ButtonText(Workshop.UploadButtonLabel(this.curScen.GetPublishedFileId()), null))
					{
						if (Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
						{
							AcceptanceReport acceptanceReport = this.curScen.TryUploadReport();
							if (!acceptanceReport.Accepted)
							{
								Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate
								{
									SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
									Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate
									{
										SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
										Workshop.Upload(this.curScen);
									}, true, null));
								}, true, null));
							}
						}
					}
				}
			}
			listing_Standard.End();
		}

		private static bool CheckAllPartsCompatible(Scenario scen)
		{
			foreach (ScenPart scenPart in scen.AllParts)
			{
				int num = 0;
				foreach (ScenPart scenPart2 in scen.AllParts)
				{
					if (scenPart2.def == scenPart.def)
					{
						num++;
					}
					if (num > scenPart.def.maxUses)
					{
						Messages.Message("TooMany".Translate(new object[]
						{
							scenPart.def.maxUses
						}) + ": " + scenPart.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
					if (scenPart != scenPart2 && !scenPart.CanCoexistWith(scenPart2))
					{
						Messages.Message(string.Concat(new string[]
						{
							"Incompatible".Translate(),
							": ",
							scenPart.def.label,
							", ",
							scenPart2.def.label
						}), MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		private void OpenAddScenPartMenu()
		{
			FloatMenuUtility.MakeMenu<ScenPartDef>(from p in ScenarioMaker.AddableParts(this.curScen)
			where p.category != ScenPartCategory.Fixed
			orderby p.label
			select p, (ScenPartDef p) => p.LabelCap, (ScenPartDef p) => delegate()
			{
				this.AddScenPart(p);
			});
		}

		private void AddScenPart(ScenPartDef def)
		{
			ScenPart scenPart = ScenarioMaker.MakeScenPart(def);
			scenPart.Randomize();
			this.curScen.parts.Add(scenPart);
		}

		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else if (this.curScen == null)
			{
				result = false;
			}
			else if (!Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				result = false;
			}
			else
			{
				Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
				result = true;
			}
			return result;
		}

		[CompilerGenerated]
		private void <DoConfigControls>m__0(Scenario loadedScen)
		{
			this.curScen = loadedScen;
			this.seedIsValid = false;
		}

		[CompilerGenerated]
		private void <DoConfigControls>m__1()
		{
			SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				Workshop.Upload(this.curScen);
			}, true, null));
		}

		[CompilerGenerated]
		private static bool <OpenAddScenPartMenu>m__2(ScenPartDef p)
		{
			return p.category != ScenPartCategory.Fixed;
		}

		[CompilerGenerated]
		private static string <OpenAddScenPartMenu>m__3(ScenPartDef p)
		{
			return p.label;
		}

		[CompilerGenerated]
		private static string <OpenAddScenPartMenu>m__4(ScenPartDef p)
		{
			return p.LabelCap;
		}

		[CompilerGenerated]
		private Action <OpenAddScenPartMenu>m__5(ScenPartDef p)
		{
			return delegate()
			{
				this.AddScenPart(p);
			};
		}

		[CompilerGenerated]
		private void <DoConfigControls>m__6()
		{
			SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			Workshop.Upload(this.curScen);
		}

		[CompilerGenerated]
		private sealed class <OpenAddScenPartMenu>c__AnonStorey0
		{
			internal ScenPartDef p;

			internal Page_ScenarioEditor $this;

			public <OpenAddScenPartMenu>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.AddScenPart(this.p);
			}
		}
	}
}
