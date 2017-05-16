using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DevOpsApi.Controllers
{
#if RELEASE
    [Authorize(Roles = "Engineers")]
#endif
    public class NoteApiController : ApiController
    {
        BusinessLayer.ManageNotes noteProcessor = new BusinessLayer.ManageNotes();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (An Action that handles HTTP GET requests) gets. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="id">       The Identifier to delete. </param>
        /// <param name="noteType"> Type of the note. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        [HttpGet]
        public HttpResponseMessage Get(int id, string noteType)
        {
            try
            {
                return Request.CreateResponse<List<ViewModel.Note>>(HttpStatusCode.OK, noteProcessor.GetNote(id, noteType));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Puts the given note. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="note"> The note to put. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        [HttpPut]
        public HttpResponseMessage Put(ViewModel.Note note)
        {
            try
            {
                return Post(note);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (An Action that handles HTTP POST requests) post this message. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
        [HttpPost]
        public HttpResponseMessage Post(ViewModel.Note note)
        {
            try
            {
                var response = Request.CreateResponse<ViewModel.Note>(HttpStatusCode.OK, noteProcessor.UpdateNote(note));
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the given ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="id">   The Identifier to delete. </param>
        ///
        /// <returns>   A HttpResponseMessage. </returns>
        ///-------------------------------------------------------------------------------------------------
#if RELEASE
    [Authorize(Roles = "DevOps")]
#endif
        [HttpDelete]
        public HttpResponseMessage Delete(int id, string noteType = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(noteType))
                    return Request.CreateResponse<ViewModel.Note>(HttpStatusCode.OK, noteProcessor.DeleteNoteByType(id, noteType));
                else
                    return Request.CreateResponse<ViewModel.Note>(HttpStatusCode.OK, noteProcessor.DeleteNoteById(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<Exception>(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
