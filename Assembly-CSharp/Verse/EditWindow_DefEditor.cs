using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E4C RID: 3660
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x0600562B RID: 22059 RVA: 0x002C5E88 File Offset: 0x002C4288
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x0600562C RID: 22060 RVA: 0x002C5ED8 File Offset: 0x002C42D8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x0600562D RID: 22061 RVA: 0x002C5EFC File Offset: 0x002C42FC
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x002C5F14 File Offset: 0x002C4314
		public override void DoWindowContents(Rect inRect)
		{
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape))
			{
				UI.UnfocusCurrentControl();
			}
			Rect rect = new Rect(0f, 0f, inRect.width, 16f);
			this.labelColumnWidth = Widgets.HorizontalSlider(rect, this.labelColumnWidth, 0f, inRect.width, false, null, null, null, -1f);
			Rect outRect = inRect.AtZero();
			outRect.yMin += 16f;
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Listing_TreeDefs listing_TreeDefs = new Listing_TreeDefs(this.labelColumnWidth);
			listing_TreeDefs.Begin(rect2);
			TreeNode_Editor node = EditTreeNodeDatabase.RootOf(this.def);
			listing_TreeDefs.ContentLines(node, 0);
			listing_TreeDefs.End();
			if (Event.current.type == EventType.Layout)
			{
				this.viewHeight = listing_TreeDefs.CurHeight + 200f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x040038EA RID: 14570
		public Def def;

		// Token: 0x040038EB RID: 14571
		private float viewHeight;

		// Token: 0x040038EC RID: 14572
		private Vector2 scrollPosition = default(Vector2);

		// Token: 0x040038ED RID: 14573
		private float labelColumnWidth = 140f;

		// Token: 0x040038EE RID: 14574
		private const float TopAreaHeight = 16f;

		// Token: 0x040038EF RID: 14575
		private const float ExtraScrollHeight = 200f;
	}
}
