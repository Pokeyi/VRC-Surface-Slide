// Copyright © 2022 Pokeyi - https://pokeyi.dev - pokeyi@pm.me - This work is licensed under the MIT License.

// using System;
using UdonSharp;
using UnityEngine;
// using UnityEngine.UI;
// using VRC.SDKBase;
// using VRC.SDK3.Components;
// using VRC.Udon.Common.Interfaces;

namespace Pokeyi.UdonSharp
{
    [AddComponentMenu("Pokeyi.VRChat/P.VRC Surface Slide")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)] // No networking.

    public class P_SurfaceSlide : UdonSharpBehaviour
    {   // Surface sliding object controller for VRChat:
        [Header(":: VRC Surface Slide by Pokeyi ::")]
        [Space]
        [Tooltip("Sliding game objects.")]
        [SerializeField] private GameObject[] slidingObjects;
        [Tooltip("Slide distance limit.")]
        [SerializeField] private float distanceLimit = 1F;
        [Tooltip("Slide power multiplier.")]
        [SerializeField] [Range(0F, 1F)] private float surfaceSmoothness = 0.25F;

        private const float DELTA_DIST = 0.001F; // Baseline per-frame delta distance.
        private Vector3[] startPosLocal; // Original local start position of sliding objects.
        private bool hasStarted = false;

        public void Start()
        {   // Record local start position for all sliding objects.
            if (slidingObjects == null) return;
            startPosLocal = new Vector3[slidingObjects.Length];
            for (int i = 0; i < slidingObjects.Length; i++) if (slidingObjects[i] != null)
                {
                    startPosLocal[i] = slidingObjects[i].transform.localPosition;
                }
            hasStarted = true;
        }

        public void OnEnable()
        {
            if (!hasStarted) return;
            for (int i = 0; i < slidingObjects.Length; i++) if (slidingObjects[i] != null) slidingObjects[i].transform.localPosition = startPosLocal[i];
        }

        public void LateUpdate()
        {   // 
            for (int i = 0; i < slidingObjects.Length; i++) if ((slidingObjects[i] != null) && (slidingObjects[i].activeSelf) && (slidingObjects[i].transform.parent != null))
                {
                    Vector3 currentPos = slidingObjects[i].transform.localPosition;
                    Vector3 parentEuler = slidingObjects[i].transform.parent.rotation.eulerAngles;
                    float parentX = parentEuler.x;
                    float parentZ = parentEuler.z;
                    if (parentX >= 180F) parentX -= 360F;
                    if (parentZ >= 180F) parentZ -= 360F;
                    Vector3 direction = new Vector3(-parentZ, 0F, parentX);
                    float magnitude = Mathf.Max(Mathf.Abs(parentX), Mathf.Abs(parentZ));
                    Vector3 newPos = Vector3.MoveTowards(currentPos, currentPos + direction, DELTA_DIST * (surfaceSmoothness * magnitude));
                    if (Vector3.Distance(startPosLocal[i], newPos) <= distanceLimit) slidingObjects[i].transform.localPosition = newPos;
                }
        }
    }
}

/* MIT License

Copyright (c) 2022 Pokeyi - https://pokeyi.dev - pokeyi@pm.me

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */