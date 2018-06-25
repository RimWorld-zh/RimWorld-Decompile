using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FD RID: 2045
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x04001815 RID: 6165
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x04001816 RID: 6166
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04001817 RID: 6167
		protected KeyBindingDef keyDef;

		// Token: 0x04001818 RID: 6168
		protected KeyPrefs.BindingSlot slot;

		// Token: 0x06002D87 RID: 11655 RVA: 0x0017F9B8 File Offset: 0x0017DDB8
		public Dialog_DefineBinding(KeyPrefsData keyPrefsData, KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			this.keyDef = keyDef;
			this.slot = slot;
			this.keyPrefsData = keyPrefsData;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002D88 RID: 11656 RVA: 0x0017FA1C File Offset: 0x0017DE1C
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002D89 RID: 11657 RVA: 0x0017FA38 File Offset: 0x0017DE38
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0017FA54 File Offset: 0x0017DE54
		public override void DoWindowContents(Rect inRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(inRect, "PressAnyKeyOrEsc".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
			{
				if (Event.current.keyCode != KeyCode.Escape)
				{
					this.keyPrefsData.EraseConflictingBindingsForKeyCode(this.keyDef, Event.current.keyCode, delegate(KeyBindingDef oldDef)
					{
						Messages.Message("KeyBindingOverwritten".Translate(new object[]
						{
							oldDef.LabelCap
						}), MessageTypeDefOf.TaskCompletion, false);
					});
					this.keyPrefsData.SetBinding(this.keyDef, this.slot, Event.current.keyCode);
				}
				this.Close(true);
				Event.current.Use();
			}
		}
	}
}
