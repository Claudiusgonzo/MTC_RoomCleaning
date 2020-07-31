﻿using Newtonsoft.Json;
using RoomCleaning.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RoomCleaning.AdminUI.Services
{
    public class RoomPolicyService
    {
        private readonly HttpClient _client;

        public string BaseUri => (_client.BaseAddress == null).ToString();

        public RoomPolicyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<RoomDetail[]> GetRoomsAsync()
        {
                    
            try
            {
                var result = await _client.GetAsync("/api/rooms");
                var json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RoomDetail[]>(json);
            }
            catch (Exception ex)
            {

            }

            return new RoomDetail[0];
        }

        public async Task<bool> SendPolicyRequest(RoomPolicyRequest request)
        {
            if (request is null)
                throw new ArgumentNullException($"{nameof(request)} cannot be null");

            if (request.Policy is null || request.Rooms == null)
                throw new ArgumentNullException($"{nameof(request.Rooms)} and/or{nameof(request.Policy)} cannot be null");

            var json = JsonConvert.SerializeObject(request);
            var result = await _client.PostAsync("/api/subscriptions", new StringContent(json));
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<RoomPolicy[]> GetRoomPoliciesAsync()
        {
            var policies = new List<RoomPolicy>();
            for (int i = 0; i < 3; i++)
            {
                var policy = new RoomPolicy() { CleaningPolicy = new CleaningPolicy(), Room = new RoomDetail { Id = $"{i}", DisplayName = $"{i} Room" } };
                policies.Add(policy);
            }

            return policies.ToArray();
        }
    }
}
