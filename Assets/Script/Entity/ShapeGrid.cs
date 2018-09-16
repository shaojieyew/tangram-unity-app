using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Entity
{

    public class ShapeGrid
    {
        public double x;
        public double y;
        public ShapeGrid(double inx, double iny)
        {
            x = inx;
            y = iny;
        }


        static ShapeGrid[] G0 = { new ShapeGrid(0.5, 0.5), new ShapeGrid(-0.5, 0.5), new ShapeGrid(0.5, -0.5), new ShapeGrid(-0.5, -0.5) };
        static ShapeGrid[] G1 = { new ShapeGrid(-0.5, 0.5), new ShapeGrid(0.5, -0.5), new ShapeGrid(-0.5, -0.5) };
        static ShapeGrid[] G2 = { new ShapeGrid(-0.5, 0.5), new ShapeGrid(0.5, -0.5), new ShapeGrid(-0.5, -0.5) };
        static ShapeGrid[] G3 = { new ShapeGrid(0, 0), new ShapeGrid(-0.7, 0.7), new ShapeGrid(0.7, -0.7), new ShapeGrid(-0.7, -0.7) };
        static ShapeGrid[] G4 = { new ShapeGrid(0, 0.5), new ShapeGrid(0, -0.5), new ShapeGrid(1, 0.5), new ShapeGrid(-1, -0.5) };
        static ShapeGrid[] G5 = { new ShapeGrid(0, 0), new ShapeGrid(-1, 1), new ShapeGrid(1, -1), new ShapeGrid(0, -1), new ShapeGrid(-1, 0), new ShapeGrid(-1, -1) };
        static ShapeGrid[] G6 = { new ShapeGrid(0, 0), new ShapeGrid(-1, 1), new ShapeGrid(1, -1), new ShapeGrid(0, -1), new ShapeGrid(-1, 0), new ShapeGrid(-1, -1) };
        public static ShapeGrid[][] SHAPESGRIDS = { G0, G1, G2, G3, G4, G5, G6 };
        public static ShapeGrid[][] getShapesGrid()
        {
            return SHAPESGRIDS;
        }
    }


}
