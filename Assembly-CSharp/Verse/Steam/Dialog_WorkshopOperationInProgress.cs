using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000FBB RID: 4027
	internal class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x06006139 RID: 24889 RVA: 0x00310EA2 File Offset: 0x0030F2A2
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x0600613A RID: 24890 RVA: 0x00310ED0 File Offset: 0x0030F2D0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x00310EF4 File Offset: 0x0030F2F4
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

		// Token: 0x0600613C RID: 24892 RVA: 0x00310FA0 File Offset: 0x0030F3A0
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
