using Aequor.Main;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Util
{

    internal class ShanBayUtil
    {

        private ShanBayUtil() { }

        public static bool IsStudyFinished()
        {
            try
            {
                return JObject.Parse(HttpsUtil.SendGetString("https://apiv3.shanbay.com/wordsapp/user_material_books/" + JObject.Parse(HttpsUtil.SendGetString("https://apiv3.shanbay.com/wordsapp/user_material_books/current", CookieUtil.GetCookiesByDomain(".shanbay.com"))!)["materialbook_id"]!.Value<string>() + "/learning/statuses", CookieUtil.GetCookiesByDomain(".shanbay.com"))!)["is_finished"]!.Value<bool>();
            } catch
            {
                return true;
            }
        }

    }

}
