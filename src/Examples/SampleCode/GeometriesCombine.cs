

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Numerics;
using System.Runtime.CompilerServices;
using unvell.D2DLib.Examples.Demos;
using unvell.D2DLib.WinForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace unvell.D2DLib.Examples.SampleCode
{
	internal class GeometriesCombine : ExampleForm
	{
		private const FLOAT left = 250;
		private const FLOAT offset = 50f;
		private const FLOAT size = 100f;

		private const string fontName = "Arials";
		private const FLOAT fontSize = 20;

		public GeometriesCombine()
		{
			Text = "Demonstrate combining two geometries - d2dlib Examples";
		}

		protected override void OnRender(D2DGraphics g)
		{
			/// prepare 
			D2DColor borderColor = D2DColor.DarkGray;
			D2DColor fillColor = D2DColor.CornflowerBlue;
			fillColor.a = 0.25f;

			D2DCombineMode[] modes = {
				D2DCombineMode.Union,
				D2DCombineMode.Intersect,
				D2DCombineMode.Xor,
				D2DCombineMode.Exclude
			};

			string[] modeNames = { "Union", "Intersect", "XOR", "Exclude" };

			/// create overlapping test geometries
			D2DRect rect1 = new D2DRect(left + offset, offset, size, size);
			D2DGeometry geo1 = this.Device.CreateRectangleGeometry(new D2DRect(left + offset, offset, size, size));

			D2DRect rect2 = new D2DRect(rect1);
			rect2.Offset(size / 2, 0);
			D2DEllipse ellipse = new D2DEllipse(rect2);
			D2DGeometry geo2 = this.Device.CreateEllipseGeometry(ellipse);

			/// start drawing
			g.Clear(D2DColor.White);

			// draw test geometries without combining them
			D2DPoint pt = new D2DPoint(0, 0);
			DrawGeometries(geo1, geo2, g, pt, fillColor, borderColor, 2);

			// draw test geometries by combining them
			for (int i = 0; i < modes.Length; i++)
			{
				pt.Y += (size + offset / 2);

				DrawCombinedGeometries(geo1, geo2, modes[i], modeNames[i],
					g, pt, fillColor, borderColor, 2);
			}
			
			geo1?.Dispose();
			geo2?.Dispose();
		}

		/// <summary>
		/// Draw the geometry combining G1 and G2 geometries using combineMode 
		/// </summary>
		protected void DrawCombinedGeometries(D2DGeometry G1, D2DGeometry G2,
			D2DCombineMode combineMode, string modeName, D2DGraphics g, D2DPoint topLeft,
			D2DColor fillColor, D2DColor borderColor, float borderWidth)
		{
			using (var path = Device.CreateCombinedGeometry(G1, G2, combineMode))
			{
				g.PushTransform();
				g.TranslateTransform(topLeft.X, topLeft.Y);

				g.DrawText(modeName, D2DColor.DimGray, fontName, fontSize, offset, offset + size / 2 - fontSize / 2);

				g.FillPath(path, fillColor);
				g.DrawPath(path, borderColor, borderWidth);

				g.PopTransform();
			}
		}

		/// <summary>
		/// Draw G1 and G2 geometries without combining them
		/// </summary>
		protected void DrawGeometries(D2DGeometry G1, D2DGeometry G2,
			D2DGraphics g, D2DPoint topLeft, D2DColor fillColor, D2DColor borderColor, float borderWidth)
		{
			g.PushTransform();
			g.TranslateTransform(topLeft.X, topLeft.Y);

			g.DrawText("Before combining", D2DColor.DimGray, fontName, fontSize, offset, offset + size / 2 - fontSize / 2);

			g.FillPath(G1, fillColor);
			g.DrawPath(G1, borderColor, borderWidth);

			g.FillPath(G2, fillColor);
			g.DrawPath(G2, borderColor, borderWidth);

			g.PopTransform();
		}
	}
}
