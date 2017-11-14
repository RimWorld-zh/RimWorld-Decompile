using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldFeatures : IExposable
	{
		public List<WorldFeature> features = new List<WorldFeature>();

		public bool textsCreated;

		private static List<TextMeshPro> texts = new List<TextMeshPro>();

		private const float BaseAlpha = 0.3f;

		private const float AlphaChangeSpeed = 5f;

		private static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		public void ExposeData()
		{
			Scribe_Collections.Look<WorldFeature>(ref this.features, "features", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				WorldGrid grid = Find.WorldGrid;
				if (grid.tileFeature != null && grid.tileFeature.Length != 0)
				{
					DataSerializeUtility.LoadUshort(grid.tileFeature, grid.TilesCount, delegate(int i, ushort data)
					{
						grid[i].feature = ((data != 65535) ? this.GetFeatureWithID(data) : null);
					});
				}
				this.textsCreated = false;
			}
		}

		public void UpdateFeatures()
		{
			if (!this.textsCreated)
			{
				this.textsCreated = true;
				this.CreateTextsAndSetPosition();
			}
			bool showWorldFeatures = Find.PlaySettings.showWorldFeatures;
			for (int i = 0; i < this.features.Count; i++)
			{
				Vector3 position = WorldFeatures.texts[i].transform.position;
				bool flag = showWorldFeatures && !WorldRendererUtility.HiddenBehindTerrainNow(position);
				if (flag != WorldFeatures.texts[i].gameObject.activeInHierarchy)
				{
					WorldFeatures.texts[i].gameObject.SetActive(flag);
					this.WrapAroundPlanetSurface(WorldFeatures.texts[i]);
				}
				if (flag)
				{
					this.UpdateAlpha(WorldFeatures.texts[i], this.features[i]);
				}
			}
		}

		public WorldFeature GetFeatureWithID(int uniqueID)
		{
			for (int i = 0; i < this.features.Count; i++)
			{
				if (this.features[i].uniqueID == uniqueID)
				{
					return this.features[i];
				}
			}
			return null;
		}

		private void UpdateAlpha(TextMeshPro text, WorldFeature feature)
		{
			float num = (float)(0.30000001192092896 * feature.alpha);
			Color color = text.color;
			if (color.a != num)
			{
				text.color = new Color(1f, 1f, 1f, num);
				this.WrapAroundPlanetSurface(text);
			}
			float num2 = (float)(Time.deltaTime * 5.0);
			if (this.GoodCameraAltitudeFor(text))
			{
				feature.alpha += num2;
			}
			else
			{
				feature.alpha -= num2;
			}
			feature.alpha = Mathf.Clamp01(feature.alpha);
		}

		private bool GoodCameraAltitudeFor(TextMeshPro text)
		{
			float renderedHeight = text.renderedHeight;
			float altitude = Find.WorldCameraDriver.altitude;
			float num = (float)(1.0 / (altitude / 125.0 * (altitude / 125.0)));
			renderedHeight *= num;
			if ((int)Find.WorldCameraDriver.CurrentZoom <= 0 && renderedHeight >= 0.56000000238418579)
			{
				return false;
			}
			if (renderedHeight < 0.037999998778104782)
			{
				return Find.WorldCameraDriver.AltitudePercent <= 0.070000000298023224;
			}
			if (renderedHeight > 0.81999999284744263)
			{
				return Find.WorldCameraDriver.AltitudePercent >= 0.34999999403953552;
			}
			return true;
		}

		private void CreateTextsAndSetPosition()
		{
			this.CreateOrDestroyTexts();
			WorldGrid worldGrid = Find.WorldGrid;
			float averageTileSize = worldGrid.averageTileSize;
			for (int i = 0; i < this.features.Count; i++)
			{
				WorldFeatures.texts[i].text = this.features[i].name;
				WorldFeatures.texts[i].rectTransform.sizeDelta = new Vector2(this.features[i].maxDrawSizeInTiles.x * averageTileSize, this.features[i].maxDrawSizeInTiles.y * averageTileSize);
				Vector3 normalized = this.features[i].drawCenter.normalized;
				Quaternion lhs = Quaternion.LookRotation(Vector3.Cross(normalized, Vector3.up), normalized);
				lhs *= Quaternion.Euler(Vector3.right * 90f);
				lhs *= Quaternion.Euler(Vector3.forward * (float)(90.0 - this.features[i].drawAngle));
				WorldFeatures.texts[i].transform.rotation = lhs;
				WorldFeatures.texts[i].transform.localPosition = this.features[i].drawCenter;
				this.WrapAroundPlanetSurface(WorldFeatures.texts[i]);
				WorldFeatures.texts[i].gameObject.SetActive(false);
			}
		}

		private void CreateOrDestroyTexts()
		{
			while (WorldFeatures.texts.Count > this.features.Count)
			{
				Object.Destroy(WorldFeatures.texts[WorldFeatures.texts.Count - 1]);
				WorldFeatures.texts.RemoveLast();
			}
			while (WorldFeatures.texts.Count < this.features.Count)
			{
				GameObject gameObject = Object.Instantiate(WorldFeatures.WorldTextPrefab);
				Object.DontDestroyOnLoad(gameObject);
				TextMeshPro component = gameObject.GetComponent<TextMeshPro>();
				component.color = new Color(1f, 1f, 1f, 0f);
				Material[] sharedMaterials = ((Component)component).GetComponent<MeshRenderer>().sharedMaterials;
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					sharedMaterials[i].renderQueue = WorldMaterials.FeatureNameRenderQueue;
				}
				WorldFeatures.texts.Add(component);
			}
		}

		private void WrapAroundPlanetSurface(TextMeshPro textMesh)
		{
			TMP_TextInfo textInfo = textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount != 0)
			{
				textMesh.ForceMeshUpdate();
				Vector3[] vertices = textMesh.mesh.vertices;
				Vector3 extents = textMesh.bounds.extents;
				float num = (float)(extents.x * 2.0);
				float num2 = Find.WorldGrid.DistOnSurfaceToAngle(num);
				for (int i = 0; i < characterCount; i++)
				{
					TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
					if (tMP_CharacterInfo.isVisible)
					{
						int vertexIndex = tMP_CharacterInfo.vertexIndex;
						Vector3 a = vertices[vertexIndex] + vertices[vertexIndex + 1] + vertices[vertexIndex + 2] + vertices[vertexIndex + 3];
						a /= 4f;
						float num3 = (float)(a.x / (num / 2.0));
						bool flag = num3 >= 0.0;
						num3 = Mathf.Abs(num3);
						float num4 = (float)(num2 / 2.0 * num3);
						float num5 = (float)((180.0 - num4) / 2.0);
						float num6 = (float)(200.0 * Mathf.Tan((float)(num4 / 2.0 * 0.01745329238474369)));
						Vector3 vector = new Vector3((float)(Mathf.Sin((float)(num5 * 0.01745329238474369)) * num6 * ((!flag) ? -1.0 : 1.0)), a.y, Mathf.Cos((float)(num5 * 0.01745329238474369)) * num6);
						vector += new Vector3((float)(Mathf.Sin((float)(num4 * 0.01745329238474369)) * ((!flag) ? -1.0 : 1.0)), 0f, (float)(0.0 - Mathf.Cos((float)(num4 * 0.01745329238474369)))) * 0.06f;
						Vector3 b = vector - a;
						Vector3 a2 = vertices[vertexIndex] + b;
						Vector3 a3 = vertices[vertexIndex + 1] + b;
						Vector3 a4 = vertices[vertexIndex + 2] + b;
						Vector3 a5 = vertices[vertexIndex + 3] + b;
						Quaternion rotation = Quaternion.Euler(0f, (float)(num4 * ((!flag) ? 1.0 : -1.0)), 0f);
						a2 = rotation * (a2 - vector) + vector;
						a3 = rotation * (a3 - vector) + vector;
						a4 = rotation * (a4 - vector) + vector;
						a5 = rotation * (a5 - vector) + vector;
						vertices[vertexIndex] = a2;
						vertices[vertexIndex + 1] = a3;
						vertices[vertexIndex + 2] = a4;
						vertices[vertexIndex + 3] = a5;
					}
				}
				textMesh.mesh.vertices = vertices;
			}
		}
	}
}
