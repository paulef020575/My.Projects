using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using My.Projects.Classes.References;

namespace My.Projects.Web.Controllers
{
    [Microsoft.AspNetCore.Components.Route("API/[controller]")]
    [ApiController]
    public class PersonController : BaseController<Person>
    {
        public PersonController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
