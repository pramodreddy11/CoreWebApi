using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        HotelManagementContext context = new HotelManagementContext();
        private static readonly string _adminAuth = "adminfhskdjhfsdgfhksdgfhsdgfh3sgdf";
        private static readonly string _loginAuth = "userdbfhbdfhbdfjhasjfhv0";

        [HttpGet]
        public ActionResult<List<Items>> GetItems()

        {
            StringValues headerValues;
            if (Request.Headers.TryGetValue("AuthCode", out headerValues))
            {
               
            }

            AuthenticateAndAuthorize flag = Authenticate(headerValues);

            if (flag.Authenticate = true && (flag.Role == "admin" || flag.Role=="user"))
                return Ok(context.Items.ToList());
            else
                return Unauthorized();
        }

        [HttpPost("Login")]
        public IActionResult LoginForm([FromBody] LoginDetails obj)
        {
            UserModel userObj = new UserModel();

            if (obj.userName != null && obj.password != null)
            {

                var result = context.Customer.Where(cust => cust.Email == obj.userName && cust.Password == obj.password).FirstOrDefault();
                if (result == null)
                {
                    return Unauthorized("Please provide the valid Details");

                }
                else
                {
                    if (result.CustRole == "admin")
                    {
                        userObj.CustId = result.CustId;
                        userObj.CustName = result.CustName;
                        userObj.CustPhoneNo = result.CustPhoneNo;
                        userObj.CustAge = result.CustAge;
                        userObj.CustRole = result.CustRole;
                        userObj.AuthCode = "adminfhskdjhfsdgfhksdgfhsdgfh3sgdf";
                        return Ok(userObj);


                    }
                    else if (result.CustRole == "user")
                    {
                        userObj.CustId = result.CustId;
                        userObj.CustName = result.CustName;
                        userObj.CustPhoneNo = result.CustPhoneNo;
                        userObj.CustAge = result.CustAge;
                        userObj.CustRole = result.CustRole;
                        userObj.AuthCode = "userdbfhbdfhbdfjhasjfhv0";
                        return Ok(userObj);


                    }
                    else
                    {
                        return NoContent();
                    }
                }

            }
            return Unauthorized("Please provide the valid Details");

        }
        [HttpGet("{id}")]
        public ActionResult<List<Items>> GetItemsById(int id)
        {
            var re = Request;
            var headers = re.Headers;
            StringValues x = default(StringValues);
            if (headers.ContainsKey("AuthCode"))
            {
                var m = headers.TryGetValue("AuthCode", out x);
            }

            AuthenticateAndAuthorize flag = Authenticate(x);

            if (flag.Authenticate = true && (flag.Role == "admin" || flag.Role == "user"))
                return Ok(context.Items.Where(item => item.ItemId == id).ToList());

            else
                return Unauthorized("Please contact the system administrator for the active access");

        }
        [HttpPost("AddItems")]

        public ActionResult<List<Items>> AddItems([FromBody] Items obj)
        {
            try {
                var re = Request;
                var headers = re.Headers;
                StringValues x = default(StringValues);
                if (headers.ContainsKey("AuthCode"))
                {
                    var m = headers.TryGetValue("AuthCode", out x);
                }

                AuthenticateAndAuthorize flag = Authenticate(x);

                if (flag.Authenticate = true && flag.Role == "admin") 
                {
                    Items item = new Items();
                    if (obj != null)
                    {
                        if (obj.ItemId == 0)
                        {
                            item.ItemName = obj.ItemName;
                            context.Items.Add(item);
                            context.SaveChanges();
                            return Ok(context.Items.ToList());
                        }
                        else
                        {
                            Items value = context.Items.Where(item => item.ItemId == obj.ItemId).FirstOrDefault();
                            value.ItemName = obj.ItemName;
                            context.Items.Update(value);
                            context.SaveChanges();
                            return Ok(context.Items.ToList());
                        }
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else
                {
                    return Unauthorized(" Please contact the system administrator for the active access");
                }
            } catch (Exception e) {

                return NotFound(e);
            }

        }

        [HttpDelete("Delete")]
        public ActionResult DeleteItem(Items obj)
        {
            try
            {
                var re = Request;
                var headers = re.Headers;
                StringValues delAuth = default(StringValues);
                if (headers.ContainsKey("AuthCode"))
                {
                    var m = headers.TryGetValue("AuthCode", out delAuth);
                }

                AuthenticateAndAuthorize flag = Authenticate(delAuth);
                if (flag.Authenticate = true && flag.Role == "admin")
                {
                    if (obj != null) { 
                    Items value = context.Items.Where(item => item.ItemId == obj.ItemId).FirstOrDefault();
                        if (value != null)
                        {
                            context.Items.Remove(value);
                            context.SaveChanges();

                            return Ok(context.Items.ToList());
                        }
                        else
                        {
                            return NoContent();
                        }
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                return Unauthorized(" Please contact the system administrator for the active access");
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        public AuthenticateAndAuthorize Authenticate(StringValues x)
        {
            AuthenticateAndAuthorize token = new AuthenticateAndAuthorize();
            if (x == _adminAuth)

            {
                token.Authenticate = true;
                token.Role = "admin";
                return token;

            }

            else if (x == _loginAuth )
            {


                token.Authenticate = true;
                token.Role = "user";
                return token;
            }
            else
            {

                token.Authenticate = false;
                token.Role = "No User Found";
                return token;
            }
        }



    }


    
}
