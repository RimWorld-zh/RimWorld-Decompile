using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AE4 RID: 2788
	public class CameraDriver : MonoBehaviour
	{
		// Token: 0x04002706 RID: 9990
		public CameraShaker shaker = new CameraShaker();

		// Token: 0x04002707 RID: 9991
		private Camera cachedCamera = null;

		// Token: 0x04002708 RID: 9992
		private GameObject reverbDummy;

		// Token: 0x04002709 RID: 9993
		public CameraMapConfig config = new CameraMapConfig_Normal();

		// Token: 0x0400270A RID: 9994
		private Vector3 velocity;

		// Token: 0x0400270B RID: 9995
		private Vector3 rootPos;

		// Token: 0x0400270C RID: 9996
		private float rootSize;

		// Token: 0x0400270D RID: 9997
		private float desiredSize;

		// Token: 0x0400270E RID: 9998
		private Vector2 desiredDolly = Vector2.zero;

		// Token: 0x0400270F RID: 9999
		private Vector2 mouseDragVect = Vector2.zero;

		// Token: 0x04002710 RID: 10000
		private bool mouseCoveredByUI = false;

		// Token: 0x04002711 RID: 10001
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04002712 RID: 10002
		private float fixedTimeStepBuffer;

		// Token: 0x04002713 RID: 10003
		private static int lastViewRectGetFrame = -1;

		// Token: 0x04002714 RID: 10004
		private static CellRect lastViewRect;

		// Token: 0x04002715 RID: 10005
		public const float MaxDeltaTime = 0.1f;

		// Token: 0x04002716 RID: 10006
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04002717 RID: 10007
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x04002718 RID: 10008
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x04002719 RID: 10009
		private const float MapEdgeClampMarginCells = -2f;

		// Token: 0x0400271A RID: 10010
		public const float StartingSize = 24f;

		// Token: 0x0400271B RID: 10011
		private const float MinSize = 11f;

		// Token: 0x0400271C RID: 10012
		private const float MaxSize = 60f;

		// Token: 0x0400271D RID: 10013
		private const float ZoomTightness = 0.4f;

		// Token: 0x0400271E RID: 10014
		private const float ZoomScaleFromAltDenominator = 35f;

		// Token: 0x0400271F RID: 10015
		private const float PageKeyZoomRate = 4f;

		// Token: 0x04002720 RID: 10016
		private const float ScrollWheelZoomRate = 0.35f;

		// Token: 0x04002721 RID: 10017
		public const float MinAltitude = 15f;

		// Token: 0x04002722 RID: 10018
		private const float MaxAltitude = 65f;

		// Token: 0x04002723 RID: 10019
		private const float ReverbDummyAltitude = 65f;

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x0020A044 File Offset: 0x00208444
		private Camera MyCamera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x0020A07C File Offset: 0x0020847C
		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				float result;
				if (Screen.fullScreen)
				{
					result = 6f;
				}
				else
				{
					result = 20f;
				}
				return result;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x0020A0AC File Offset: 0x002084AC
		public CameraZoomRange CurrentZoom
		{
			get
			{
				CameraZoomRange result;
				if (this.rootSize < 12f)
				{
					result = CameraZoomRange.Closest;
				}
				else if (this.rootSize < 13.8f)
				{
					result = CameraZoomRange.Close;
				}
				else if (this.rootSize < 42f)
				{
					result = CameraZoomRange.Middle;
				}
				else if (this.rootSize < 57f)
				{
					result = CameraZoomRange.Far;
				}
				else
				{
					result = CameraZoomRange.Furthest;
				}
				return result;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x0020A120 File Offset: 0x00208520
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x0020A148 File Offset: 0x00208548
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x0020A174 File Offset: 0x00208574
		public IntVec3 MapPosition
		{
			get
			{
				IntVec3 result = this.CurrentRealPosition.ToIntVec3();
				result.y = 0;
				return result;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x0020A1A0 File Offset: 0x002085A0
		public CellRect CurrentViewRect
		{
			get
			{
				if (Time.frameCount != CameraDriver.lastViewRectGetFrame)
				{
					CameraDriver.lastViewRect = default(CellRect);
					float num = (float)UI.screenWidth / (float)UI.screenHeight;
					CameraDriver.lastViewRect.minX = Mathf.FloorToInt(this.CurrentRealPosition.x - this.rootSize * num - 1f);
					CameraDriver.lastViewRect.maxX = Mathf.CeilToInt(this.CurrentRealPosition.x + this.rootSize * num);
					CameraDriver.lastViewRect.minZ = Mathf.FloorToInt(this.CurrentRealPosition.z - this.rootSize - 1f);
					CameraDriver.lastViewRect.maxZ = Mathf.CeilToInt(this.CurrentRealPosition.z + this.rootSize);
					CameraDriver.lastViewRectGetFrame = Time.frameCount;
				}
				return CameraDriver.lastViewRect;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x0020A298 File Offset: 0x00208698
		public static float HitchReduceFactor
		{
			get
			{
				float result = 1f;
				if (Time.deltaTime > 0.1f)
				{
					result = 0.1f / Time.deltaTime;
				}
				return result;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003DDA RID: 15834 RVA: 0x0020A2D0 File Offset: 0x002086D0
		public float CellSizePixels
		{
			get
			{
				return (float)UI.screenHeight / (this.rootSize * 2f);
			}
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0020A2F8 File Offset: 0x002086F8
		public void Awake()
		{
			this.ResetSize();
			this.reverbDummy = GameObject.Find("ReverbZoneDummy");
			this.ApplyPositionToGameObject();
			this.MyCamera.farClipPlane = 71.5f;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0020A327 File Offset: 0x00208727
		public void OnPreRender()
		{
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				if (Find.CurrentMap == null)
				{
				}
			}
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0020A348 File Offset: 0x00208748
		public void OnPreCull()
		{
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				if (Find.CurrentMap != null)
				{
					if (!WorldRendererUtility.WorldRenderedNow)
					{
						Find.CurrentMap.weatherManager.DrawAllWeather();
					}
				}
			}
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0020A384 File Offset: 0x00208784
		public void OnGUI()
		{
			GUI.depth = 100;
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				if (Find.CurrentMap != null)
				{
					UnityGUIBugsFixer.OnGUI();
					this.mouseCoveredByUI = false;
					if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
					{
						this.mouseCoveredByUI = true;
					}
					if (!this.AnythingPreventsCameraMotion)
					{
						if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
						{
							this.mouseDragVect = Event.current.delta;
							Event.current.Use();
						}
						float num = 0f;
						if (Event.current.type == EventType.ScrollWheel)
						{
							num -= Event.current.delta.y * 0.35f;
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.TinyInteraction);
						}
						if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
						{
							num += 4f;
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
						}
						if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
						{
							num -= 4f;
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
						}
						this.desiredSize -= num * this.config.zoomSpeed * this.rootSize / 35f;
						this.desiredSize = Mathf.Clamp(this.desiredSize, 11f, 60f);
						this.desiredDolly = Vector3.zero;
						if (KeyBindingDefOf.MapDolly_Left.IsDown)
						{
							this.desiredDolly.x = -this.config.dollyRateKeys;
						}
						if (KeyBindingDefOf.MapDolly_Right.IsDown)
						{
							this.desiredDolly.x = this.config.dollyRateKeys;
						}
						if (KeyBindingDefOf.MapDolly_Up.IsDown)
						{
							this.desiredDolly.y = this.config.dollyRateKeys;
						}
						if (KeyBindingDefOf.MapDolly_Down.IsDown)
						{
							this.desiredDolly.y = -this.config.dollyRateKeys;
						}
						if (this.mouseDragVect != Vector2.zero)
						{
							this.mouseDragVect *= CameraDriver.HitchReduceFactor;
							this.mouseDragVect.x = this.mouseDragVect.x * -1f;
							this.desiredDolly += this.mouseDragVect * this.config.dollyRateMouseDrag;
							this.mouseDragVect = Vector2.zero;
						}
						this.config.ConfigOnGUI();
					}
				}
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0020A61C File Offset: 0x00208A1C
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				if (Current.SubcameraDriver != null)
				{
					Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				}
			}
			else if (Find.CurrentMap != null)
			{
				Vector2 lhs = this.CalculateCurInputDollyVect();
				if (lhs != Vector2.zero)
				{
					float d = (this.rootSize - 11f) / 49f * 0.7f + 0.3f;
					this.velocity = new Vector3(lhs.x, 0f, lhs.y) * d;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraDolly, KnowledgeAmount.FrameInteraction);
				}
				if (!this.AnythingPreventsCameraMotion)
				{
					float d2 = Time.deltaTime * CameraDriver.HitchReduceFactor;
					this.rootPos += this.velocity * d2 * this.config.moveSpeedScale;
					this.rootPos.x = Mathf.Clamp(this.rootPos.x, 2f, (float)Find.CurrentMap.Size.x + -2f);
					this.rootPos.z = Mathf.Clamp(this.rootPos.z, 2f, (float)Find.CurrentMap.Size.z + -2f);
				}
				int num = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
				for (int i = 0; i < num; i++)
				{
					if (this.velocity != Vector3.zero)
					{
						this.velocity *= this.config.camSpeedDecayFactor;
						if (this.velocity.magnitude < 0.1f)
						{
							this.velocity = Vector3.zero;
						}
					}
					if (this.config.smoothZoom)
					{
						float num2 = Mathf.Lerp(this.rootSize, this.desiredSize, 0.05f);
						this.desiredSize += (num2 - this.rootSize) * this.config.zoomPreserveFactor;
						this.rootSize = num2;
					}
					else
					{
						float num3 = this.desiredSize - this.rootSize;
						float num4 = num3 * 0.4f;
						this.desiredSize += this.config.zoomPreserveFactor * num4;
						this.rootSize += num4;
					}
					this.config.ConfigFixedUpdate_60(ref this.velocity);
				}
				this.shaker.Update();
				this.ApplyPositionToGameObject();
				Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				if (Find.CurrentMap != null)
				{
					RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
					rememberedCameraPos.rootPos = this.rootPos;
					rememberedCameraPos.rootSize = this.rootSize;
				}
			}
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0020A90C File Offset: 0x00208D0C
		private void ApplyPositionToGameObject()
		{
			this.rootPos.y = 15f + (this.rootSize - 11f) / 49f * 50f;
			this.MyCamera.orthographicSize = this.rootSize;
			this.MyCamera.transform.position = this.rootPos + this.shaker.ShakeOffset;
			Vector3 position = base.transform.position;
			position.y = 65f;
			this.reverbDummy.transform.position = position;
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0020A9A4 File Offset: 0x00208DA4
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredDolly;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 point = mousePositionOnUI;
				point.y = (float)UI.screenHeight - point.y;
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect4 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
				if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && mainTabWindow_Inspect.RecentHeight > rect3.height)
				{
					rect3.yMin = (float)UI.screenHeight - mainTabWindow_Inspect.RecentHeight;
				}
				if (!rect.Contains(point) && !rect3.Contains(point) && !rect2.Contains(point) && !rect4.Contains(point))
				{
					Vector2 b = new Vector2(0f, 0f);
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						b.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						b.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						b.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							b.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += b;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x0020AC98 File Offset: 0x00209098
		public void Expose()
		{
			if (Scribe.EnterNode("cameraMap"))
			{
				try
				{
					Scribe_Values.Look<Vector3>(ref this.rootPos, "camRootPos", default(Vector3), false);
					Scribe_Values.Look<float>(ref this.desiredSize, "desiredSize", 0f, false);
					this.rootSize = this.desiredSize;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0020AD14 File Offset: 0x00209114
		public void ResetSize()
		{
			this.desiredSize = 24f;
			this.rootSize = this.desiredSize;
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x0020AD2E File Offset: 0x0020912E
		public void JumpToCurrentMapLoc(IntVec3 cell)
		{
			this.JumpToCurrentMapLoc(cell.ToVector3Shifted());
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0020AD3E File Offset: 0x0020913E
		public void JumpToCurrentMapLoc(Vector3 loc)
		{
			this.rootPos = new Vector3(loc.x, this.rootPos.y, loc.z);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0020AD65 File Offset: 0x00209165
		public void SetRootPosAndSize(Vector3 rootPos, float rootSize)
		{
			this.rootPos = rootPos;
			this.rootSize = rootSize;
			this.desiredDolly = Vector2.zero;
			this.desiredSize = rootSize;
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}
	}
}
