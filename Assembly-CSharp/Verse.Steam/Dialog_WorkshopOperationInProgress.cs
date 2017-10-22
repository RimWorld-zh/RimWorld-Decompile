using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	internal class Dialog_WorkshopOperationInProgress : Window
	{
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		public Dialog_WorkshopOperationInProgress()
		{
			base.forcePause = true;
			base.closeOnEscapeKey = false;
			base.absorbInputAroundWindow = true;
			base.preventDrawTutor = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			EItemUpdateStatus eItemUpdateStatus = default(EItemUpdateStatus);
			float num = default(float);
			Workshop.GetUpdateStatus(out eItemUpdateStatus, out num);
			WorkshopInteractStage curStage = Workshop.CurStage;
			if (curStage == WorkshopInteractStage.None && eItemUpdateStatus == EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				this.Close(true);
			}
			else
			{
				string text = string.Empty;
				if (curStage != 0)
				{
					text += curStage.GetLabel();
					text += "\n\n";
				}
				if (eItemUpdateStatus != 0)
				{
					text += eItemUpdateStatus.GetLabel();
					if (num > 0.0)
					{
						text = text + " (" + num.ToStringPercent() + ")";
					}
					text += GenText.MarchingEllipsis(0f);
				}
				Widgets.Label(inRect, text);
			}
		}

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
