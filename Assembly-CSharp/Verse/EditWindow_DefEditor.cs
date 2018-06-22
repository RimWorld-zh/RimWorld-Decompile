using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E4A RID: 3658
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x06005649 RID: 22089 RVA: 0x002C7A78 File Offset: 0x002C5E78
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x0600564A RID: 22090 RVA: 0x002C7AC8 File Offset: 0x002C5EC8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x0600564B RID: 22091 RVA: 0x002C7AEC File Offset: 0x002C5EEC
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x002C7B04 File Offset: 0x002C5F04
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
	}
}
