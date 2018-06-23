using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FB RID: 2043
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x04001811 RID: 6161
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x04001812 RID: 6162
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04001813 RID: 6163
		protected KeyBindingDef keyDef;

		// Token: 0x04001814 RID: 6164
		protected KeyPrefs.BindingSlot slot;

		// Token: 0x06002D84 RID: 11652 RVA: 0x0017F604 File Offset: 0x0017DA04
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
		// (get) Token: 0x06002D85 RID: 11653 RVA: 0x0017F668 File Offset: 0x0017DA68
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002D86 RID: 11654 RVA: 0x0017F684 File Offset: 0x0017DA84
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x0017F6A0 File Offset: 0x0017DAA0
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
