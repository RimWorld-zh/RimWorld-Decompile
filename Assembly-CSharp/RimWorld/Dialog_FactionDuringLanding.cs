using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Dialog_FactionDuringLanding : Window
	{
		private Vector2 scrollPosition = Vector2.zero;

		private float scrollViewHeight;

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		public Dialog_FactionDuringLanding()
		{
			base.doCloseButton = true;
			base.closeOnEscapeKey = true;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(inRect, ref this.scrollPosition, ref this.scrollViewHeight);
		}
	}
}
