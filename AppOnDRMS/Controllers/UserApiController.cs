using DRMS_Models;
using User_BL;

namespace AppOnDRMS.Controllers
{
    public class UserApiController : Controllers
    {
        // GET: UserApi
        [HttpPost]
        [ActionName("GetUser")]
        public ActionResult GetUser([FromBody] UserModel userModel)
        {
            UserBL userBL = new UserBL();
            return Ok(userBL.GetUser(userModel));
        }
    }
}