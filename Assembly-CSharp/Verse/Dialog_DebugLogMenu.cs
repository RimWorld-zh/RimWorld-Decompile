using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Verse
{
	public class Dialog_DebugLogMenu : Dialog_DebugOptionLister
	{
		private struct ListItem
		{
			public string title;

			public string category;

			public Action action;
		}

		private List<ListItem> dataAnalyzerLogs = new List<ListItem>();

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		public Dialog_DebugLogMenu()
		{
			base.forcePause = true;
			MethodInfo[] methods = typeof(DataAnalysisLogger).GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo mi in methods)
			{
				if (Current.ProgramState == ProgramState.Playing || !mi.HasAttribute<ModeRestrictionPlay>())
				{
					string name = mi.Name;
					if (name.StartsWith("DoLog_"))
					{
						string title = GenText.SplitCamelCase(name.Substring(6));
						Action action = delegate
						{
							mi.Invoke(null, null);
						};
						string text = "Logs";
						Category category = null;
						if (((MemberInfo)mi).TryGetAttribute<Category>(out category))
						{
							text = text + " - " + category.name;
						}
						this.dataAnalyzerLogs.Add(new ListItem
						{
							title = title,
							category = text,
							action = action
						});
					}
				}
			}
			this.dataAnalyzerLogs.Sort((ListItem lhs, ListItem rhs) => lhs.category.CompareTo(rhs.category));
		}

		protected override void DoListingItems()
		{
			string b = null;
			foreach (ListItem dataAnalyzerLog in this.dataAnalyzerLogs)
			{
				ListItem current = dataAnalyzerLog;
				if (current.category != b)
				{
					base.DoLabel(current.category);
					b = current.category;
				}
				base.DebugAction(current.title, current.action);
			}
			base.DoGap();
			Text.Font = GameFont.Small;
			base.DoLabel("Tables");
			MethodInfo[] methods = typeof(DataAnalysisTableMaker).GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo mi in methods)
			{
				string name = mi.Name;
				if (name.StartsWith("DoTable_"))
				{
					base.DebugAction(GenText.SplitCamelCase(name.Substring(8)), delegate
					{
						mi.Invoke(null, null);
					});
				}
			}
			base.DoGap();
			base.DoLabel("UI");
			base.DebugAction("Pawn column", delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					PawnColumnDef localDef = allDefsListForReading[j];
					list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate
					{
						Find.WindowStack.Add(new Dialog_PawnTableTest(localDef));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}
	}
}
