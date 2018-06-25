using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x02000BDC RID: 3036
	public static class Current
	{
		// Token: 0x04002D4C RID: 11596
		private static ProgramState programStateInt = ProgramState.Entry;

		// Token: 0x04002D4D RID: 11597
		private static Root rootInt;

		// Token: 0x04002D4E RID: 11598
		private static Root_Entry rootEntryInt;

		// Token: 0x04002D4F RID: 11599
		private static Root_Play rootPlayInt;

		// Token: 0x04002D50 RID: 11600
		private static Camera cameraInt;

		// Token: 0x04002D51 RID: 11601
		private static CameraDriver cameraDriverInt;

		// Token: 0x04002D52 RID: 11602
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04002D53 RID: 11603
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04002D54 RID: 11604
		private static Game gameInt;

		// Token: 0x04002D55 RID: 11605
		private static World creatingWorldInt;

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x0022E264 File Offset: 0x0022C664
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x0022E280 File Offset: 0x0022C680
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x0022E29C File Offset: 0x0022C69C
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x0600423D RID: 16957 RVA: 0x0022E2B8 File Offset: 0x0022C6B8
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x0600423E RID: 16958 RVA: 0x0022E2D4 File Offset: 0x0022C6D4
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x0600423F RID: 16959 RVA: 0x0022E2F0 File Offset: 0x0022C6F0
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x0022E30C File Offset: 0x0022C70C
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06004241 RID: 16961 RVA: 0x0022E328 File Offset: 0x0022C728
		// (set) Token: 0x06004242 RID: 16962 RVA: 0x0022E342 File Offset: 0x0022C742
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06004243 RID: 16963 RVA: 0x0022E34C File Offset: 0x0022C74C
		// (set) Token: 0x06004244 RID: 16964 RVA: 0x0022E366 File Offset: 0x0022C766
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004245 RID: 16965 RVA: 0x0022E370 File Offset: 0x0022C770
		// (set) Token: 0x06004246 RID: 16966 RVA: 0x0022E38A File Offset: 0x0022C78A
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0022E394 File Offset: 0x0022C794
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
			}
			else if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}
	}
}
