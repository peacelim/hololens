﻿using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using UnityEngine.UI;

/// <summary>
/// This class handles the drawing of polygons.
/// </summary>
public class DrawingManager : Singleton<DrawingManager>, IInputClickHandler {
    [SerializeField]
    private CustomObjectCursor cursor;

    // using this object's position is more accurate than using GazeManager.hitPosition
    [SerializeField]
    private Transform drawingPointOnCursorTransform;

    /// <summary>
    /// stores the positions of instantiated spheres
    /// </summary>
    private List<Vector3> polygonVertices;

    /// <summary>
    /// transform parent of the instantiated spheres. A simple new gameobject that parents all the point objects in order to make it easier to destroy them.
    /// </summary>
    private GameObject pointsContainerObject;

    /// <summary>
    /// message to the user that shows up when the polygon can be enclosed with a click
    /// </summary>
    [SerializeField]
    private GameObject EncloseGuidePrefab;
    private GameObject guideObjectInstance;
    private float guidePositionAbovePoint = 0.05f;

    public bool CanPolygonBeEnclosedAndCursorOnFirstPoint {
        get; set;
    }

    [SerializeField]
    private GameObject SpherePrefab;
    private bool isAnyPointDrawnYet; // whether or not to call the update function to render line
    private LineRenderer currentlyDrawnLine;

    private GameObject lastDrawnPoint;

    private float lineWidth = 0.002f;

    private void Start() {
        guideObjectInstance = Instantiate(EncloseGuidePrefab);
        guideObjectInstance.GetComponentInChildren<Text>().text = "Click to instantiate this polygon";
        guideObjectInstance.SetActive(false); // just pool it
    }

    public void StartDrawing() {
        GuideStatus.ShouldShowGuide = false;
        isAnyPointDrawnYet = false;
        polygonVertices = new List<Vector3>();
        changeCursorToDrawingPoint();
        InputManager.Instance.PushModalInputHandler(gameObject);
        CanPolygonBeEnclosedAndCursorOnFirstPoint = false;
        InteractibleMap.Instance.HideAllTables();
    }

    public void StopDrawing() {
        InputManager.Instance.PopModalInputHandler();
        clearPoints();
        changeCursorBack();
        isAnyPointDrawnYet = false;
        CanPolygonBeEnclosedAndCursorOnFirstPoint = false;
        GuideStatus.ShouldShowGuide = true;
        GlobalVoiceCommands.Instance.exitDrawingMode();
    }

    /// <summary>
    /// used to render the lines. Is there any other use?
    /// </summary>
	void Update () {
        if (!isAnyPointDrawnYet || CanPolygonBeEnclosedAndCursorOnFirstPoint)
            return;
        UpdateLineDrawing();
	}

    /// <summary>
    /// Should be called from OnTriggerEnter/OnFocusEnter by FirstDrawnPoint script.
    /// </summary>
    public bool CheckCanPolygonBeEnclosed() {
        if (polygonVertices.Count < 3)
            return false;
        return true;
    }

    private void UpdateLineDrawing() {
        currentlyDrawnLine.SetPosition(1, drawingPointOnCursorTransform.position);
    }

    private void changeCursorToDrawingPoint() {
        cursor.EnterDrawingMode();
    }

    private void changeCursorBack() {
        cursor.ExitDrawingMode();
    }

    /// <summary>
    /// the argument position should be the actual position that requires no
    /// further calculations or processing.
    /// </summary>
    /// <param name="position"></param>
    private void createSphereAt(Vector3 position) {
        GameObject sphere = Instantiate(SpherePrefab, position, Quaternion.identity);
        polygonVertices.Add(sphere.transform.position);
        lastDrawnPoint = sphere;
        if (!isAnyPointDrawnYet) {
            // if this sphere is the first sphere to be drawn, set up a empty parent
            // that contains all the sphere objects so that they become easier to delete
            pointsContainerObject = new GameObject("PointsContainer");
            setUpFirstSphere(sphere);
        }

        if (isAnyPointDrawnYet)
            currentlyDrawnLine.SetPosition(1, position);
        instantiateNewLine();
        sphere.transform.parent = pointsContainerObject.transform;
    }

    private void setUpFirstSphere(GameObject firstSphere) {
        Vector3 originalPosition = firstSphere.transform.position;
        firstSphere.AddComponent<FirstDrawnPoint>();
        // fix its position as it bounces off the map collider for some reason
        firstSphere.transform.position = originalPosition;
    }

    /// <summary>
    /// args should take in pointsDrawn
    /// </summary>
    private void instantiatePolygon() {
        PolygonManager.Instance.InstantiatePolygonFromVertices(polygonVertices);
        StopDrawing();
    }

    private void clearPoints() {
        Destroy(pointsContainerObject);
        polygonVertices.Clear();
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        if (CanPolygonBeEnclosedAndCursorOnFirstPoint) {
            instantiatePolygon();
            clearPoints();
            isAnyPointDrawnYet = false;
            CanPolygonBeEnclosedAndCursorOnFirstPoint = false;
        } else if (GazeManager.Instance.HitObject.name.Equals(GameObjectNamesHolder.NAME_MAP_COLLIDER)){
            Vector3 drawPointPosition = drawingPointOnCursorTransform.position;
            createSphereAt(drawPointPosition);
            isAnyPointDrawnYet = true;
        } else {
            // do nothing
        }

    }

    private void instantiateNewLine() {
        currentlyDrawnLine = lastDrawnPoint.GetComponent<LineRenderer>();
        currentlyDrawnLine.positionCount = 2;
        currentlyDrawnLine.SetPosition(0, lastDrawnPoint.transform.position);
        currentlyDrawnLine.SetPosition(1, drawingPointOnCursorTransform.position);
        currentlyDrawnLine.endWidth = 0.001f;
        currentlyDrawnLine.startWidth = 0.001f;
    }

    public void FixLineEndAtFirstSphere() {
        currentlyDrawnLine.SetPosition(1, polygonVertices[0]);
    }

    /// <summary>
    /// This displays the guide which notifies the user that the polygon can now be enclosed with a tap gesture.
    /// </summary>
    public void DisplayGuide(Vector3 firstSpherePosition) {
        guideObjectInstance.transform.position = firstSpherePosition + new Vector3(0, guidePositionAbovePoint, 0);
        guideObjectInstance.transform.LookAt(Camera.main.transform.position, Vector3.up);
        guideObjectInstance.SetActive(true);
    }

    public void HideGuide() {
        if (guideObjectInstance.activeSelf)
            guideObjectInstance.SetActive(false); // just hide it
    }

    public void ForceCursorStateChange() {
        // after all we need the map to tell the cursor state change between the map and 
        // other objects, which cannot be detected using the default cursor state
        cursor.OnCursorStateChange(HoloToolkit.Unity.InputModule.Cursor.CursorStateEnum.Observe);
    }

}
