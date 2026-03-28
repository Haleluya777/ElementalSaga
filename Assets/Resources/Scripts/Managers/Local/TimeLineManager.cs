using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public void TimeLinePlay()
    {
        director.Play();
    }

    public void TimeLinePause()
    {
        director.Pause();
    }
}
