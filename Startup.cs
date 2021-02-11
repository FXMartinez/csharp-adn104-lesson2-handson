using System.Diagnostics.SymbolStore;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lesson2_HandsOn
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        /*
        Add the name that is being passed to the query string to the cookie and display it on the page.
        Add a conditional that will make the default name equal to your name if no name is entered in the query string.
        */

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Run (async (context) => {

                #region these didnt work due to what i assume was async coming up empty during if statement
                var q = context.Request.Query; // entire query value from the url // address bar after the question mark
                var fn = q["firstname"].ToString();
                var ln = q["lastname"].ToString();
                var dfn = "Felix";
                var dln = "Martinez";
                #endregion

                #region creating operations to split and filter a queryString, which ended up not mattering
                /*
                var someParams = context.Request.QueryString.ToString();
                string[] splitParams = someParams.Replace("?", string.Empty).Split("&");
                
                string joinParams = string.Join(" ", splitParams);
                // string[] yetAnotherParams = joinParams.Split(new Char[] {"\n", "="});
                // Console.WriteLine(splitParams);

                foreach (var param in splitParams) {
                    Console.WriteLine($"inside foreach {param}");
                } 
                */
                #endregion

                var cookie = context.Request.Cookies["NameCookie"];

                // checks if the value of the key nameCookie in cookie variable has a string or is empty.
                if (string.IsNullOrWhiteSpace(cookie)) 
                {
                    // context.Response.Redirect("localhost:5001/?firstname=Felix&lastname=Martinez&age=35"); this doesnt work either way probably overthinking it
                    DateTime now = DateTime.Now;
                    DateTime expires = now + TimeSpan.FromSeconds(15);

                    context.Response.Cookies.Append
                    (
                        "NameCookie", // key
                        #region verbose way
                        // $"<p> first name: { (fn != "" ? fn : dfn) } </p>" +  this is left over from when I tried to declare variable that didnt work
                        // $"<p> first name: { (context.Request.Query["firstname"].ToString() == "" ? "Felix" : context.Request.Query["firstname"].ToString() ) } </p>" + 
                        // $"<p> last name: { (context.Request.Query["lastname"].ToString() == "" ? "Martinez" : context.Request.Query["lastname"].ToString() ) } </p>" +
                        #endregion
                        $"<p> first name: { (fn == "" ? dfn : fn) } </p>" + 
                        $"<p> last name: { (ln == "" ? dln : ln ) } </p>" +
                        $"<p> Cookie created at: {now.ToString("h.mm.ss tt")} </p>",

                        new CookieOptions {
                            Path = "/",
                            HttpOnly = false,
                            Secure = false,
                            Expires = expires
                        } // third parameter cookieOptions
                    );
                }
                #region big mistake making this as I didnt realize til much later what this if statement was actually for
                // else 
                // { 
                //     DateTime now = DateTime.Now;
                //     DateTime expires = now + TimeSpan.FromSeconds(15);

                //     // Console.WriteLine($"ic first name: {fn}");
                //     // Console.WriteLine($"ic last name: {ln}");

                //     context.Response.Cookies.Append
                //     (
                //         "NameCookie", // key

                //         $"<p> else statement </p>" + 
                //         $"<p> else statement </p>" +
                //         $"<p> Cookie created at: {now.ToString("h.mm.ss tt")} </p>",
                //         //$"<p> {context.Request.Query ? context.Request.Query[firstname]}" +
                //         //$"{context.Request.Query[lastname]} </p>",

                //         new CookieOptions {
                //             Path = "/",
                //             HttpOnly = false,
                //             Secure = false,
                //             Expires = expires
                //         } // third parameter cookieOptions
                //     );
                // } 
                #endregion
                

                string response = 
                    "<h1> HTTP Cookies </h1>" +
                    $"<p>This is the cookie value received from browser: \"<strong>{cookie}</strong>\".</p>" +
                    "<p>Refresh page to see current cookie value...</p>" +
                    "<p>Cookie expires after 15 seconds.</p>" +
                    "<h1>Query String Parameters</h1>" +"<p>Enter a URL like:</p>" + "<a href=\"https://localhost:5001/?firstname=Jane&lastname=Smith&age=30\">" + "http://localhost:5001/?firstname=Jane&lastname=Smith&age=30</a>";

                foreach (var queryParameter in context.Request.Query) {
                    response += $"<p> {queryParameter} </p>";
                }
    
                await context.Response.WriteAsync(response);
            });


        } // end of configure method
    }
}

#region reference code
// var qs = context.Request.QueryString;
// var parsed = HttpUtility.ParseQueryString(qs);

// string response = 
//     "<h1> HTTP Cookies </h1>" +
//     $"<p>This is the cookie value received from browser: \"<strong>{cookie}</strong>\".</p>" +
//     "<p>Refresh page to see current cookie value...</p>" +
//     "<p>Cookie expires after 15 seconds.</p>";

// string response = "<h1>Query String Parameters</h1>" +"<p>Enter a URL like:</p>" + "<a href=\"https://localhost:5001/?firstname=Jane&lastname=Smith&age=30\">" + "http://localhost:5001/?firstname=Jane&lastname=Smith&age=30</a>";
#endregion

#region old useless code
// string name;

// if (context.Request.Query == null) {
//     name = "Felix Martinez";
// } else {
//     foreach (var queryParameter in context.Request.Query) {
//         queryResponse += $"<p> {queryParameter} </p>";
//     }
// }
#endregion

#region my condition to check for querystring null value
// Console.WriteLine(cookie);
// if (context.Request.Query == null) {
//     // Console.WriteLine("no string value");
//     var test = context.Request.Cookies["NameCookie"].ToString();
//     foreach (var cv in test) {
//         Console.WriteLine(cv);
//     }
// } else {
//     Console.WriteLine(context.Request.Query);
//     // foreach (var queryParameter in context.Request.Query) {
//     //     Console.WriteLine(queryParameter);
//     // }
// } // checking for null query values
#endregion


#region cookie code
// cookie code
// app.Run (async (context) => 
// {
//     var cookie = context.Request.Cookies["MyCoolLittleCookie"];

//     if (string.IsNullOrWhiteSpace(cookie)) 
//     {
//         DateTime now = DateTime.Now;
//         DateTime expires = now + TimeSpan.FromSeconds(15);
//         context.Response.Cookies.Append
//         (
//             "MyCoolLittleCookie",
//             "Cookie created at: " + now.ToString("h.mm.ss tt"),
//             new CookieOptions {
//                 Path = "/",
//                 HttpOnly = false,
//                 Secure = false,
//                 Expires = expires
//             }
//         );
//     }

//     string response = 
//         "<h1> HTTP Cookies </h1>" +
//         $"<p>This is the cookie value received from browser: \"<strong>{cookie}</strong>\".</p>" +
//         "<p>Refresh page to see current cookie value...</p>" +
//         "<p>Cookie expires after 15 seconds.</p>";
//     await context.Response.WriteAsync(response);
            // });
#endregion
#region 
// if (env.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }

// app.UseRouting();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapGet("/", async context =>
//     {
//         await context.Response.WriteAsync("Hello World!");
//     });
// });
#endregion