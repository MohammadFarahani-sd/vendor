using Framework.Web.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Framework.Web.Api.Controllers;

public class BaseApiController : ControllerBase
{
    protected string GetNextPageUrl(PaginationRequest paginationRequest, long totalCount)
    {
        if (paginationRequest.Offset + paginationRequest.Count >= totalCount)
        {
            return string.Empty;
        }

        var requestQuery = Request.Query;

        var requestQueryWithoutOffset = requestQuery.Where(q => q.Key != nameof(PaginationRequest.Offset)).ToList();

        var nextPageOffset = paginationRequest.Offset + paginationRequest.Count;
        var offerQueryParam = new KeyValuePair<string, StringValues>(nameof(PaginationRequest.Offset), nextPageOffset.ToString());

        requestQueryWithoutOffset.Add(offerQueryParam);

        var queryString = new QueryString();
        requestQueryWithoutOffset.ForEach(q => queryString = queryString.Add(q.Key, q.Value));

        var url = Request.Path + queryString.ToString();
        return url;
    }
}