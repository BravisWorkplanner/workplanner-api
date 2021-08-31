using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIEndpoints
{
    /// <summary>
    /// A base class for all asynchronous endpoints.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType((int) HttpStatusCode.UnsupportedMediaType)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    public abstract class BaseEndpointAsync : ControllerBase
    {
    }
}