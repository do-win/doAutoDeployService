using doAutoDeployService.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace doAutoDeployService.Models.Controller
{
    [Route("doAutoDeploy/AutoDeploy_N_Notify")]
    public class AutoDeploy_N_NotifyController: ApiController
    {
        //public async Task<IHttpActionResult> Get() {
        //    return Ok("成功");
        //}


        public async Task<IHttpActionResult> Post([FromBody]N_BuildPostVm model)
        {
            if (!ModelState.IsValid) throw new Exception("请求参数不正确");
            Dictionary<string, object> _dictData = new Dictionary<string, object>();
            _dictData.Add("TimeStamp", model.TimeStamp);
            _dictData.Add("Url", model.Url);
            _dictData.Add("ProjectId", model.ProjectId);
            _dictData.Add("Unit", model.Unit);
            var _sign = SignData.Md5SignDict("1234567890", _dictData);

            if (_sign != model.Sign) throw new Exception("签名不正确");

            //验证通过，下载Qiniu上面的zip文件
            string _savePath = Path.Combine(Constants.Temp, Path.GetFileName(model.Url));
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(model.Url, _savePath);

            //判断文件是否存在
            if (!IOUtils.FileExists(_savePath))
            { //表示文件下载失败
                throw new Exception(model.Url + " 下载失败");
            }

            //解压文件
            ZipFile.ExtractToDirectory(_savePath, Constants.Temp);

            //删除zip文件
            FileUtils.DeleteFile(_savePath);

            string _sourcePath = Path.Combine(Constants.Temp, model.ProjectId, model.Unit);
            string _targetPath = Path.Combine(Constants.RootPath, model.ProjectId, model.Unit);

            if (!IOUtils.DirExists(_targetPath))
            {
                IOUtils.CreateDir(_targetPath);
            }

            FileUtils.CopyDir(_sourcePath, _targetPath);

            FileUtils.DeleteDir(Path.Combine(Constants.Temp, model.ProjectId));

            //判断是否存在build.sh脚本文件，如果有就执行
            string shellFilePath = Path.Combine(_targetPath, Constants.ScripteFile);
            if (IOUtils.FileExists(shellFilePath))
            {
                int _code = CMDUtils.Execute(shellFilePath);
                if (_code == 0)
                {
                    //_logEngin.Debug("build " + _slnFile + " Success");
                }
                else
                {
                    //_logEngin.Debug("build " + _slnFile + " Fail");
                }
            }

            return Ok();
        }


        public class N_BuildPostVm
        {
            /// <summary>
            /// 时间戳
            /// </summary>
            public string TimeStamp { get; set; }

            /// <summary>
            /// 签名
            /// </summary>
            public string Sign { get; set; }


            /// <summary>
            /// Qiniu下载地址
            /// </summary>
            public string Url { get; set; }


            /// <summary>
            /// 发布的模块
            /// </summary>
            public string Unit { get; set; }

               /// <summary>
            /// 发布的模块
            /// </summary>
            public string ProjectId { get; set; }
            
        }
    }
}