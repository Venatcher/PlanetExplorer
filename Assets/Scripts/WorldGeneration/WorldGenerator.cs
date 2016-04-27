using System;
using AccidentalNoise;
using UnityEngine;

namespace WorldGeneration
{
		public class WorldGenerator
		{
				// Variables générales de la carte
				int Seed; // valeur pour calculter le bruit
				int Width = 512;
				int Height = 512;

				// Variables de la HeightMap (matrice de l'altitude)
				ImplicitFractal HeightMap;
				int TerrainOctaves = 6;
				double TerrainFrequency = 1.25;

				// Variables de la HeatMap (matrice de la température)
				ImplicitCombiner HeatMap;
				int HeatOctaves = 4;
				double HeatFrequency = 3.0;

				// Variable de la MoistureMap (matrice de l'humidité)
				ImplicitFractal MoistureMap;
				int MoistureOctaves = 4;
				double MoistureFrequency = 3.0;

				// Notre objet MAP altitude contenant les info
				MapData HeightData;
				MapData HeatData;
				MapData MoistureData;

				public WorldGenerator ()
				{
						
						Seed = UnityEngine.Random.Range (0, int.MaxValue);
						Initialize ();
				}

				/// <summary>
				/// Initialisation des matrices de bruit
				/// </summary>
				private void Initialize ()
				{
						// Les différentes matrices de bruit sont générées à l'aide de la bibliothèque AccidentalNoise (http://accidentalnoise.sourceforge.net/)
						// Initialize the HeightMap Generator
						// Initialize the HeightMap Generator
						HeightMap = new ImplicitFractal (FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC, 
			                                 TerrainOctaves, TerrainFrequency, Seed);
			
						// Initialize the Heat map
						ImplicitGradient gradient = new ImplicitGradient (1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
						ImplicitFractal heatFractal = new ImplicitFractal (FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC, 
			                                                   HeatOctaves, HeatFrequency, Seed);
			
						HeatMap = new ImplicitCombiner (CombinerType.MULTIPLY);
						HeatMap.AddSource (gradient);
						HeatMap.AddSource (heatFractal);
			
						//moisture map
						MoistureMap = new ImplicitFractal (FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC, 
			                                   MoistureOctaves, MoistureFrequency, Seed);
				}
		
		
				// Extract les données de la matrice de bruit et les renseigne dans notre objet MapData
				private void GetData ()
				{
						HeightData = new MapData (Width, Height);
						HeatData = new MapData (Width, Height);
						MoistureData = new MapData (Width, Height);
			
						// loop through each x,y point - get height value
						for (var x = 0; x < Width; x++) {
								for (var y = 0; y < Height; y++) {
										
										//Noise range
										float x1 = 0, x2 = 1;
										float y1 = 0, y2 = 1;				
										float dx = x2 - x1;
										float dy = y2 - y1;
									
										//Sample noise at smaller intervals
										float s = x / (float)Width;
										float t = y / (float)Height;
									
										//Wrap on x-axis only
										// Calculate our 3D coordinates
										float nx = x1 + Mathf.Cos (s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
										float ny = x1 + Mathf.Sin (s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
										float nz = t;
									
										float heightValue = (float)HeightMap.Get (nx, ny, nz);
										float heatValue = (float)HeatMap.Get (nx, ny, nz);
										float moistureValue = (float)MoistureMap.Get (nx, ny, nz);
										//END Wrap on x-axis only

										//			// WRAP ON BOTH AXIS
										//			// Calculate our 4D coordinates
										//			float nx = x1 + Mathf.Cos (s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
										//			float ny = y1 + Mathf.Cos (t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
										//			float nz = x1 + Mathf.Sin (s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
										//			float nw = y1 + Mathf.Sin (t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
					
										//			float heightValue = (float)HeightMap.Get (nx, ny, nz, nw);
										//			float heatValue = (float)HeatMap.Get (nx, ny, nz, nw);
										//			float moistureValue = (float)MoistureMap.Get (nx, ny, nz, nw);
										//			// END WRAP ON BOTH AXIS

										// keep track of the max and min values found
										if (heightValue > HeightData.Max)
												HeightData.Max = heightValue;
										if (heightValue < HeightData.Min)
												HeightData.Min = heightValue;
					
										if (heatValue > HeatData.Max)
												HeatData.Max = heatValue;
										if (heatValue < HeatData.Min)
												HeatData.Min = heatValue;
					
										if (moistureValue > MoistureData.Max)
												MoistureData.Max = moistureValue;
										if (moistureValue < MoistureData.Min)
												MoistureData.Min = moistureValue;
										
										// Renseigner la valeur en xy dans nous différentes MAP
										HeightData.Data [x, y] = heightValue;
										HeatData.Data [x, y] = heatValue;
										MoistureData.Data [x, y] = moistureValue;	
								} // for y
						}// fot x	
				} // GetData()
		} // Class WorldGenerator
}

