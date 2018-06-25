using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E4C RID: 3660
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x040038F7 RID: 14583
		public Def def;

		// Token: 0x040038F8 RID: 14584
		private float viewHeight;

		// Token: 0x040038F9 RID: 14585
		private Vector2 scrollPosition = default(Vector2);

		// Token: 0x040038FA RID: 14586
		private float labelColumnWidth = 140f;

		// Token: 0x040038FB RID: 14587
		private const float TopAreaHeight = 16f;

		// Token: 0x040038FC RID: 14588
		private const float ExtraScrollHeight = 200f;

		// Token: 0x0600564D RID: 22093 RVA: 0x002C7BA4 File Offset: 0x002C5FA4
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x0600564E RID: 22094 RVA: 0x002C7BF4 File Offset: 0x002C5FF4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x0600564F RID: 22095 RVA: 0x002C7C18 File Offset: 0x002C6018
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x002C7C30 File Offset: 0x002C6030
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
	}
}
