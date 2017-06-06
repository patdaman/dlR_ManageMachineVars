﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModel;

namespace DevOpsApi.Controllers
{
    public class BoardApiController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var repo = new BoardRepository();
            var response = Request.CreateResponse();

            response.Content = new StringContent(JsonConvert.SerializeObject(repo.GetColumns()));
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        [HttpGet]
        public HttpResponseMessage CanMove(int sourceColId, int targetColId)
        {
            var response = Request.CreateResponse();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent(JsonConvert.SerializeObject(new { canMove = false }));

            if (sourceColId == (targetColId - 1))
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new { canMove = true }));
            }

            return response;
        }

        [HttpPost]
        public HttpResponseMessage MoveTask(JObject moveTaskParams)
        {
            dynamic json = moveTaskParams;
            var repo = new BoardRepository();
            repo.MoveTask((int)json.taskId, (int)json.targetColId);

            var response = Request.CreateResponse();
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }
    }
}

