using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Component
{
    public class SessionConnectionController : MonoBehaviour
    {
        private ISession _session;
        private IClient _client;
        private bool _sessionConnectionStart;
        private long _targetUnixTimestamp; // Unix timestamp in seconds
        private int _durationInSeconds;
        private bool _isFirstTime; // Duration in seconds
        private bool _handelRefreshSession;

        public void Init(HClient client, HSession session, bool sessionConnectionStart, int durationInSeconds = 2,
            bool handelRefreshSession = false)
        {
            _client = client.client;
            _session = session.Session;
            _sessionConnectionStart = sessionConnectionStart;
            _durationInSeconds = durationInSeconds;
            _targetUnixTimestamp = _session.ExpireTime;
            long currentUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            DateTimeOffset clientTime = DateTimeOffset.FromUnixTimeSeconds(currentUnixTimestamp);
            DateTimeOffset serverTime = DateTimeOffset.FromUnixTimeSeconds(_targetUnixTimestamp);
            long difference = serverTime.ToUnixTimeSeconds() - clientTime.ToUnixTimeSeconds();
            _handelRefreshSession = handelRefreshSession;
            if (_handelRefreshSession)
            {
                InvokeRepeating(nameof(refresh_Session_Repeatedly), 0.0f, difference);
            }
            //long secondsElapsed = currentUnixTimestamp - _targetUnixTimestamp;
        }

        public void StartScheduleExpireTime(bool sessionConnectionStart)
        {
            _sessionConnectionStart = sessionConnectionStart;
        }

        private async void refresh_Session_Repeatedly()
        {

            if (_isFirstTime)
            {
                await RefreshSession(_client);
            }

            _isFirstTime = true;

        }

        private async UniTask<ISession> RefreshSession(IClient client)
        {

            _session = await client.SessionRefreshAsync(_session);
            _targetUnixTimestamp = _session.ExpireTime;
            return _session;
        }

        public async UniTask<bool> LogoutSession(IClient client)
        {

            await client.SessionLogoutAsync(_session);
            return true;
        }
        // private async void Update()
        // {
        //     if (!_sessionConnectionStart)
        //         return;
        //
        //     long currentUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        //     long secondsElapsed = currentUnixTimestamp - _targetUnixTimestamp;
        //     if (secondsElapsed >= _durationInSeconds)
        //     {
        //         // The duration has elapsed
        //         //Debug.Log("The duration has elapsed.");
        //         await RefreshSession(_client);
        //
        //     }
        //     else
        //     {
        //         // The duration has not elapsed yet
        //         //Debug.Log($"The duration has not elapsed yet. {_durationInSeconds - secondsElapsed} seconds remaining.");
        //     }
        //     await UniTask.Yield();
        // }
    }
}