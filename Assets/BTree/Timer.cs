using System;
using UnityEngine;

namespace BTree
{
	public class Timer
		{
		private readonly bool _resetOnAlarm;
		private float _alarmTime;
		private bool _alarmRaised;
		
		/// <summary>
		/// Delegate that will be called when the timer reaches the alarm time.
		/// </summary>
		public event Action Alarm;

		public float TimeElapsed { get; set; }

		public float AlarmTime => _alarmTime;

		/// <summary>
		/// Creates a timer. By default starts automatically and resets when Alarm raised.
		/// </summary>
		/// <param name="time">The time when the Alarm is raised.</param>
		/// <param name="autoReset">True resets when Alarmed.</param>
		/// <param name="initialTime">Starting time in the timer, default 0.</param>
		public Timer(float time, bool autoReset = true, float initialTime = 0)
		{
			if (time < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			_alarmTime = time;
			_resetOnAlarm = autoReset;
			TimeElapsed = initialTime;
		}


		/// <summary>
		/// Does not run automatically each frame. Must be called from a MonoBehavior Update().
		/// </summary>
		public void Update()
		{
			TimeElapsed += Time.deltaTime;

			if (TimeElapsed >= _alarmTime && !_alarmRaised)
			{
				Alarm?.Invoke();
                _alarmRaised = true;

                if (_resetOnAlarm)
                {
                    Reset();
                }               
            }
		}

		
		/// <summary>
		/// Resets the timer. 
		/// </summary>
		public void Reset()
		{
			TimeElapsed = 0;
			_alarmRaised = false;
		}

		/// <summary>
		/// Sets the time when the alarm is raised.
		/// </summary>
		/// <param name="time"></param>
		public void SetAlarm(float time)
		{
			_alarmTime = time;
		}
	}
}