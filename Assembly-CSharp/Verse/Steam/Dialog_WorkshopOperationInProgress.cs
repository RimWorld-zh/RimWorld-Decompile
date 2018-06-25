using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FC1 RID: 4033
	internal class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x06006172 RID: 24946 RVA: 0x00313C9A File Offset: 0x0031209A
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06006173 RID: 24947 RVA: 0x00313CC8 File Offset: 0x003120C8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x00313CEC File Offset: 0x003120EC
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

		// Token: 0x06006175 RID: 24949 RVA: 0x00313D98 File Offset: 0x00312198
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
