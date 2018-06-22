using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FBC RID: 4028
	internal class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x06006162 RID: 24930 RVA: 0x00312F76 File Offset: 0x00311376
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06006163 RID: 24931 RVA: 0x00312FA4 File Offset: 0x003113A4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x00312FC8 File Offset: 0x003113C8
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

		// Token: 0x06006165 RID: 24933 RVA: 0x00313074 File Offset: 0x00311474
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
