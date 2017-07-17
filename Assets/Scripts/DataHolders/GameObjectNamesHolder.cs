﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectNamesHolder {
    public static string NAME_MAP_PARENT = "MapboxParent";
    // contains
    public static string NAME_MAP_COLLIDER = "ScalableMapObjects";
    // contains
    public static string NAME_MAPBOX = "Mapbox";

    public static string NAME_TOOL_BAR = "Toolbar";
    public static string NAME_MAP_DATA_HOLDER = "DataHolder";
    public static string NAME_CURSOR = "CustomCursorWithFeedback";

    // within data holder
    public static string NAME_DATA_MAP_ZOOM = "Zoom";
    public static string NAME_DATA_MAP_THEME = "Theme";
    public static string NAME_DATA_MAP_LATITUDE_LONGITUDE = "CenterCoordinates";
    public static string NAME_DATA_MAP_SCALE = "WorldRelativeScale";

    // physics layers
    public static int LAYER_MAP_OBJECTS = LayerMask.NameToLayer("MapObjects");
    public static int LAYER_VISIBLE_TILES = LayerMask.NameToLayer("VisibleTiles");
    public static int LAYER_INVISIBLE_TILES = LayerMask.NameToLayer("InvisibleTiles");

}
