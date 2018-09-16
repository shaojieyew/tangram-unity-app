using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Entity;
using System;
using Assets.Script;
using Assets;

public class GridManager : MonoBehaviour {

    public static List<ShapeGrid> grids = null;
    
    public static List<ShapeGrid> getGrid()
    {
        return grids;
    }

    public static void loadGrid(GameObject[]  sih)
    {
        grids = new List<ShapeGrid>();
        foreach (GameObject shape in sih)
        {
            Debug.Log("Object: " + shape.name);
            double x = shape.transform.position.x;
            double y = shape.transform.position.y;
            double angle = shape.transform.rotation.eulerAngles.z;
            Debug.Log("Coord: " + x + "," + y);
            Debug.Log("Angle: " + angle);
            int index = int.Parse(shape.name);
            ShapeGrid[] shapeGrids = ShapeGrid.getShapesGrid()[index];
            foreach (ShapeGrid grid in shapeGrids)
            {
                ShapeGrid gridToAdd = getGridByOffset(grid.x, grid.y, x, y, angle);
                if (!isGridExist(gridToAdd))
                {
                    grids.Add(gridToAdd);
                    Debug.Log("Coord of grid point: "+gridToAdd.x + "," + gridToAdd.y);
                }
            }
        }
    }
    
    private static bool isGridExist(ShapeGrid ingrid)
    {
        if (grids == null)
            return false;
        foreach (ShapeGrid grid in grids)
        {
            if (Util.getDistance(ingrid.x, ingrid.y, grid.x, grid.y) < AppConstant.GRID_OVERLAP_TRESHOLD)
            {
                return true;
            }
        }
        return false;
    }

    public static ShapeGrid getGridByOffset(double x, double y, double xOffset, double yOffset, double angle)
    {
        angle = 360 - angle;
        double rad = angle * (Math.PI / 180.0);
        double x1 = (Math.Cos(rad) * x) + (Math.Sin(rad) * y) + xOffset;
        double y1 = (Math.Sin(rad) * x * -1) + (Math.Cos(rad) * y) + yOffset;
        return new ShapeGrid(x1, y1);
    }

}
