
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ThreeDimensionalVisualizationLibrary.Objects;
using MathematicsLibrary.Interpolation;
using MathematicsLibrary.Geometry;
using ThreeDimensionalVisualizationLibrary;
using ThreeDimensionalVisualizationLibrary.Faces;

namespace FaceApplication.AddedClasses
{
    [DataContract]
    public class BodyPart : Face
    {
        private List<BezierCurve> horizontalBezierCurveList = null;
        private int numberOfLongitudePoints;
        public void Initialize()
        {
            vertexSize = 3f;  // A bit ugly to hard-code, but OK ...
            wireFrameWidth = 1.5f; // A bit ugly to hard-code, but OK ...
            horizontalBezierCurveList = new List<BezierCurve>();
            // Some arbitrary hard-coding here:
            BezierCurve bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0, 1);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.025, 0.999);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.075, 0.99);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.15, 0.98);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.3, 0.93);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.4, 0.875);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.5, 0.80);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.55, 0.70);
            horizontalBezierCurveList.Add(bezierCurve);


            //smaller beizercurve is used for bodyparts


            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.55, 0.60);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.55, 0.50);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.525, 0.40);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.50, 0.30);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.15, 0.20);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.15, 0.10);
            horizontalBezierCurveList.Add(bezierCurve);
            bezierCurve = new BezierCurve();
            bezierCurve.GenerateCircle(32, 0.15, 0.00);
            horizontalBezierCurveList.Add(bezierCurve);
            
        }

        //cheat to reach Object3D.Generate
        public virtual void Object3DGenerate(List<double> parameterList)
        {
            vertexList = new List<Vertex3D>();
            triangleIndicesList = new List<TriangleIndices>();
            position = new double[] { 0f, 0f, 0f };
            rotation = new double[] { 0f, 0f, 0f };
        }

        //Generate+scale
        public void Generate(List<double> parameterList, float scale)
        {
            Object3DGenerate(parameterList);
            if (parameterList == null) { return; }
            if (parameterList.Count < 1) { return; }
            numberOfLongitudePoints = (int)Math.Round(parameterList[0]);
            double deltaU = 1 / (double)numberOfLongitudePoints;
            List<List<List<double>>> pointList = new List<List<List<double>>>();
            for (int iZ = 0; iZ < horizontalBezierCurveList.Count; iZ++)
            {
                List<List<double>> slicePointList = new List<List<double>>();
                BezierCurve horizontalBezierCurve = horizontalBezierCurveList[iZ];
                double z= horizontalBezierCurve.SplineList[0].ControlPointList[0].CoordinateList[2]*scale ;
                for (int iLongitude = 0; iLongitude < numberOfLongitudePoints; iLongitude++)
                {
                    double uGlobal = iLongitude * deltaU;
                    if (iZ % 2 == 0) { uGlobal += deltaU / 2; } // Shift points in every other layer
                    PointND point = horizontalBezierCurve.GetPoint(uGlobal);
                    double x = point.CoordinateList[0] * scale;
                    double y= point.CoordinateList[1] * scale;
                    slicePointList.Add(new List<double>() { x, y, z });
                    Vertex3D vertex = new Vertex3D(x, y, z);
                    vertexList.Add(vertex);
                }
                pointList.Add(slicePointList);
            }

            for (int iZ = 1; iZ < horizontalBezierCurveList.Count; iZ++)
            {
                int iSumPrevious = (iZ - 1) * numberOfLongitudePoints;
                int iSum = iZ * numberOfLongitudePoints;
                for (int iPhi = 0; iPhi < numberOfLongitudePoints; iPhi++)
                {
                    if (iZ % 2 == 0)
                    {
                        int i1 = iSumPrevious + iPhi;
                        int i3 = iSum + iPhi;
                        int i2 = iSumPrevious + iPhi + 1;


                        if ((iPhi + 1) >= numberOfLongitudePoints) { i2 -= numberOfLongitudePoints; }
                        TriangleIndices triangleIndices1 = new TriangleIndices(i1, i2, i3);
                        triangleIndicesList.Add(triangleIndices1);
                        i1 = iSum + iPhi;
                        i3 = iSum + iPhi + 1;
                        i2 = iSumPrevious + iPhi + 1;
                        if ((iPhi + 1) >= numberOfLongitudePoints)
                        {
                            i2 -= numberOfLongitudePoints;
                            i3 -= numberOfLongitudePoints;
                        }
                        TriangleIndices triangleIndices2 = new TriangleIndices(i1, i2, i3);
                        triangleIndicesList.Add(triangleIndices2);
                    }
                    else
                    {
                        int i1 = iSumPrevious + iPhi;
                        int i3 = iSum + iPhi + 1;
                        int i2 = iSumPrevious + iPhi + 1;
                        if ((iPhi + 1) >= numberOfLongitudePoints)
                        {
                            i2 -= numberOfLongitudePoints;
                            i3 -= numberOfLongitudePoints;
                        }
                        TriangleIndices triangleIndices1 = new TriangleIndices(i1, i2, i3);
                        triangleIndicesList.Add(triangleIndices1);
                        i1 = iSum + iPhi;
                        i3 = iSum + iPhi + 1;
                        i2 = iSumPrevious + iPhi;
                        if ((iPhi + 1) >= numberOfLongitudePoints)
                        {
                            i3 -= numberOfLongitudePoints;
                        }
                        TriangleIndices triangleIndices2 = new TriangleIndices(i1, i2, i3);
                        triangleIndicesList.Add(triangleIndices2);
                    }
                }
            }
            GenerateTriangleConnectionLists();
            ComputeTriangleNormalVectors();
            ComputeVertexNormalVectors();
        }

    }
}
