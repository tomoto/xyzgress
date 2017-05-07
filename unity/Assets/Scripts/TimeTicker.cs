using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TimeTicker
{
    private float interval;
    private float timeout;

    public TimeTicker(float interval)
    {
        this.interval = interval;
        timeout = 0;
    }

    public bool IsTimeout()
    {
        return IsTimeout(Time.time);
    }

    public bool IsTimeout(float currentTime)
    {
        return currentTime >= timeout;
    }

    public TimeTicker Reset()
    {
        timeout = 0;
        return this;
    }

    public TimeTicker Start()
    {
        return Start(Time.time);
    }

    public TimeTicker Start(float currentTime)
    {
        timeout = currentTime + interval;
        return this;
    }
}
