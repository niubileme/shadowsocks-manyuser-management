using SSMM.Cache;
using SSMM.Helper;
using SSMM.Model;
using SSMM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.UserCenter.Controllers
{
    public class TradeController : BaseController
    {
        /// <summary>
        /// 充值
        /// </summary>
        public ActionResult Recharge()
        {
            var AlipayAccount = SettingCache.Cache.Get(SettingFlag.AlipayAccount);
            ViewBag.AlipayAccount = AlipayAccount;
            return View();
        }

        [HttpPost]
        public ActionResult CheckTradeNo()
        {
            var email = CookieHelper.Email;
            var tradeno = RequestHelper.GetValue("tradeno");
            if (string.IsNullOrEmpty(tradeno))
            {
                return Json(new { result = false, info = "交易号不能为空！" }, JsonRequestBehavior.DenyGet);
            }
            var info = "";
            var result = TradeService.AccountRecharge(email, tradeno, out info);
            return Json(new { result = result, info = info }, JsonRequestBehavior.DenyGet);
        }


        /// <summary>
        /// 订单详情
        /// </summary>
        public ActionResult Order(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("List", "Product");
            var product = ProductService.Query(id);
            if (product == null)
                return RedirectToAction("List", "Product");
            ViewData["Product"] = product;
            return View();
        }

        /// <summary>
        /// 优惠码
        /// </summary>
        [HttpPost]
        public JsonResult CheckPCode()
        {
            var pcode = RequestHelper.GetValue("pcode");
            if (string.IsNullOrEmpty(pcode))
                return Json(new { result = false, info = "优惠码不能为空！" }, JsonRequestBehavior.DenyGet);
            var info = "";
            decimal amount = 0;
            var couponcode = new CouponCodeDto();
            var result = CouponCodeService.Query(pcode, out couponcode);
            if (result)
            {
                amount = couponcode.Amount;
                info = $"优惠码有效！优惠金额：{amount}";
            }
            else
            {
                info = "优惠码已过期或已超过最大使用次数！";
                amount = 0;
            }
            return Json(new { result = result, info = info, amount = amount }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        public ActionResult PayMent()
        {
            var uid = RequestHelper.GetValue("uid");
            var pid = RequestHelper.GetValue("pid");
            var pwd = RequestHelper.GetValue("pwd");
            var pcode = RequestHelper.GetValue("pcode");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pwd))
                return RedirectToAction("List", "Product");
            //判断服务是否已在运行
            var user = UserCache.Cache.GetValue(uid);
            var IsRunning = SSService.IsRunning(user.Id);
            if (IsRunning)
            {
                return View("PayMentResultNotice", new ResponseResult() { Result = false, Info = "购买失败！请再当前服务过期后，再进行购买！" });
            }
            var timestamp = FormatHelper.ConvertDateTimeInt(DateTime.Now);//时间戳
            var sign = TradeService.ParameterSign(uid, pid, pwd, pcode, timestamp);//参数签名
            var alipaycode = TradeService.GetAlipayTransferCode(uid); //支付宝唯一支付码
            SessionHelper.SetValue("AlipayCode", alipaycode);
            ViewData["Uid"] = uid;
            ViewData["Pid"] = pid;
            ViewData["Pwd"] = pwd;
            ViewData["Pcode"] = pcode;
            ViewData["TimeStamp"] = timestamp;
            ViewData["Sign"] = sign;
            return View();
        }

        /// <summary>
        ///  获取支付方式 进行跳转
        /// </summary>
        [HttpPost]
        public ActionResult PayMentPost()
        {
            var uid = RequestHelper.GetValue("uid");
            var pid = RequestHelper.GetValue("pid");
            var pwd = RequestHelper.GetValue("pwd");
            var pcode = RequestHelper.GetValue("pcode");
            var timestamp = RequestHelper.GetInt("timestamp");
            var sign = RequestHelper.GetValue("sign");
            var type = RequestHelper.GetInt("type");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(sign) || timestamp == 0)
                return RedirectToAction("List", "Product");
            var paymentType = (PaymentType)type;
            var obj = new { uid = uid, pid = pid, pwd = pwd, pcode = pcode, timestamp = timestamp, sign = sign };
            switch (paymentType)
            {
                case PaymentType.账户余额:
                    return RedirectToAction("AccountBalancePayMent", obj);
                case PaymentType.支付宝转账:
                    return RedirectToAction("AlipayTransferPayMent", obj);
                //case PaymentType.微信转账:
                //    break;
                //case PaymentType.支付宝支付:
                //    break;
                //case PaymentType.微信支付:
                //    break;
                default:
                    return RedirectToAction("List", "Product");
            }

        }


        public ActionResult PayMentResultNotice()
        {
            return View();
        }

        /// <summary>
        /// 账户余额支付
        /// </summary>
        public ActionResult AccountBalancePayMent()
        {
            var uid = RequestHelper.GetValue("uid");
            var pid = RequestHelper.GetValue("pid");
            var pwd = RequestHelper.GetValue("pwd");
            var pcode = RequestHelper.GetValue("pcode");
            var timestamp = RequestHelper.GetInt("timestamp");
            var sign = RequestHelper.GetValue("sign");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(sign) || timestamp == 0)
                return RedirectToAction("List", "Product");
            //判断服务是否已在运行
            var user = UserCache.Cache.GetValue(uid);
            var IsRunning = SSService.IsRunning(user.Id);
            if (IsRunning)
            {
                return View("PayMentResultNotice", new ResponseResult() { Result = false, Info = "购买失败！请再当前服务过期后，再进行购买！" });
            }
            var info = "";
            var result = TradeService.PayMentForAccountBalance(uid, pid, pwd, pcode, timestamp, sign, out info);
            return View("PayMentResultNotice", new ResponseResult() { Result = result, Info = info });
        }

        /// <summary>
        /// 支付宝转账
        /// </summary>
        public ActionResult AlipayTransferPayMent()
        {
            var uid = RequestHelper.GetValue("uid"); ;
            var pid = RequestHelper.GetValue("pid");
            var pwd = RequestHelper.GetValue("pwd");
            var pcode = RequestHelper.GetValue("pcode");
            var timestamp = RequestHelper.GetInt("timestamp");
            var sign = RequestHelper.GetValue("sign");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(sign) || timestamp == 0)
                return RedirectToAction("List", "Product");
            //判断服务是否已在运行
            var user = UserCache.Cache.GetValue(uid);
            var IsRunning = SSService.IsRunning(user.Id);
            if (IsRunning)
            {
                return View("PayMentResultNotice", new ResponseResult() { Result = false, Info = "购买失败！请再当前服务过期后，再进行购买！" });
            }
            ViewData["Uid"] = uid;
            ViewData["Pid"] = pid;
            ViewData["Pwd"] = pwd;
            ViewData["PCode"] = pcode;
            ViewData["TimeStamp"] = timestamp;
            ViewData["Sign"] = sign;
            //支付宝支付码
            var paycode = SessionHelper.GetValue("AlipayCode");
            if (paycode == null)
                return View("PayMentResultNotice", new ResponseResult() { Result = false, Info = "支付已过期！请重新选择支付！" });
            ViewData["PayCode"] = paycode;
            //校验参数
            var info = "";
            decimal amount = 0;
            if (!TradeService.CheckParameterSign(uid, pid, pwd, pcode, timestamp, sign, PaymentType.支付宝转账, out info, out amount))
            {
                return View("PayMentResultNotice", new ResponseResult() { Result = false, Info = info });
            }
            if (amount == 0)
            {
                //直接生成订单
                if (TradeService.PayMentForAlipayTransfer(uid, pid, pwd, pcode, out info))
                    return View("PayMentResultNotice", new ResponseResult() { Result = true, Info = info });
                else
                    return View("PayMentResultNotice", new ResponseResult() { Result = true, Info = info });
            }
            ViewData["Amount"] = amount.ToString("0.00");
            return View();
        }

        [HttpPost]
        public JsonResult CheckAlipayTransfer()
        {
            var uid = RequestHelper.GetValue("uid"); 
            var pid = RequestHelper.GetValue("pid");
            var pwd = RequestHelper.GetValue("pwd");
            var pcode = RequestHelper.GetValue("pcode");
            var timestamp = RequestHelper.GetInt("timestamp");
            var sign = RequestHelper.GetValue("sign");
            var paycode = RequestHelper.GetValue("paycode");
            var amount = RequestHelper.GetDecimal("amount");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(sign) || timestamp == 0 || string.IsNullOrEmpty(paycode))
                return Json(new { result = false, info = "参数错误" }, JsonRequestBehavior.DenyGet);
            var serverPayCode = SessionHelper.GetValue("AlipayCode");
            if (serverPayCode == null || serverPayCode.ToString() != paycode)
            {
                return Json(new { result = false, info = "支付已过期或已失效！" }, JsonRequestBehavior.DenyGet);
            }
            decimal serverAmount = 0;
            string serverRemark = "";
            if (TradeService.QueryAlipayRemarkSearchApi(paycode, out serverAmount, out serverRemark))
            {
                if (serverRemark == paycode && serverAmount == amount)
                {
                    var info = "";
                    //生成订单
                    if (TradeService.PayMentForAlipayTransfer(uid, pid, pwd, pcode, out info))
                        return Json(new { result = true, info = info }, JsonRequestBehavior.DenyGet);
                    else
                        return Json(new { result = false, info = info }, JsonRequestBehavior.DenyGet);
                }
                else
                    return Json(new { result = false, info = "付款金额与备注不匹配！" }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { result = false, info = serverRemark }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}