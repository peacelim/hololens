﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// The object cursor can switch between different game objects based on its state.
/// It simply links the game object to set to active with its associated cursor state.
/// </summary>
public class CustomObjectCursor : HoloToolkit.Unity.InputModule.Cursor {
    [Serializable]
    public struct ObjectCursorDatum {
        public string Name;
        public CursorStateEnum CursorState;
        public GameObject CursorObject;
    }

    [SerializeField]
    public ObjectCursorDatum[] CursorStateData;

    /// <summary>
    /// Sprite renderer to change.  If null find one in children
    /// </summary>
    public Transform ParentTransform;

    /// <summary>
    /// for feedbacks for rotation, translation and scaling
    /// </summary>
    public GameObject translationFeedbackObject;
    public GameObject rotationFeedbackObject;
    public GameObject scaleIndicator;

    /// <summary>
    /// On enable look for a sprite renderer on children
    /// </summary>
    protected override void OnEnable() {
        if (ParentTransform == null) {
            ParentTransform = transform;
        }

        for (int i = 0; i < ParentTransform.childCount; i++) {
            ParentTransform.GetChild(i).gameObject.SetActive(false);
        }

        base.OnEnable();
    }

    /// <summary>
    /// Override OnCursorState change to set the correct animation
    /// state for the cursor
    /// </summary>
    /// <param name="state"></param>
    public override void OnCursorStateChange(CursorStateEnum state) {
        base.OnCursorStateChange(state);
        if (state != CursorStateEnum.Contextual) {

            // First, try to find a cursor for the current state
            var newActive = new ObjectCursorDatum();
            for (int cursorIndex = 0; cursorIndex < CursorStateData.Length; cursorIndex++) {
                ObjectCursorDatum cursor = CursorStateData[cursorIndex];
                if (cursor.CursorState == state) {
                    newActive = cursor;
                    break;
                }
            }

            // If no cursor for current state is found, let the last active cursor be
            // (any cursor is better than an invisible cursor)
            if (newActive.Name == null) {
                return;
            }

            // If we come here, there is a cursor for the new state, 
            // so de-activate a possible earlier active cursor
            for (int cursorIndex = 0; cursorIndex < CursorStateData.Length; cursorIndex++) {
                ObjectCursorDatum cursor = CursorStateData[cursorIndex];
                if (cursor.CursorObject.activeSelf) {
                    cursor.CursorObject.SetActive(false);
                    break;
                }
            }
            newActive.CursorObject.SetActive(true);
        }
    }

    /// The below codes are for navigation and manipulation feedback
    /// To be called by objects that implement IFocusable

    public void ShowRotationFeedback() {
        if (rotationFeedbackObject == null || rotationFeedbackObject.activeSelf)
            return;
        rotationFeedbackObject.SetActive(true);
    }

    public void HideRotationFeedback() {
        if (rotationFeedbackObject == null || !rotationFeedbackObject.activeSelf)
            return;
        rotationFeedbackObject.SetActive(false);
    }

    public void ShowTranslationFeedback() {
        if (translationFeedbackObject == null || translationFeedbackObject.activeSelf)
            return;
        translationFeedbackObject.SetActive(true);
    }

    public void HideTranslationFeedback() {
        if(translationFeedbackObject == null || !translationFeedbackObject.activeSelf)
            return;
        translationFeedbackObject.SetActive(false);
    }

    public void ShowScalingFeedback() {
        if (rotationFeedbackObject == null || rotationFeedbackObject.activeSelf)
            return;
        // make the rotation feedback vertical
        rotationFeedbackObject.transform.Rotate(new Vector3(0, 0, 90));
        rotationFeedbackObject.transform.localScale += new Vector3(1, 1, 1);
        SurfaceCursorDistance += 0.4f;
        rotationFeedbackObject.SetActive(true);
        scaleIndicator.SetActive(true);
    }

    public void HideScalingFeedback() {
        if (rotationFeedbackObject == null || !rotationFeedbackObject.activeSelf)
            return;
        //put everything back to before state
        rotationFeedbackObject.transform.Rotate(new Vector3(0, 0, -90));
        rotationFeedbackObject.transform.localScale += new Vector3(-1, -1, -1);
        SurfaceCursorDistance -= 0.4f;
        rotationFeedbackObject.SetActive(false);
        scaleIndicator.SetActive(false);
    }
}
