using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E36 RID: 3638
	public class Dialog_RenamePackage : Window
	{
		// Token: 0x06005625 RID: 22053 RVA: 0x002C6872 File Offset: 0x002C4C72
		public Dialog_RenamePackage(DefPackage renamingPackage)
		{
			this.renamingPackage = renamingPackage;
			this.proposedName = renamingPackage.fileName;
			this.doCloseX = true;
			this.forcePause = true;
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06005626 RID: 22054 RVA: 0x002C689C File Offset: 0x002C4C9C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 200f);
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06005627 RID: 22055 RVA: 0x002C68C0 File Offset: 0x002C4CC0
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x002C68D8 File Offset: 0x002C4CD8
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

		// Token: 0x06005629 RID: 22057 RVA: 0x002C6958 File Offset: 0x002C4D58
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

		// Token: 0x040038E7 RID: 14567
		private DefPackage renamingPackage;

		// Token: 0x040038E8 RID: 14568
		private string proposedName;
	}
}
