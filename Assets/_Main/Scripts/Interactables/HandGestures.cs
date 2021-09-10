using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DreamHouseStudios.VR
{
    //[CanEditMultipleObjects]
    public class HandGestures : MonoBehaviour
    {
        public bool isLeft;

        public List<Transform> thumb, index, middle, pinky, ring;
        public float thumbRot, indexRot, middleRot, pinkyRot, ringRot;

        public float thumbDot, indexDot, middleDot, pinkyDot, ringDot;

        public float grabTrigger;

        public bool isGrabbing, isPointing, isRelease;

        public GestureEvent onGrab, onRelease, onPoint, onPointRelease;

        private void Start()
        {
            SetBones();
        }

        private bool ind;
        private bool mid;
        private bool pin;
        private bool rin;

        public bool rotByFloat;
        public float rot;

        private void Update()
        {
            if (rotByFloat)
            {
                //foreach (Transform f in thumb)
                //    f.localEulerAngles = new Vector3(0f, 0f, rot);

                foreach (Transform f in index)
                    f.localEulerAngles = new Vector3(0f, 0f, rot);

                foreach (Transform f in middle)
                    f.localEulerAngles = new Vector3(0f, 0f, rot);

                foreach (Transform f in pinky)
                    f.localEulerAngles = new Vector3(0f, 0f, rot);

                foreach (Transform f in ring)
                    f.localEulerAngles = new Vector3(0f, 0f, rot);
            }

            //thumbDot = Vector3.Dot(thumb[0].right * -1f, thumb[1].right * -1f);

            //thumbDot = Vector3.Dot(thumb[1].right * -1f, thumb[2].right * -1f);
            //indexDot = Vector3.Dot(index[0].right * -1f, index[index.Count - 2].right * -1f);
            //middleDot = Vector3.Dot(middle[0].right * -1f, middle[middle.Count - 2].right * -1f);
            //pinkyDot = Vector3.Dot(pinky[0].right * -1f, pinky[pinky.Count - 2].right * -1f);
            //ringDot = Vector3.Dot(ring[0].right * -1f, ring[ring.Count - 2].right * -1f);

            thumbDot = Vector3.Dot(thumb[1].right.normalized * -1f, thumb[2].right.normalized * -1f);
            indexDot = Vector3.Dot(index[0].right.normalized * -1f, index[index.Count - 2].right.normalized * -1f);
            middleDot = Vector3.Dot(middle[0].right.normalized * -1f, middle[middle.Count - 2].right.normalized * -1f);
            pinkyDot = Vector3.Dot(pinky[0].right.normalized * -1f, pinky[pinky.Count - 2].right.normalized * -1f);
            ringDot = Vector3.Dot(ring[0].right.normalized * -1f, ring[ring.Count - 2].right.normalized * -1f);

            #region GRAB/RELEASE EVENTS

            if (ind && indexDot >= .1f)
            {
                ind = false;
            }
            else if (!ind && indexDot < -.2f)
            {
                ind = true;
            }

            if (mid && middleDot >= .1f)
            {
                mid = false;
            }
            else if (!mid && middleDot < -.2f)
            {
                mid = true;
            }

            if (pin && pinkyDot >= .1f)
            {
                pin = false;
            }
            else if (!pin && pinkyDot < -.2f)
            {
                pin = true;
            }

            if (rin && ringDot >= .1f)
            {
                rin = false;
            }
            else if (!rin && ringDot < -.2f)
            {
                rin = true;
            }

            // if(!isGrabbing &&ind && mid && pin && rin && !isGrabbing)
            if (Input.GetKeyDown(KeyCode.Alpha1))
                onPoint.Invoke(this);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                onPointRelease.Invoke(this);

            if (!isPointing && indexDot > .95f && mid && rin)
            {
                isPointing = true;
                isRelease = false;
                onPoint.Invoke(this);
            }
            else if ((isPointing && indexDot < .9f) || (!mid || !rin))
            {
                isPointing = false;
                onPointRelease.Invoke(this);
            }

            if (!isGrabbing && ind && mid && rin)
            {
                isGrabbing = true;
                isRelease = false;
                onGrab.Invoke(this);
            }
            else if ((!ind || !mid || !rin) && isGrabbing)
            {
                // if (isGrabbing)
                {
                    isGrabbing = false;
                    isRelease = true;
                    onRelease.Invoke(this);
                }
            }
            // if (!ind || !mid || !pin || !rin)
            // {
            //     isGrabbing = false;
            //     onRelease.Invoke(this);
            // }
            // if (ind && mid && pin && rin && !isGrabbing)
            // {
            //     isGrabbing = true;
            //     onGrab.Invoke(this);
            // }
            // else if ((!ind || !mid || !pin || !rin) && isGrabbing)
            // {
            //     isGrabbing = false;
            //     onRelease.Invoke(this);
            // }
            /*
                        if(!ind || !mid || !pin || !rin)
                        {
                            isGrabbing = false;
                            onRelease.Invoke(this);
                            isPointing = false;
                            onPointRelease.Invoke(this);
                        }
                        if (ind && mid && pin && rin && !isGrabbing)
                        {
                            isGrabbing = true;
                            onGrab.Invoke(this);
                            isPointing = false;
                            onPointRelease.Invoke(this);
                        }
                        else if ((!ind || !mid || !pin || !rin) && isGrabbing)
                        {
                            isGrabbing = false;
                            onRelease.Invoke(this);
                        }

                        if (!isGrabbing && !isPointing && !ind && mid && pin && rin &&indexDot >.95f)
                        {
                            isPointing = true;
                            onPoint.Invoke(this);
                        }
                        else if (isPointing && (ind || !mid || !pin || !rin) && indexDot < .95f)
                        {
                            isPointing = false;
                            onPointRelease.Invoke(this);
                        }
            */
            // if(isPointing)
            // {
            //     onPoint.Invoke(this);
            // }
            // else
            // {
            //     onPointRelease.Invoke(this);
            // }

            // if(isGrabbing)
            // {
            //     onGrab.Invoke(this);
            // }
            // else
            // {
            //     onRelease.Invoke(this);
            // }

            #endregion GRAB/RELEASE EVENTS
        }

        [ContextMenu("Set bones")]
        public void SetBones()
        {
            List<Transform> tl = new List<Transform>();
            tl.AddRange(transform.GetComponentsInChildren<Transform>());

            //thumb = tl.Find(i => i.name == "Human_" + (isLeft ? "Left" : "Right") + "HandThumb4");
            //index = tl.Find(i => i.name == "Human_" + (isLeft ? "Left" : "Right") + "HandIndex4");
            //middle = tl.Find(i => i.name == "Human_" + (isLeft ? "Left" : "Right") + "HandMiddle4");
            //pinky = tl.Find(i => i.name == "Human_" + (isLeft ? "Left" : "Right") + "HandPinky4");
            //ring = tl.Find(i => i.name == "Human_" + (isLeft ? "Left" : "Right") + "HandRing4");

            thumb.Clear();
            index.Clear();
            middle.Clear();
            pinky.Clear();
            ring.Clear();

            for (int i = 1; i < 5; i++)
                //if (i == 1 || i == 4)
                thumb.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandThumb" + i.ToString()));

            for (int i = 1; i < 5; i++)
                //if (i == 1 || i == 4)
                index.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandIndex" + i.ToString()));

            for (int i = 1; i < 5; i++)
                //if (i == 1 || i == 4)
                middle.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandMiddle" + i.ToString()));

            for (int i = 1; i < 5; i++)
                //if (i == 1 || i == 4)
                pinky.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandPinky" + i.ToString()));

            for (int i = 1; i < 5; i++)
                //if (i == 1 || i == 4)
                ring.Add(tl.Find(I => I.name == "Human_" + (isLeft ? "Left" : "Right") + "HandRing" + i.ToString()));
        }

        [System.Serializable]
        public class GestureEvent : UnityEvent<HandGestures> { }
    }
}