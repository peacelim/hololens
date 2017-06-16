﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

/// <summary>
/// attach to the map so that this is available only when the map is gazed at
/// </summary>
public class StreetViewVoiceListener : MonoBehaviour, ISpeechHandler {
    public const string COMMAND_STREET_VIEW = "street view";

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData) {
        switch (eventData.RecognizedText.ToLower()) {
            case COMMAND_STREET_VIEW:
                setUpStreetView();
                break;

            default:
                //ignore
                break;
        }
    }

    private void setUpStreetView() {
        StreetView.Instance.SetUpStreetView();
    }

}