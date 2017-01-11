﻿using System.Collections.Generic;
using System.Net;

namespace PlayFab.Editor
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;

    public class EditorCoroutine
    {
        public string Id;
        public class EditorWaitForSeconds : YieldInstruction
        {
            public float Seconds;

            /// <summary>
            /// 
            /// <para>
            /// Creates a yield instruction to wait for a given number of seconds using scaled time.
            /// </para>
            /// 
            /// </summary>
            /// <param name="seconds"/>
            public EditorWaitForSeconds(float seconds)
            {
                this.Seconds = seconds;
            }
        }

        private SortedList<float, IEnumerator> shouldRunAfterTimes = new SortedList<float, IEnumerator>();
        private const float _tick = .02f;

        public static EditorCoroutine start(IEnumerator _routine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_routine);
            coroutine.Id = System.Guid.NewGuid().ToString();
            coroutine.start();
            return coroutine;
        }

        public static EditorCoroutine start(IEnumerator _routine, WWW www)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_routine);
            coroutine.Id = System.Guid.NewGuid().ToString();
            coroutine._www = www;
            coroutine.start();
            return coroutine;
        }


        readonly IEnumerator routine;
        private WWW _www;

        EditorCoroutine(IEnumerator _routine)
        {
            routine = _routine;
        }

        void start()
        {
            EditorApplication.update += update;
        }
        public void stop()
        {
            EditorApplication.update -= update;
        }

        private float _timeCounter = 0;
        void update()
        {
            _timeCounter += _tick;
            //Debug.LogFormat("ID:{0}  TimeCounter:{1}", this.Id, _timeCounter);

            try
            {
                if (_www != null)
                {
                    if (_www.isDone && !routine.MoveNext())
                    {
                        stop();
                    }
                }
                else       
                {
                    var seconds = routine.Current as EditorWaitForSeconds;
                    if (seconds != null)
                    {
                        var wait = seconds;
                        shouldRunAfterTimes.Add(_timeCounter + wait.Seconds, routine);
                    }
                    else if (!routine.MoveNext())
                    {
                        stop();
                    }
                }

                var shouldRun = shouldRunAfterTimes;
                var index = 0;
                foreach (var runAfterSeconds in shouldRun)
                {
                    if (_timeCounter >= runAfterSeconds.Key)
                    {
                        //Debug.LogFormat("RunAfterSeconds: {0} >= {1}", runAfterSeconds.Key, _timeCounter);
                        shouldRunAfterTimes.RemoveAt(index);
                        if (!runAfterSeconds.Value.MoveNext())
                        {
                            stop();
                        }
                    }
                    index++;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.StackTrace);
            }
        }
    }
}
