using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WxCore.Helper;
using WxCore.Models;

namespace WxCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WchartController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Check()
        {
            string signature = HttpContext.Request.Query["signature"].FirstOrDefault()?.Trim()?.ToString();
            string timestamp = HttpContext.Request.Query["timestamp"].FirstOrDefault()?.Trim()?.ToString();
            string token = HttpContext.Request.Query["token"].FirstOrDefault()?.Trim()?.ToString();
            string nonce = HttpContext.Request.Query["nonce"].FirstOrDefault()?.Trim()?.ToString();
            string echostr = HttpContext.Request.Query["echostr"].FirstOrDefault()?.ToString();
            string[] ArrTmp = { token, timestamp, nonce };
            string tmpStr = string.Join("", ArrTmp);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var bytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(tmpStr));
            var ostr = System.Text.Encoding.UTF8.GetString(bytes);
            ostr = ostr.ToLower();
            if (ostr == signature)
            {
                return echostr;
            }
            else
            {
                return "no";
            }
        }
        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetAccessToken()
        {
            string url = $"{WchartApiConst.access_token}?grant_type=client_credential&appid={WchartApiConst.appid}&secret={WchartApiConst.appsecret}";
            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "GET";
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            return httpResult.Html;

        }
        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }


    }
}