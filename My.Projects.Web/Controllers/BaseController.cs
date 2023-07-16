using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPV.Data.DataItems;
using Microsoft.Extensions.Configuration;
using My.Projects.Web.Classes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace My.Projects.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TDataItem> : ControllerBase
        where TDataItem : DataItem, new()
    {
        private IConfiguration _configuration;
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // GET: api/<BaseController>
        [HttpGet]
        public async Task<IList<TDataItem>> Get()
        {
            MyProjectsConnection connection 
                = new MyProjectsConnection(_configuration.GetConnectionString("MyProjectsConnection"));
            return await connection.LoadList<TDataItem>();
        }

        // GET api/<BaseController>/5
        [HttpGet("{id}")]
        public async Task<TDataItem> Get(Guid id)
        {
            MyProjectsConnection connection
                = new MyProjectsConnection(_configuration.GetConnectionString("MyProjectsConnection"));
            TDataItem dataItem = new TDataItem {Id = id};
            await connection.Load(dataItem);
            return dataItem;
        }

        // POST api/<BaseController>
        [HttpPost]
        public async Task Post([FromBody] TDataItem dataItem)
        {
            MyProjectsConnection connection
                = new MyProjectsConnection(_configuration.GetConnectionString("MyProjectsConnection"));
            await connection.Save(dataItem);
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            TDataItem dataItem = new TDataItem {Id = id};

            MyProjectsConnection connection
                = new MyProjectsConnection(_configuration.GetConnectionString("MyProjectsConnection"));
            await connection.Delete(dataItem);
        }
    }
}
