using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E38 RID: 3640
	public class Dialog_RenamePackage : Window
	{
		// Token: 0x040038E7 RID: 14567
		private DefPackage renamingPackage;

		// Token: 0x040038E8 RID: 14568
		private string proposedName;

		// Token: 0x06005629 RID: 22057 RVA: 0x002C699E File Offset: 0x002C4D9E
		public Dialog_RenamePackage(DefPackage renamingPackage)
		{
			this.renamingPackage = renamingPackage;
			this.proposedName = renamingPackage.fileName;
			this.doCloseX = true;
			this.forcePause = true;
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x0600562A RID: 22058 RVA: 0x002C69C8 File Offset: 0x002C4DC8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 200f);
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x0600562B RID: 22059 RVA: 0x002C69EC File Offset: 0x002C4DEC
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x002C6A04 File Offset: 0x002C4E04
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			this.proposedName = Widgets.TextField(new Rect(0f, 0f, 200f, 32f), this.proposedName);
			if (Widgets.ButtonText(new Rect(0f, 40f, 100f, 32f), "Rename", true, false, true))
			{
				if (this.TrySave())
				{
					this.Close(true);
				}
			}
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x002C6A84 File Offset: 0x002C4E84
		private bool TrySave()
		{
			bool result;
			if (string.IsNullOrEmpty(this.proposedName) || this.proposedName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				Messages.Message("Invalid filename.", MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else if (Path.GetExtension(this.proposedName) != ".xml")
			{
				Messages.Message("Data package file names must end with .xml", MessageTypeDefOf.RejectInput, false);
				result = false;
			}
			else
			{
				this.renamingPackage.fileName = this.proposedName;
				result = true;
			}
			return result;
		}
	}
}
