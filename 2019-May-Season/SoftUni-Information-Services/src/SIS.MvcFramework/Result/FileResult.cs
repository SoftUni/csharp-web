﻿using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    public class FileResult : ActionResult
    {
        public FileResult(byte[] fileContent, string fileName, HttpResponseStatusCode httpResponseStatusCode = HttpResponseStatusCode.Ok) : base(httpResponseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength, fileContent.Length.ToString()));
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, $"attachment; filename=\"{fileName}\""));
            this.Content = fileContent;
        }
    }
}
