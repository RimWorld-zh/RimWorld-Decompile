using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FBC RID: 4028
	internal class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x0600613B RID: 24891 RVA: 0x00310DC6 File Offset: 0x0030F1C6
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x0600613C RID: 24892 RVA: 0x00310DF4 File Offset: 0x0030F1F4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x00310E18 File Offset: 0x0030F218
		public override void DoWindowContents(Rect inRect)
		{
			EItemUpdateStatus eitemUpdateStatus;
			float num;
			Workshop.GetUpdateStatus(out eitemUpdateStatus, out num);
			WorkshopInteractStage curStage = Workshop.CurStage;
			if (curStage == WorkshopInteractStage.None && eitemUpdateStatus == EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				this.Close(true);
			}
			else
			{
				string text = "";
				if (curStage != WorkshopInteractStage.None)
				{
					text += curStage.GetLabel();
					text += "\n\n";
				}
				if (eitemUpdateStatus != EItemUpdateStatus.k_EItemUpdateStatusInvalid)
				{
					text += eitemUpdateStatus.GetLabel();
					if (num > 0f)
					{
						text = text + " (" + num.ToStringPercent() + ")";
					}
					text += GenText.MarchingEllipsis(0f);
				}
				Widgets.Label(inRect, text);
			}
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x00310EC4 File Offset: 0x0030F2C4
		public static void CloseAll()
		{
			Dialog_WorkshopOperationInProgress dialog_WorkshopOperationInProgress = Find.WindowStack.WindowOfType<Dialog_WorkshopOperationInProgress>();
			if (dialog_WorkshopOperationInProgress != null)
			{
				dialog_WorkshopOperationInProgress.Close(true);
			}
		}
	}
}
