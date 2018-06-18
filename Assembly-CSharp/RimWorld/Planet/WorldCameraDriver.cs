using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000586 RID: 1414
	public class WorldCameraDriver : MonoBehaviour
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000E7080 File Offset: 0x000E5480
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

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x000E70B8 File Offset: 0x000E54B8
		public WorldCameraZoomRange CurrentZoom
		{
			get
			{
				float altitudePercent = this.AltitudePercent;
				WorldCameraZoomRange result;
				if (altitudePercent < 0.025f)
				{
					result = WorldCameraZoomRange.VeryClose;
				}
				else if (altitudePercent < 0.042f)
				{
					result = WorldCameraZoomRange.Close;
				}
				else if (altitudePercent < 0.125f)
				{
					result = WorldCameraZoomRange.Far;
				}
				else
				{
					result = WorldCameraZoomRange.VeryFar;
				}
				return result;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000E710C File Offset: 0x000E550C
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

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x000E713C File Offset: 0x000E553C
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x000E7164 File Offset: 0x000E5564
		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(125f, 1100f, this.altitude);
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x000E7190 File Offset: 0x000E5590
		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x000E71C0 File Offset: 0x000E55C0
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x000E71EF File Offset: 0x000E55EF
		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x000E7200 File Offset: 0x000E5600
		public void OnGUI()
		{
			GUI.depth = 100;
			if (!LongEventHandler.ShouldWaitForEvent)
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
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.FrameInteraction);
					}
					float num = 0f;
					if (Event.current.type == EventType.ScrollWheel)
					{
						num -= Event.current.delta.y * 0.1f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
					{
						num += 2f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
					{
						num -= 2f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					this.desiredAltitude -= num * this.config.zoomSpeed * this.altitude / 12f;
					this.desiredAltitude = Mathf.Clamp(this.desiredAltitude, 125f, 1100f);
					this.desiredRotation = Vector2.zero;
					if (KeyBindingDefOf.MapDolly_Left.IsDown)
					{
						this.desiredRotation.x = -this.config.dollyRateKeys;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapDolly_Right.IsDown)
					{
						this.desiredRotation.x = this.config.dollyRateKeys;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapDolly_Up.IsDown)
					{
						this.desiredRotation.y = this.config.dollyRateKeys;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapDolly_Down.IsDown)
					{
						this.desiredRotation.y = -this.config.dollyRateKeys;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (this.mouseDragVect != Vector2.zero)
					{
						this.mouseDragVect *= CameraDriver.HitchReduceFactor;
						this.mouseDragVect.x = this.mouseDragVect.x * -1f;
						this.desiredRotation += this.mouseDragVect * this.config.dollyRateMouseDrag;
						this.mouseDragVect = Vector2.zero;
					}
					this.config.ConfigOnGUI();
				}
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000E74C4 File Offset: 0x000E58C4
		public void Update()
		{
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				if (Find.World == null)
				{
					this.MyCamera.gameObject.SetActive(false);
				}
				else
				{
					if (!Find.WorldInterface.everReset)
					{
						Find.WorldInterface.Reset();
					}
					Vector2 lhs = this.CalculateCurInputDollyVect();
					if (lhs != Vector2.zero)
					{
						float d = (this.altitude - 125f) / 975f * 0.85f + 0.15f;
						this.rotationVelocity = new Vector2(lhs.x, lhs.y) * d;
					}
					if (!this.AnythingPreventsCameraMotion)
					{
						float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
						this.sphereRotation *= Quaternion.AngleAxis(this.rotationVelocity.x * num * this.config.rotationSpeedScale, this.MyCamera.transform.up);
						this.sphereRotation *= Quaternion.AngleAxis(-this.rotationVelocity.y * num * this.config.rotationSpeedScale, this.MyCamera.transform.right);
					}
					int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
					for (int i = 0; i < num2; i++)
					{
						if (this.rotationVelocity != Vector2.zero)
						{
							this.rotationVelocity *= this.config.camRotationDecayFactor;
							if (this.rotationVelocity.magnitude < 0.05f)
							{
								this.rotationVelocity = Vector2.zero;
							}
						}
						if (this.config.smoothZoom)
						{
							float num3 = Mathf.Lerp(this.altitude, this.desiredAltitude, 0.05f);
							this.desiredAltitude += (num3 - this.altitude) * this.config.zoomPreserveFactor;
							this.altitude = num3;
						}
						else
						{
							float num4 = this.desiredAltitude - this.altitude;
							float num5 = num4 * 0.4f;
							this.desiredAltitude += this.config.zoomPreserveFactor * num5;
							this.altitude += num5;
						}
					}
					this.rotationAnimation_lerpFactor += Time.deltaTime * 8f;
					if (Find.PlaySettings.lockNorthUp)
					{
						this.RotateSoNorthIsUp(false);
						this.ClampXRotation(ref this.sphereRotation);
					}
					for (int j = 0; j < num2; j++)
					{
						this.config.ConfigFixedUpdate_60(ref this.rotationVelocity);
					}
					this.ApplyPositionToGameObject();
				}
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x000E7794 File Offset: 0x000E5B94
		private void ApplyPositionToGameObject()
		{
			Quaternion rotation;
			if (this.rotationAnimation_lerpFactor < 1f)
			{
				rotation = Quaternion.Lerp(this.rotationAnimation_prevSphereRotation, this.sphereRotation, this.rotationAnimation_lerpFactor);
			}
			else
			{
				rotation = this.sphereRotation;
			}
			if (Find.PlaySettings.lockNorthUp)
			{
				this.ClampXRotation(ref rotation);
			}
			this.MyCamera.transform.rotation = Quaternion.Inverse(rotation);
			Vector3 a = this.MyCamera.transform.rotation * Vector3.forward;
			this.MyCamera.transform.position = -a * this.altitude;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000E7840 File Offset: 0x000E5C40
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredRotation;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Rect rect = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect2 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect3 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				WorldInspectPane inspectPane = Find.World.UI.inspectPane;
				if (Find.WindowStack.IsOpen<WorldInspectPane>() && inspectPane.RecentHeight > rect2.height)
				{
					rect2.yMin = (float)UI.screenHeight - inspectPane.RecentHeight;
				}
				if (!rect2.Contains(mousePositionOnUIInverted) && !rect3.Contains(mousePositionOnUIInverted) && !rect.Contains(mousePositionOnUIInverted))
				{
					Vector2 zero = Vector2.zero;
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						zero.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						zero.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						zero.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							zero.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += zero;
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

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000E7AE6 File Offset: 0x000E5EE6
		public void ResetAltitude()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.altitude = 160f;
			}
			else
			{
				this.altitude = 550f;
			}
			this.desiredAltitude = this.altitude;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000E7B1B File Offset: 0x000E5F1B
		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000E7B53 File Offset: 0x000E5F53
		public void JumpTo(int tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x000E7B68 File Offset: 0x000E5F68
		public void RotateSoNorthIsUp(bool interpolate = true)
		{
			if (interpolate)
			{
				this.rotationAnimation_prevSphereRotation = this.sphereRotation;
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(Quaternion.Inverse(this.sphereRotation) * Vector3.forward));
			if (interpolate)
			{
				this.rotationAnimation_lerpFactor = 0f;
			}
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000E7BC0 File Offset: 0x000E5FC0
		private void ClampXRotation(ref Quaternion invRot)
		{
			Vector3 eulerAngles = Quaternion.Inverse(invRot).eulerAngles;
			float altitudePercent = this.AltitudePercent;
			float num = Mathf.Lerp(88.6f, 78f, altitudePercent);
			bool flag = false;
			if (eulerAngles.x <= 90f)
			{
				if (eulerAngles.x > num)
				{
					eulerAngles.x = num;
					flag = true;
				}
			}
			else if (eulerAngles.x < 360f - num)
			{
				eulerAngles.x = 360f - num;
				flag = true;
			}
			if (flag)
			{
				invRot = Quaternion.Inverse(Quaternion.Euler(eulerAngles));
			}
		}

		// Token: 0x04000FBE RID: 4030
		public WorldCameraConfig config = new WorldCameraConfig_Normal();

		// Token: 0x04000FBF RID: 4031
		public Quaternion sphereRotation = Quaternion.identity;

		// Token: 0x04000FC0 RID: 4032
		private Vector2 rotationVelocity;

		// Token: 0x04000FC1 RID: 4033
		private Vector2 desiredRotation;

		// Token: 0x04000FC2 RID: 4034
		private float desiredAltitude;

		// Token: 0x04000FC3 RID: 4035
		public float altitude;

		// Token: 0x04000FC4 RID: 4036
		private Camera cachedCamera;

		// Token: 0x04000FC5 RID: 4037
		private Vector2 mouseDragVect;

		// Token: 0x04000FC6 RID: 4038
		private bool mouseCoveredByUI;

		// Token: 0x04000FC7 RID: 4039
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04000FC8 RID: 4040
		private float fixedTimeStepBuffer;

		// Token: 0x04000FC9 RID: 4041
		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		// Token: 0x04000FCA RID: 4042
		private float rotationAnimation_lerpFactor = 1f;

		// Token: 0x04000FCB RID: 4043
		private const float SphereRadius = 100f;

		// Token: 0x04000FCC RID: 4044
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04000FCD RID: 4045
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x04000FCE RID: 4046
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x04000FCF RID: 4047
		private const float MaxXRotationAtMinAltitude = 88.6f;

		// Token: 0x04000FD0 RID: 4048
		private const float MaxXRotationAtMaxAltitude = 78f;

		// Token: 0x04000FD1 RID: 4049
		private const float StartingAltitude_Playing = 160f;

		// Token: 0x04000FD2 RID: 4050
		private const float StartingAltitude_Entry = 550f;

		// Token: 0x04000FD3 RID: 4051
		public const float MinAltitude = 125f;

		// Token: 0x04000FD4 RID: 4052
		private const float MaxAltitude = 1100f;

		// Token: 0x04000FD5 RID: 4053
		private const float ZoomTightness = 0.4f;

		// Token: 0x04000FD6 RID: 4054
		private const float ZoomScaleFromAltDenominator = 12f;

		// Token: 0x04000FD7 RID: 4055
		private const float PageKeyZoomRate = 2f;

		// Token: 0x04000FD8 RID: 4056
		private const float ScrollWheelZoomRate = 0.1f;
	}
}
