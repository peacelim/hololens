﻿// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections.Generic;

public class CSVTable: MonoBehaviour {
    public TextAsset csvFile;

    public class Row {
        public string OBJECTID;
        public string building_name;
        public string measured_height;
        public string building_class;
        public string storeys_above_ground;
        public string gameobject_name;
        public string latitude;
        public string longitude;
	}

    List<Row> rowList = new List<Row>();
    bool isLoaded = false;

    public bool IsLoaded() {
        return isLoaded;
    }

    public List<Row> GetRowList() {
        return rowList;
    }

    public void Load(TextAsset csv) {
        rowList.Clear();
        string[][] grid = CsvParser2.Parse(csv.text);
        for (int i = 1; i < grid.Length; i++) {
            Row row = new Row();
            row.OBJECTID = grid[i][0];
            row.building_name = grid[i][1];
            row.measured_height = grid[i][2];
            row.building_class = grid[i][3];
            row.storeys_above_ground = grid[i][4];
            row.gameobject_name = grid[i][5];
            row.latitude = grid[i][6];
            row.longitude = grid[i][7];

            rowList.Add(row);
        }
        isLoaded = true;
    }

    public int NumRows() {
        return rowList.Count;
    }

    public Row GetAt(int i) {
        if (rowList.Count <= i)
            return null;
        return rowList[i];
    }

    public Row Find_OBJECTID(string find) {
        return rowList.Find(x => x.OBJECTID == find);
    }
    public List<Row> FindAll_OBJECTID(string find) {
        return rowList.FindAll(x => x.OBJECTID == find);
    }
    public Row Find_building_name(string find) {
        return rowList.Find(x => x.building_name == find);
    }
    public List<Row> FindAll_building_name(string find) {
        return rowList.FindAll(x => x.building_name == find);
    }
    public Row Find_measured_height(string find) {
        return rowList.Find(x => x.measured_height == find);
    }
    public List<Row> FindAll_measured_height(string find) {
        return rowList.FindAll(x => x.measured_height == find);
    }
    public Row Find_building_class(string find) {
        return rowList.Find(x => x.building_class == find);
    }
    public List<Row> FindAll_building_class(string find) {
        return rowList.FindAll(x => x.building_class == find);
    }
    public Row Find_storeys_above_ground(string find) {
        return rowList.Find(x => x.storeys_above_ground == find);
    }
    public List<Row> FindAll_storeys_above_ground(string find) {
        return rowList.FindAll(x => x.storeys_above_ground == find);
    }
    public Row Find_gameobject_name(string find) {
        return rowList.Find(x => x.gameobject_name == find);
    }
    public List<Row> FindAll_gameobject_name(string find) {
        return rowList.FindAll(x => x.gameobject_name == find);
    }
    public Row Find_lat(string find) {
        return rowList.Find(x => x.latitude == find);
    }
    public List<Row> FindAll_lat(string find) {
        return rowList.FindAll(x => x.latitude == find);
    }
    public Row Find_long(string find) {
        return rowList.Find(x => x.longitude == find);
    }
    public List<Row> FindAll_long(string find) {
        return rowList.FindAll(x => x.longitude == find);
    }

}