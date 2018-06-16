using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E3A RID: 3642
	public class Dialog_RenamePackage : Window
	{
		// Token: 0x0600560B RID: 22027 RVA: 0x002C4CB6 File Offset: 0x002C30B6
		public Dialog_RenamePackage(DefPackage renamingPackage)
		{
			this.renamingPackage = renamingPackage;
			this.proposedName = renamingPackage.fileName;
			this.doCloseX = true;
			this.forcePause = true;
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x0600560C RID: 22028 RVA: 0x002C4CE0 File Offset: 0x002C30E0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 200f);
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x0600560D RID: 22029 RVA: 0x002C4D04 File Offset: 0x002C3104
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x002C4D1C File Offset: 0x002C311C
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

		// Token: 0x0600560F RID: 22031 RVA: 0x002C4D9C File Offset: 0x002C319C
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

		// Token: 0x040038DB RID: 14555
		private DefPackage renamingPackage;

		// Token: 0x040038DC RID: 14556
		private string proposedName;
	}
}
