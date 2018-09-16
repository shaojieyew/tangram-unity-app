using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Script.Entity;
using Assets.Script;
using Assets;

public class DragManager : MonoBehaviour
{
    private static bool draggingItem = false;
    private static GameObject draggedObject;
    private static Vector2 touchOffset;

    void Update ()
    {
        //on user touch screen
        if (HasInput)
        {
            DragOrPickUp();
            RestrictPosition();
        }
        else  //no touchs
        {
            //on user release drag item
            if (draggingItem)
            {
                DropItem();
                AutoFit();
                ValidateResult();
            }
        }
    }

    public void ValidateResult()
    {
        bool result = Validate.Validation();
        if (result)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void AutoFit()
    {
        //prepare grids
        List<ShapeGrid> grids = GridManager.getGrid();
        List<ShapeGrid> draggedGrids = new List<ShapeGrid>();
        List<ShapeGrid> toMatchGrids = new List<ShapeGrid>();

        //get the dragged obj's grids
        int index = int.Parse(draggedObject.name);
        ShapeGrid[] shapeGrids = ShapeGrid.getShapesGrid()[index];
        double x = draggedObject.transform.position.x;
        double y = draggedObject.transform.position.y;
        double angle = draggedObject.transform.rotation.eulerAngles.z;

        Debug.Log("Angle: " + angle);
        bool allGridPointHaveCandidate = true;
        double candidateDistanceTreshold = AppConstant.AUTOFIT_DISTANCE_THRESHOLD;
        //find candidate grid points
        foreach (ShapeGrid grid in shapeGrids)
        {
            bool match = false;
            ShapeGrid gridToAdd = GridManager.getGridByOffset(grid.x, grid.y, x, y, angle);
            Debug.Log("Coord: " + gridToAdd.x + "," + gridToAdd.y);
            draggedGrids.Add(gridToAdd);
            foreach (ShapeGrid resultGrid in grids)
            {
                double distance = Util.getDistance(gridToAdd.x, gridToAdd.y, resultGrid.x, resultGrid.y);
                if(distance< candidateDistanceTreshold)
                {
                    toMatchGrids.Add(resultGrid);
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                allGridPointHaveCandidate = false;
                Debug.Log("No Auto Match");
                break;
            }
        }

        //match the position and direction of the matched grid points
        if (allGridPointHaveCandidate)
        {
            float draggedGrids_x1 = (float)draggedGrids[0].x;
            float draggedGrids_y1 = (float)draggedGrids[0].y;
            float draggedGrids_x2 = (float)draggedGrids[1].x;
            float draggedGrids_y2 = (float)draggedGrids[1].y;

            float toMatchGrids_x1 = (float)toMatchGrids[0].x;
            float toMatchGrids_y1 = (float)toMatchGrids[0].y;
            float toMatchGrids_x2 = (float)toMatchGrids[1].x;
            float toMatchGrids_y2 = (float)toMatchGrids[1].y;

            //auto rotation
            Vector2 vFrom = new Vector2((draggedGrids_x2 - draggedGrids_x1), (draggedGrids_y2 - draggedGrids_y1));
            Vector2 vTo = new Vector2((toMatchGrids_x2 - toMatchGrids_x1), (toMatchGrids_y2 - toMatchGrids_y1));
            float angleDifferent = Vector2.Angle(vFrom, vTo);
            Vector3 cross = Vector3.Cross(vFrom, vTo);
            if (cross.z > 0)
                angleDifferent = 360 - angleDifferent;
            Debug.Log("Angle Diff:" + angleDifferent);
            Quaternion rotation = Quaternion.Euler(draggedObject.transform.rotation.eulerAngles.x, draggedObject.transform.rotation.eulerAngles.y, draggedObject.transform.rotation.eulerAngles.z- angleDifferent);
            draggedObject.transform.rotation = rotation;

            //auto position
            ShapeGrid newDraggedGrid =GridManager.getGridByOffset(shapeGrids[0].x, shapeGrids[0].y, draggedObject.transform.position.x, draggedObject.transform.position.y, draggedObject.transform.rotation.eulerAngles.z) ;
            float newDraggedGrid_x = (float)newDraggedGrid.x;
            float newDraggedGrid_y = (float)newDraggedGrid.y;
            float xDifferent = toMatchGrids_x1 - newDraggedGrid_x;
            float yDifferent = toMatchGrids_y1 - newDraggedGrid_y;
            Vector3 position = new Vector3(draggedObject.transform.position.x + xDifferent, draggedObject.transform.position.y + yDifferent, draggedObject.transform.position.z);
            draggedObject.transform.position = position;
        }
    }

    //limit the object position to be within the screen
    private void RestrictPosition()
    {
        if (draggedObject)
        {
            var dist = (draggedObject.transform.position - Camera.main.transform.position).z;
            float cameraXmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
            float cameraXmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
            float cameraYmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
            float cameraYmax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;
            
            float x = draggedObject.transform.position.x;
            float y = draggedObject.transform.position.y;
            float z = draggedObject.transform.position.z;
            if (x > cameraXmax)
                x = cameraXmax;
            if (x < cameraXmin)
                x = cameraXmin;
            if (y > cameraYmax)
                y = cameraYmax;
            if (y < cameraYmin)
                y = cameraYmin;
            draggedObject.transform.position = new Vector3(x, y, z);
        }
    }

    //get touched position
    Vector2 CurrentTouchPosition()
    {
        Vector3 touch = Input.mousePosition;
        if (Input.touchCount > 0)
        {
            touch= Input.GetTouch(0).position;
        }
        return Camera.main.ScreenToWorldPoint(touch);
    }

    //moving objects called by updates()
    private void DragOrPickUp()
    {
        var inputPosition = CurrentTouchPosition();
        if (draggingItem) //continuous touches, update position and angle
        {
            //update position
            draggedObject.transform.position = inputPosition + touchOffset;
            draggedObject.transform.position = new Vector3(draggedObject.transform.position.x, draggedObject.transform.position.y, draggedObject.transform.position.z - 5f);

            //update angle when 2 screen touch
            if (Input.touchCount == 2)
            {
                ObjectRotationUpdate(draggedObject);
            }
        }
        else //first touch on the object, hold the object in draggedObject and set draggingItem to true
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.tag == AppConstant.DRAGGABLE_OBJ_TAG)
                    {
                        draggingItem = true;
                        draggedObject = hit.transform.gameObject;
                        touchOffset = (Vector2)hit.transform.position - inputPosition;

                        draggedObject.transform.position = new Vector3(draggedObject.transform.position.x, draggedObject.transform.position.y, draggedObject.transform.position.z-5f);
                        draggedObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    }
                }
            }
        }
    }

    //rotate object
    void ObjectRotationUpdate(GameObject draggedObject)
    {
        Quaternion desiredRotation = draggedObject.transform.rotation;
        DetectTouchMovement.Calculate();
        if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
        { // rotate
            Vector3 rotationDeg = Vector3.zero;
            rotationDeg.z = DetectTouchMovement.turnAngleDelta;
            desiredRotation *= Quaternion.Euler(rotationDeg);
        }
        draggedObject.transform.rotation = desiredRotation;
    }

    private bool HasInput
    {
        get
        {
            // returns true if either the mouse button is down or at least one touch is felt on the screen
            return Input.GetMouseButton(0);
        }
    }

    //release item and set draggingItem = false;
    void DropItem()
    {
        draggingItem = false;
        draggedObject.transform.position = new Vector3(draggedObject.transform.position.x, draggedObject.transform.position.y, draggedObject.transform.position.z+5f);
        draggedObject.transform.localScale = new Vector3(1f, 1f,1f);
    }

}
