using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E37 RID: 3639
	[HasDebugOutput]
	public class Dialog_PawnTableTest : Window
	{
		// Token: 0x040038E2 RID: 14562
		private PawnColumnDef singleColumn;

		// Token: 0x040038E3 RID: 14563
		private PawnTable pawnTableMin;

		// Token: 0x040038E4 RID: 14564
		private PawnTable pawnTableOptimal;

		// Token: 0x040038E5 RID: 14565
		private PawnTable pawnTableMax;

		// Token: 0x040038E6 RID: 14566
		private const int TableTitleHeight = 30;

		// Token: 0x06005621 RID: 22049 RVA: 0x002C657C File Offset: 0x002C497C
		public Dialog_PawnTableTest(PawnColumnDef singleColumn)
		{
			this.singleColumn = singleColumn;
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06005622 RID: 22050 RVA: 0x002C658C File Offset: 0x002C498C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06005623 RID: 22051 RVA: 0x002C65B4 File Offset: 0x002C49B4
		private List<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer).ToList<Pawn>();
			}
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x002C65E4 File Offset: 0x002C49E4
		public override void DoWindowContents(Rect inRect)
		{
			int num = ((int)inRect.height - 90) / 3;
			PawnTableDef pawnTableDef = new PawnTableDef();
			pawnTableDef.columns = new List<PawnColumnDef>
			{
				this.singleColumn
			};
			pawnTableDef.minWidth = 0;
			if (this.pawnTableMin == null)
			{
				this.pawnTableMin = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMin.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableOptimal == null)
			{
				this.pawnTableOptimal = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableOptimal.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableMax == null)
			{
				this.pawnTableMax = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMax.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), 0, num);
			}
			int num2 = 0;
			Text.Font = GameFont.Small;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Min size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMin.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Optimal size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableOptimal.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Max size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMax.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
		}

		// Token: 0x06005625 RID: 22053 RVA: 0x002C68B0 File Offset: 0x002C4CB0
		[DebugOutput]
		[Category("UI")]
		private static void PawnColumnTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnColumnDef localDef = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_PawnTableTest(localDef));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
